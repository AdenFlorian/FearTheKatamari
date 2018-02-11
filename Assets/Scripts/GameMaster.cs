using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour {
	public static Player Player;
	public static bool isGameOver {get; private set;}

	public GameObject PlayerPrefab;
	public Transform SpawnTransform;
	public GameObject GameOverUI;

	public static event Action GameOver;
	public static event Action GameStarted;
	public static event Action GameRestarted;

	bool hasStarted = false;

	void Start() {
		isGameOver = false;
		hasStarted = true;
		Player = gameObject.InstantiateEx(PlayerPrefab, SpawnTransform).GetComponent<Player>();
		GameOverUI.SetActive(false);
		GameStarted?.Invoke();
	}

	void Update() {
		if (hasStarted && Player == null && isGameOver == false) {
			OnGameOver();
		}
	}

	void OnGameOver() {
		Debug.Log("game over");
		isGameOver = true;
		GameOverUI.SetActive(true);
		GameOver?.Invoke();
	}

	public void Again() {
		Debug.Log("Again");
		this.Start();
		GameRestarted?.Invoke();
	}
}
