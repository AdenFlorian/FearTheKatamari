 public struct KatamariState {
    public float size;

    public static KatamariState Reduce(KatamariState state, XAction action) {
        if (action is ChangeKatamariSize) {
            return new KatamariState(){
                size = (action as ChangeKatamariSize).newSize
            };
        } else if (action is ResetKatamariSize) {
            return new KatamariState(){
                size = 0.5f
            };
        }

        return state;
    }
 }

public class ChangeKatamariSize : XAction {
	public float newSize;
}

public class ResetKatamariSize : XAction {
}
