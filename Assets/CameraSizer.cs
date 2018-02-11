using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSizer : MonoBehaviour {
	new Camera camera;
	float startingSize = 5f;

	void Awake() {
		camera = GetComponent<Camera>();
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		camera.orthographicSize = ((KatamariSize.GetSize() + 1) / 2);
		transform.rotation = Quaternion.identity;
	}
}
