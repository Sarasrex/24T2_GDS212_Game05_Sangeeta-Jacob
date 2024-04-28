using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData
{
    public Vector2 playerPosition;

    public int maxHealth;
    public int health;
    public int damage;
    public float speed;
    public float firingSpeed;
    public int pierce;

    // Values in this constructor are used when there is no data to load
    public GameData()
    {
        this.playerPosition = Vector2.zero;

        this.maxHealth = 6;
        this.health = 6;
        this.damage = 10;
        this.speed = 5;
        this.firingSpeed = 2;
        this.pierce = 1;
    }
}
