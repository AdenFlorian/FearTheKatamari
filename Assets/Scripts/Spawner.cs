using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ObstacleType {
	Sphere,
	RandomSprites
}

public class Spawner : MonoBehaviour {
	public static Spawner I;

	public GameObject obstaclePrefab;
	public Transform spawnTransform;
	public float minHeightPos = -1;
	float maxSpawnHeight = 1;
	public float obstaclesPerSecond = 2;
	public float obstacleSize = 1;
	public bool spawning = true;
	public ObstacleType ObstacleType;
	public Sprite[] spritesForRandomSpriteSpawner;
	public float startForce;

	GameObject obstaclesParent;
	float startTime;
	float elapsedTime = 1;
	public float startingObstaclesPerSecond = 10;
	public float minimumObstaclesPerSecond = 1;
	float currentObstaclesPerSecond;

	void Awake() {
		I = this;
		GlobalState.StateChanged += (newState) => {
			maxSpawnHeight = GlobalState.GetState().katamari.size * 2;
		};
	}

	public void Start() {
		startTime = Time.time;
		obstaclesParent = new GameObject();
		obstaclesParent.name = "obstaclesParent";
		spawning = true;
		StartCoroutine(spawn());
	}

	void Update() {
		elapsedTime = Time.time - startTime + 1;

		if (GameMaster.Player == null) return;

		spawnTransform.position = GameMaster.Player.transform.position + Vector3.right * GlobalState.GetState().katamari.size + Vector3.right * 10;
	}

	IEnumerator spawn() {
		while (spawning) {
			currentObstaclesPerSecond = startingObstaclesPerSecond / elapsedTime + minimumObstaclesPerSecond;
			yield return new WaitForSeconds(1 / currentObstaclesPerSecond);

			var go = getNextObstaclePrefab(ObstacleType);
			go.transform.position = spawnTransform.position + Vector3.up * Random.Range(minHeightPos, maxSpawnHeight);
			go.transform.rotation = Quaternion.identity;
			go.transform.parent = obstaclesParent.transform;
			go.transform.localScale *= obstacleSize * Random.Range(0.1f, Random.Range(elapsedTime / 10, elapsedTime / 5));

			go.AddComponent<Obstacle>();

			var goRigidbody2D = go.GetComponent<Rigidbody2D>();
			goRigidbody2D.mass *= (elapsedTime / 100);
			launchObstacle(goRigidbody2D);

			if (spawning == false) {
				GameObject.Destroy(obstaclesParent);
				break;
			}
		}
	}

	void launchObstacle(Rigidbody2D rigidbody2D) {
		rigidbody2D.velocity = new Vector3(Random.Range(startForce, startForce * 1.5f), 0, 0);
	}

	GameObject getNextObstaclePrefab(ObstacleType ObstacleType) {
		switch (ObstacleType)
		{
			case ObstacleType.Sphere: return getSphereObstacle();
			case ObstacleType.RandomSprites: return getRandomSpriteObstacle();
			default: return obstaclePrefab;
		}
	}

	GameObject getSphereObstacle() {
		return GameObject.Instantiate(obstaclePrefab);
	}

	GameObject getRandomSpriteObstacle() {
		var randomSprite = spritesForRandomSpriteSpawner[Random.Range(0, spritesForRandomSpriteSpawner.Length)];
		var spriteGo = new GameObject("spriteGO");
		spriteGo.AddComponent<SpriteRenderer>()
			.sprite = randomSprite;
		spriteGo.AddComponent<PolygonCollider2D>();
		spriteGo.AddComponent<Rigidbody2D>()
			.gravityScale = 0;
		return spriteGo;
	}
}
