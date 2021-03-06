﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSizer : MonoBehaviour {
	public float speed = 0.5f;
	public float sizeModifier = 1f;

	new Camera camera;
	float targetSize;

	void Awake() {
		camera = GetComponent<Camera>();
		GlobalState.StateChanged += (newState) => {
			targetSize = (GlobalState.GetState().katamari.size + 1);
		};
	}

	void Start() {
	}
	
	void Update() {
		var difference = targetSize * sizeModifier - camera.orthographicSize;
		camera.orthographicSize += difference * speed * Time.deltaTime;
	}
}
