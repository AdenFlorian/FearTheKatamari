using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ObstacleType {
	ObstaclePrefab,
	RandomSprites
}

public class Spawner : MonoBehaviour {
	public Transform spawnTransform;
	public float minHeightPos = -1;
	float maxSpawnHeight = 1;
	public float obstaclesPerSecond = 2;
	public float obstacleSize = 1;
	public Sprite[] spritesForRandomSpriteSpawner;
	public GameObject[] obstaclePrefabs;
	public float startForce;

	GameObject obstaclesParent;
	float startTime;
	float elapsedTime = 1;
	public float startingObstaclesPerSecond = 10;
	public float minimumObstaclesPerSecond = 1;
	float currentObstaclesPerSecond;

	void Awake() {
		GlobalState.StateChanged += (newState) => {
			maxSpawnHeight = GlobalState.GetState().katamari.size * 2;
		};
		GameMaster.GameRestarted += () => {
			GameObject.Destroy(obstaclesParent);
			Start();
		};
		StartCoroutine(SpawnLoop());
	}

	public void Start() {
		startTime = Time.time;
		obstaclesParent = new GameObject();
		obstaclesParent.name = "obstaclesParent";
	}

	void Update() {
		elapsedTime = Time.time - startTime + 1;

		if (GameMaster.Player == null) return;

		spawnTransform.position = GameMaster.Player.transform.position + Vector3.right * GlobalState.GetState().katamari.size + Vector3.right * 10;
	}

	IEnumerator SpawnLoop() {
		while (true) {
			currentObstaclesPerSecond = startingObstaclesPerSecond / elapsedTime + minimumObstaclesPerSecond;
			yield return new WaitForSeconds(1 / currentObstaclesPerSecond);

			var go = GetNextObstaclePrefab();
			go.transform.position = spawnTransform.position + Vector3.up * UnityEngine.Random.Range(minHeightPos, maxSpawnHeight);
			go.transform.rotation = Quaternion.identity;
			go.transform.parent = obstaclesParent.transform;
			go.transform.localScale *= obstacleSize * UnityEngine.Random.Range(0.1f, UnityEngine.Random.Range(elapsedTime / 10, elapsedTime / 5));

			go.AddComponent<Obstacle>();
			go.tag = Tags.Obstacle;

			var goRigidbody2D = go.GetComponent<Rigidbody2D>();
			goRigidbody2D.mass *= (elapsedTime / 100);
			LaunchObstacle(goRigidbody2D);
		}
	}

	void LaunchObstacle(Rigidbody2D rigidbody2D) {
		rigidbody2D.velocity = new Vector3(UnityEngine.Random.Range(startForce, startForce * 1.5f), 0, 0);
	}

	GameObject GetNextObstaclePrefab() {
		var x = UnityEngine.Random.Range(0, Enum.GetNames(typeof(ObstacleType)).Length);
		switch ((ObstacleType)Enum.Parse(typeof(ObstacleType), Enum.GetNames(typeof(ObstacleType))[x])) {
			case ObstacleType.ObstaclePrefab: return GetRandomObstaclePrefab();
			case ObstacleType.RandomSprites: return GetRandomSpriteObstacle();
			default: return GetRandomObstaclePrefab();
		}
	}

	GameObject GetRandomObstaclePrefab() {
		var randomPrefab = obstaclePrefabs[UnityEngine.Random.Range(0, obstaclePrefabs.Length)];
		return GameObject.Instantiate(randomPrefab);
	}

	GameObject GetRandomSpriteObstacle() {
		var randomSprite = spritesForRandomSpriteSpawner[UnityEngine.Random.Range(0, spritesForRandomSpriteSpawner.Length)];
		var spriteGo = new GameObject("spriteGO");
		spriteGo.AddComponent<SpriteRenderer>()
			.sprite = randomSprite;
		spriteGo.AddComponent<PolygonCollider2D>();
		spriteGo.AddComponent<Rigidbody2D>()
			.gravityScale = 0;
		return spriteGo;
	}
}
