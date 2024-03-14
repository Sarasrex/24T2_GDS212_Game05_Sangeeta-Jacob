using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthWormProjectile : MonoBehaviour
{
    [SerializeField] private float speed = 7.5f;
    [HideInInspector] public int damage;
    private Vector2 direction;
    private PlayerStats playerStats;

    private void Awake()
    {
        playerStats = GameObject.FindWithTag("Player").GetComponent<PlayerController>().playerStats;
    }

    public void SetDirection(Vector2 direction)
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.velocity = direction.normalized * speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerStats.ModifyHealth(-damage);
            DamageFlashManager.Flash();
            Destroy(gameObject);
        }
        else if (collision.CompareTag("Rock"))
        {
            Destroy(gameObject);
        }
        else if (collision.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }
}
