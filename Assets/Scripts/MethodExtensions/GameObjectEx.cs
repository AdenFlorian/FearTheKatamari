using UnityEngine;

public static class GameObjectEx {
    public static GameObject Instantiate(this GameObject gameObject, GameObject original, Transform target) {
        return GameObject.Instantiate(original, target.position, target.rotation);
    }
}
