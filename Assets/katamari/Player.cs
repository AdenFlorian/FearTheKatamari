using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
	public Transform katamari;

	[SerializeField]
	float jumpForce = 1;
	[SerializeField]
	float moveForce = 1;
	[SerializeField]
	float rotationForce = 1;
	[SerializeField]
	float maxRotationSpeed = 10;

	public Vector2 CenterOfMass => rigidbody2D.worldCenterOfMass;
	
	new Rigidbody2D rigidbody2D;
	bool canJump = true;
	float lastJumpTime;

	void Awake() {
		rigidbody2D = GetComponent<Rigidbody2D>();
		GlobalState.ResetKatamariSize();
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
		if (rigidbody2D.angularVelocity > maxRotationSpeed) {
			rigidbody2D.angularVelocity = maxRotationSpeed;
		} else if 	(rigidbody2D.angularVelocity < -maxRotationSpeed) {
			rigidbody2D.angularVelocity = -maxRotationSpeed;
		}
	}

	void OnCollisionEnter2D(Collision2D collision) {
		var obstacle = collision.gameObject.GetComponent<Obstacle>();
		if (obstacle != null) {
			OnCollisionEnterWithObstacle(obstacle);
		}
	}

	void OnCollisionStay2D(Collision2D collision) {
		if (collision.gameObject.tag == "Ground") {
			OnCollisionStayWithGround();
		}
	}

	void OnCollisionEnterWithObstacle(Obstacle obstacle) {
		GameObject.Destroy(obstacle.gameObject.GetComponent<Rigidbody2D>());
		obstacle.transform.parent = katamari;
		ChangeSizeIfNeeded(obstacle);
	}

	void OnCollisionStayWithGround() {
		if (canJump == false && Time.time - lastJumpTime > 1) {
			canJump = true;
		}
	}

	void ChangeSizeIfNeeded(Obstacle obstacle) {
		var greatestDistance = GetGreatestDistanceToExistingObstacles(obstacle);
		if (greatestDistance > GlobalState.GetState().katamari.size) {
			GlobalState.ChangeKatamariSize(greatestDistance);
		}		
	}

	float GetGreatestDistanceToExistingObstacles(Obstacle obstacle) {
		var greatestDistance = 0f;

		for (int i = 0; i < katamari.childCount; i++) {
			var child = katamari.GetChild(i);
			if (child.tag != Tags.Obstacle) continue;
			
			var distance = GetDistance(obstacle.transform, child.transform);

			if (distance > greatestDistance) {
				greatestDistance = distance;
			}
		}

		return greatestDistance;
	}
	
	float GetDistance(Transform a, Transform b) {
		return (a.position - b.position).magnitude;
	}
}
