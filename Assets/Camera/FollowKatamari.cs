using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowKatamari : MonoBehaviour {
	public float speed = 1;

	Player Player;
	Vector3 desiredPosition;

	void Start() {
		Player = GameMaster.Player.GetComponent<Player>();
		UpdateDesiredPosition();
	}
	
	void Update() {
		if (GameMaster.isGameOver) return;
		if (Player == null) {
			Player = GameMaster.Player.GetComponent<Player>();
		}
		UpdateDesiredPosition();

		var difference = desiredPosition - transform.position;

		transform.position += difference * speed * Time.deltaTime;
	}

	void UpdateDesiredPosition() {
		desiredPosition = new Vector3(){
			x = Player.CenterOfMass.x,
			y = Player.CenterOfMass.y,
			z = transform.position.z
		};
	}
}
