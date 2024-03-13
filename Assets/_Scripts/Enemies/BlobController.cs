using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlobController : MonoBehaviour
{
    public int health;
    [SerializeField] private float moveSpeed = 4f;
    private Rigidbody2D rb;
    private Transform playerTransform;
    private Vector2 movement;

    [SerializeField] private int damage = 1;
    [SerializeField] private float damageCooldown;
    private float damageCooldownTimer = 0f;
    private bool isOnCooldown = false;

    public bool isKnockedBack;
    private float knockedBackTime = 0.5f;
    private float knockedBackTimer = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
    }

    void Update()
    {
        if (playerTransform != null)
        {
            Vector3 direction = playerTransform.position - transform.position;
            movement = direction.normalized;

            if (movement.x > 0)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
            else if (movement.x < 0)
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
        }

        if (isOnCooldown)
        {
            damageCooldownTimer += Time.deltaTime;
            if (damageCooldownTimer >= damageCooldown)
            {
                isOnCooldown = false;
                damageCooldownTimer = 0f;
            }
        }

        if (health <= 0)
        {
            Destroy(gameObject);
        }

        if (isKnockedBack)
        {
            knockedBackTimer += Time.deltaTime;
            if (knockedBackTimer >= knockedBackTime)
            {
                isKnockedBack = false;
                knockedBackTimer = 0f;
            }
        }
    }

    void FixedUpdate()
    {
        if (!isKnockedBack)
        {
            rb.velocity = movement * moveSpeed;
        }
    }

    private void OnTriggerEnter2D(UnityEngine.Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isOnCooldown)
        {
            GameObject player = collision.gameObject;
            player.GetComponent<PlayerController>().playerStats.ModifyHealth(-damage);
            isOnCooldown = true;
        }
    }

    private void OnTriggerStay2D(UnityEngine.Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isOnCooldown)
        {
            GameObject player = collision.gameObject;
            player.GetComponent<PlayerController>().playerStats.ModifyHealth(-damage);
            isOnCooldown = true;
        }
    }
}
