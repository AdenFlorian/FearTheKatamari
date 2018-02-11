using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KatamariSize : MonoBehaviour {

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		Debug.Log("size: " + calculateSize());
	}

	float calculateSize() {
		var greatestDistance = 0f;
		for (int i = 0; i < transform.childCount; i++) {
			var child = transform.GetChild(i);
			if (getDistanceToOrigin(child) > greatestDistance) {
				greatestDistance = getDistanceToOrigin(child);
			}
		}
		return greatestDistance;
	}
	
	float getDistanceToOrigin(Transform outerTransform) {
		return (outerTransform.position - transform.position).magnitude;
	}
}
