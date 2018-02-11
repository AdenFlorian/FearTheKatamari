using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KatamariSize : MonoBehaviour {
	public Transform StuckStuffParent;

	static float size = 1f;

	public static float GetSize() {
		return size;
	}

	void Start () {
	}
	
	void Update () {
	}
	
	float GetDistance(Transform a, Transform b) {
		return (a.position - b.position).magnitude;
	}

	void OnCollisionEnter2D(Collision2D collision) {
		var obstacle = collision.gameObject.GetComponent<Obstacle>();
		if (obstacle != null) {
			OnCollisionWithObstacle(obstacle);
		}
	}

	void OnCollisionWithObstacle(Obstacle obstacle) {
		for (int i = 0; i < StuckStuffParent.childCount; i++) {
			var child = StuckStuffParent.GetChild(i);
			
			var distance = GetDistance(obstacle.transform, child.transform);

			if (distance > size) {
				size = distance;
			}
		}
	}
}
