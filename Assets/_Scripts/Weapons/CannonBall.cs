using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonBall : MonoBehaviour
{
    public float speed = 10f;
    [SerializeField] private float range = 10f;
    private Vector2 startPosition;
    private bool isDestructionStarted = false;
    private PlayerStats playerStats;
    private Rigidbody2D rb;

    private void Awake()
    {
        playerStats = GameObject.FindWithTag("Player").GetComponent<PlayerController>().playerStats;
    }

    public void SetDirection(Vector2 direction)
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = direction.normalized * speed;
        startPosition = transform.position;
    }

    void Update()
    {
        if (Vector2.Distance(startPosition, transform.position) >= range && !isDestructionStarted)
        {
            isDestructionStarted = true;
            DestructionSequence();
        }
    }

    void DestructionSequence()
    {
        rb.velocity = Vector2.zero;
        gameObject.transform.localScale = Vector3.one;
        Animator animator = GetComponent<Animator>();
        if (animator)
        {
            animator.SetTrigger("Destroy");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            GameObject enemy = collision.gameObject;
            if (enemy.TryGetComponent<BatController>(out BatController batController))
            {
                batController.health -= playerStats.damage;
            }
            else if (enemy.TryGetComponent<BlobController>(out BlobController blobController))
            {
                blobController.health -= playerStats.damage;
            }
            else if (enemy.TryGetComponent<EarthWormController>(out EarthWormController earthWormController))
            {
                earthWormController.health -= playerStats.damage;
            }
        }
        else if (collision.CompareTag("Wall"))
        {
            DestructionSequence();
        }
    }
}
