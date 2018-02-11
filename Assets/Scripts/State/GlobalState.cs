using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalState : MonoBehaviour {
	public static event Action<State> StateChanged;
	
	static State state;

	void Awake() {
	}

	public static State GetState() {
		return state;
	}

	public static void ChangeKatamariSize(float newSize) {
		GlobalState.Dispatch(new ChangeKatamariSize(){newSize = newSize});
	}

	public static void ResetKatamariSize() {
		GlobalState.Dispatch(new ResetKatamariSize());
	}

	static void Dispatch(XAction action) {
		var actionDispatchLog = new ActionDispatched(){
			action = action,
			previousState = state
		};
		var newState = state;

		newState.katamari = KatamariState.Reduce(state.katamari, action);

		actionDispatchLog.afterState = newState;

		state = newState;

		StateChanged.Invoke(state);

		Debug.Log("DISPATCH: " + actionDispatchLog.ToString());
	}
}

[Serializable]
public struct State {
	public KatamariState katamari;
}

public class XAction {
}

public struct ActionDispatched {
	public XAction action;
	public State previousState;
	public State afterState;

	public string SerializeAction() {
		return JsonUtility.ToJson(action);
	}

	public override string ToString() {
		return action.GetType().ToString()
			+ "\naction: " + SerializeAction()
			+ "\npreviousState: " + JsonUtility.ToJson(previousState)
			+ "\nnewState: " + JsonUtility.ToJson(afterState);
	}
}
