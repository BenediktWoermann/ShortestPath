using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Stats
{
    public static bool onScreenOnly;
    public static bool editMode;
    public static bool allPlaced;
    public static float gridSize;
    public static float speed;

    public static void SetDefault() {
        onScreenOnly = false;
        editMode = false;
        allPlaced = false;
        gridSize = 1.2f;
        speed = 1f;
    }
}
