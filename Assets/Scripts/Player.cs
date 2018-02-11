﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
	public float jumpForce = 1;
	public float moveForce = 1;
	public float rotationForce = 1;
	public float maxRotationSpeed = 10;
	
	new Rigidbody2D rigidbody2D;

	void Awake() {
		rigidbody2D = GetComponent<Rigidbody2D>();
	}

	void Start() {

	}

	void Update() {
		if (Input.GetKeyDown(KeyCode.Space)) {
			rigidbody2D.AddForce(new Vector3(0, jumpForce, 0), ForceMode2D.Impulse);
		}
	}

	void FixedUpdate() {
		if (Input.GetKey(KeyCode.D)) {
			rigidbody2D.AddForce(new Vector3(moveForce, 0, 0), ForceMode2D.Force);
			rigidbody2D.AddTorque(-rotationForce);
		}
		if (Input.GetKey(KeyCode.A)) {
			rigidbody2D.AddForce(new Vector3(-moveForce, 0, 0), ForceMode2D.Force);
			rigidbody2D.AddTorque(rotationForce);
		}
		if 	(rigidbody2D.angularVelocity > maxRotationSpeed) {
			rigidbody2D.angularVelocity = 10;
		} else if 	(rigidbody2D.angularVelocity < -maxRotationSpeed) {
			rigidbody2D.angularVelocity = -10;
		}
	}
}
