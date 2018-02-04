using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ObstacleType
{
	Sphere,
	RandomSprites
}

public class Spawner : MonoBehaviour {
	public GameObject obstaclePrefab;
	public float minHeightPos = -1;
	public float maxHeightPos = 1;
	public float obstaclesPerSecond = 2;
	public float obstacleSize = 1;
	public bool spawning = true;
	public ObstacleType ObstacleType;
	public Sprite[] spritesForRandomSpriteSpawner;
	public float startForce;

	GameObject obstaclesParent;
	float startTime;
	float elapsedTime = 1;

	public static Spawner I;

	void Awake() {
		I = this;
	}

	// Use this for initialization
	public void Start() {
		startTime = Time.time;
		obstaclesParent = new GameObject();
		obstaclesParent.name = "obstaclesParent";
		spawning = true;
		StartCoroutine(spawn());
	}

	// Update is called once per frame
	void Update() {
		elapsedTime = Time.time - startTime + 1;
	}

	IEnumerator spawn() {
		while (spawning) {
			yield return new WaitForSeconds(1 / ((elapsedTime / 20) * obstaclesPerSecond + 1));

			var go = getNextObstaclePrefab(ObstacleType);
			go.transform.position = transform.position;
			go.transform.rotation = Quaternion.identity;
			go.transform.position += new Vector3(0, Random.Range(minHeightPos, maxHeightPos), 0);
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
