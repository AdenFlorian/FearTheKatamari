using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour {
	GameObject Player;
	public GameObject PlayerPrefab;
	public Transform SpawnTransform;
	bool isGameOver = false;
	public GameObject GameOverUI;
	bool hasStarted = false;

	public static GameMaster I;

	void Awake() {
		I = this;
	}

	// Use this for initialization
	void Start() {
		Player = GameObject.Instantiate(PlayerPrefab, SpawnTransform.position, SpawnTransform.rotation);
		isGameOver = false;
		Spawner.I.Start();
		GameOverUI.SetActive(false);
		hasStarted = true;
	}

	// Update is called once per frame
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
