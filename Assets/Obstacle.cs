using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour {
	public bool onPlayer = false;
	
	new Rigidbody2D rigidbody2D;

	void Awake() {
		rigidbody2D = GetComponent<Rigidbody2D>();
	}

	void OnCollisionEnter2D(Collision2D collision) {
		if (onPlayer) return;
		if (collision.gameObject.tag == "Player") {
			onPlayer = true;
			Debug.Log("collided with Player");
			GameObject.Destroy(rigidbody2D);
			transform.parent = collision.transform;
		}
	}
}
