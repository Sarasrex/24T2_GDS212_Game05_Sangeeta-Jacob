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

    [SerializeField] private GameObject cannonBallAudioPrefab;

    // Shockwave
    [SerializeField] private float shockwaveRadius = 3f;
    [SerializeField] private int shockwaveDamage = 15;
    [SerializeField] private float shockwaveForce = 50f;


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
        Instantiate(cannonBallAudioPrefab);
        rb.velocity = Vector2.zero;
        gameObject.transform.localScale = Vector3.one;
        ScreenShakeController.Instance.StartShake(0.25f, 0.15f);

        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, shockwaveRadius);
        foreach (var hit in hits)
        {
            if (hit.CompareTag("Enemy"))
            {
                // Apply damage
                if (hit.gameObject.TryGetComponent<BlobController>(out BlobController blobController))
                {
                    if (!blobController.isKnockedBack)
                    {
                        blobController.health -= shockwaveDamage;
                    }
                    blobController.isKnockedBack = true;
                }
                else if (hit.gameObject.TryGetComponent<EarthWormController>(out EarthWormController earthWormController))
                {
                    earthWormController.health -= shockwaveDamage;
                    earthWormController.isKnockedBack = true;
                }

                // Apply force
                Rigidbody2D hitRb = hit.GetComponent<Rigidbody2D>();
                if (hitRb != null)
                {
                    Vector2 forceDirection = hit.transform.position - transform.position;
                    hitRb.AddForce(forceDirection.normalized * shockwaveForce);
                }
            }

            else if (hit.CompareTag("Rock"))
            {
                Destroy(hit.gameObject);
            }
        }

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
                batController.health -= playerStats.damage / 2;
            }
            else if (enemy.TryGetComponent<BlobController>(out BlobController blobController))
            {
                blobController.health -= playerStats.damage / 2;
            }
            else if (enemy.TryGetComponent<EarthWormController>(out EarthWormController earthWormController))
            {
                earthWormController.health -= playerStats.damage / 2;
            }
        }
        else if (collision.CompareTag("Wall"))
        {
            DestructionSequence();
        }
    }
}
