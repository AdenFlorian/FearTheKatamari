using UnityEngine;

public static class MyRandom {
    public static bool Boolean() {
        return Random.Range(0, 2) == 0;
    }
}
