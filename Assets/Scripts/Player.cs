using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
	public Transform StuckStuffParent;

	public float jumpForce = 1;
	public float moveForce = 1;
	public float rotationForce = 1;
	public float maxRotationSpeed = 10;

	public Vector2 CenterOfMass {
		get {
			var position2D = new Vector2(StuckStuffParent.localPosition.x, StuckStuffParent.localPosition.y);
			return StuckStuffParent.localToWorldMatrix.MultiplyPoint(position2D + VectorToCenterOfMass);
		}
	}
	Vector2 VectorToCenterOfMass = Vector2.zero;
	int countOfMassObjects = 1;
	Vector2 sumOfObjectPositions = Vector2.zero;
	
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

	void OnCollisionEnter2D(Collision2D collision) {
		if (collision.gameObject.GetComponent<Obstacle>() != null) {
			Debug.Log("collided with Player");
			GameObject.Destroy(collision.gameObject.GetComponent<Rigidbody2D>());
			collision.transform.parent = StuckStuffParent;
			UpdateCenterOfMass(collision.transform);
		}
	}

	void UpdateCenterOfMass(Transform newThing) {
		countOfMassObjects++;
		var newPosition2D = new Vector2(newThing.localPosition.x, newThing.localPosition.y);
		sumOfObjectPositions += newPosition2D;
		VectorToCenterOfMass = sumOfObjectPositions / countOfMassObjects;
	}
}
