using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowKatamari : MonoBehaviour {
	Player Player;

	// Use this for initialization
	void Start () {
		Player = GameMaster.I.Player.GetComponent<Player>();
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = new Vector3(){
			x = Player.CenterOfMass.x,
			y = Player.CenterOfMass.y,
			z = transform.position.z
		};
	}
}
