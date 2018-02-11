using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KatamariSize : MonoBehaviour {
	void Awake() {
		GlobalState.ResetKatamariSize();
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
			ChangeSizeIfNeeded(obstacle);
		}
	}

	void ChangeSizeIfNeeded(Obstacle obstacle) {
		var greatestDistance = GetGreatestDistanceToExistingObstacles(obstacle);
		if (greatestDistance > GlobalState.GetState().katamari.size) {
			GlobalState.ChangeKatamariSize(greatestDistance);
		}		
	}

	float GetGreatestDistanceToExistingObstacles(Obstacle obstacle) {
		var greatestDistance = 0f;

		for (int i = 0; i < transform.childCount; i++) {
			var child = transform.GetChild(i);
			
			var distance = GetDistance(obstacle.transform, child.transform);

			if (distance > greatestDistance) {
				greatestDistance = distance;
			}
		}

		return greatestDistance;
	}
}
