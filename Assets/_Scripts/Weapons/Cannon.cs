using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour, IWeapon
{
    [SerializeField] private GameObject cannonBallPrefab;
    private float lastFireTime = 0;
    private PlayerStats playerStats;
    private Rigidbody2D playerRb;

    private void Awake()
    {
        playerStats = GetComponent<PlayerController>().playerStats;
        playerRb = GetComponent<Rigidbody2D>();
    }

    public GameObject BulletPrefab
    {
        get => cannonBallPrefab;
        set => cannonBallPrefab = value;
    }

    public void Fire(Vector2 direction)
    {
        if (Time.time - lastFireTime >= 0.75f / playerStats.firingSpeed)
        {
            var cannonBall = Instantiate(cannonBallPrefab, transform.position, Quaternion.identity).GetComponent<CannonBall>();
            Vector2 combinedDirection = direction + (playerRb.velocity.normalized / 2);
            cannonBall.SetDirection(combinedDirection);
            lastFireTime = Time.time;
        }
    }
}
