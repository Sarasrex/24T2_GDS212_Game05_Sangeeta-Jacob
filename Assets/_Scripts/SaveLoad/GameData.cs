using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData
{
    public Vector2 playerPosition;

    // Values in this constructor are used when there is no data to load
    public GameData()
    {
        playerPosition = Vector2.zero;
    }
}
