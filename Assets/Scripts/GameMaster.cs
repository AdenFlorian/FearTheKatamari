using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour {
	public static Player Player;
	public static bool isGameOver {get; private set;}

	public GameObject PlayerPrefab;
	public Transform SpawnTransform;
	public GameObject GameOverUI;

	bool hasStarted = false;

	void Start() {
		isGameOver = false;
		hasStarted = true;
		Player = GameObject.Instantiate(PlayerPrefab, SpawnTransform).GetComponent<Player>();
		Spawner.I.Start();
		GameOverUI.SetActive(false);
	}

	void Update() {
		if (hasStarted && Player == null && isGameOver == false) {
			OnGameOver();
		}
	}

	void OnGameOver() {
		Debug.Log("game over");
		isGameOver = true;
		Spawner.I.spawning = false;
		GameOverUI.SetActive(true);
	}

	public void Again() {
		Debug.Log("Again");
		this.Start();
	}
}
