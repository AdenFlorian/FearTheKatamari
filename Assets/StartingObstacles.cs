using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartingObstacles : MonoBehaviour {
	public GameObject prefab;

	void Start() {
		GameMaster.GameRestarted += () => {
			GameObject.Destroy(transform.GetChild(0).gameObject);
			gameObject.InstantiateEx(prefab, prefab.transform).transform.parent = transform;
		};
	}

	void Update() {
		
	}
}
