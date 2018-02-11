using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
	public Transform StuckStuffParent;

	[SerializeField]
	float jumpForce = 1;
	[SerializeField]
	float moveForce = 1;
	[SerializeField]
	float rotationForce = 1;
	[SerializeField]
	float maxRotationSpeed = 10;

	public Vector2 CenterOfMass {
		get {
			if (StuckStuffParent == null) return Vector2.zero;
			var position2D = new Vector2(StuckStuffParent.localPosition.x, StuckStuffParent.localPosition.y);
			return StuckStuffParent.localToWorldMatrix.MultiplyPoint(position2D + VectorToCenterOfMass);
		}
	}
	Vector2 VectorToCenterOfMass = Vector2.zero;
	int countOfMassObjects = 1;
	Vector2 sumOfObjectPositions = Vector2.zero;
	
	new Rigidbody2D rigidbody2D;
	bool isGrounded;
	bool canJump = true;
	float lastJumpTime;

	void Awake() {
		rigidbody2D = GetComponent<Rigidbody2D>();
	}

	void Start() {

	}

	void Update() {
		if (Input.GetKeyDown(KeyCode.Space) && canJump) {
			Jump();
		}
	}

	void Jump() {
		canJump = false;
		lastJumpTime = Time.time;
		rigidbody2D.AddForce(new Vector3(0, jumpForce, 0), ForceMode2D.Impulse);
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

	void OnCollisionEnter2D(Collision2D collision) {
		if (collision.gameObject.GetComponent<Obstacle>() != null) {
			OnCollisionEnterWithObstacle(collision);
		} else if (collision.gameObject.tag == "Ground") {
			OnCollisionEnterWithGround();
		}
	}

	void OnCollisionStay2D(Collision2D collision) {
		if (collision.gameObject.tag == "Ground") {
			OnCollisionStayWithGround();
		}
	}

	void OnCollisionExit2D(Collision2D collision) {
		if (collision.gameObject.tag == "Ground") {
			OnCollisionExitWithGround();
		}
	}

	void OnCollisionEnterWithObstacle(Collision2D collision) {
		GameObject.Destroy(collision.gameObject.GetComponent<Rigidbody2D>());
		collision.transform.parent = StuckStuffParent;
		UpdateCenterOfMass(collision.transform);
	}

	void UpdateCenterOfMass(Transform newThing) {
		countOfMassObjects++;
		var newPosition2D = new Vector2(newThing.localPosition.x, newThing.localPosition.y);
		sumOfObjectPositions += newPosition2D;
		VectorToCenterOfMass = sumOfObjectPositions / countOfMassObjects;
	}

	void OnCollisionEnterWithGround() {
		isGrounded = true;
	}

	void OnCollisionStayWithGround() {
		if (canJump == false && Time.time - lastJumpTime > 1) {
			canJump = true;
		}
	}

	void OnCollisionExitWithGround() {
		isGrounded = false;
	}
}
