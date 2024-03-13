using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EarthWormController : MonoBehaviour
{
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    [SerializeField] private GameObject projectilePrefab;
    private Vector3 playerPosition;
    private List<Collider2D> colliders = new List<Collider2D>();
    private Rigidbody2D rb;

    public int health;
    public int damage;
    [SerializeField] private float attackCooldown = 5f;
    private float nextAttackTime = 0f;

    public bool isKnockedBack;
    private float knockedBackTime = 0.5f;
    private float knockedBackTimer = 0f;

    private RoomGenerator roomGenerator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        roomGenerator = GameObject.FindWithTag("GameController").GetComponent<RoomGenerator>();
        rb = GetComponent<Rigidbody2D>();

        foreach (Collider2D collider in GetComponents<Collider2D>())
        {
            colliders.Add(collider);
        }
    }

    private void Start()
    {
        playerPosition = GameObject.FindGameObjectWithTag("Player").transform.position;
    }

    private void Update()
    {
        if (playerPosition != null)
        {
            Vector3 direction = playerPosition - transform.position;
            direction = direction.normalized;

            if (direction.x < 0)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
            else if (direction.x > 0)
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
        }

        if (Time.time >= nextAttackTime)
        {
            StartCoroutine(AttackSequence());
            nextAttackTime = Time.time + attackCooldown;
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
                rb.velocity = Vector3.Lerp(rb.velocity, Vector3.zero, 0.25f);
            }
        }
    }

    private IEnumerator AttackSequence()
    {
        foreach (Collider2D collider in colliders)
        {
            collider.enabled = true;
        }

        Vector2 spawnPosition = CalculateRandomPosition();
        transform.position = spawnPosition;
        
        animator.SetTrigger("Emerge");
        spriteRenderer.enabled = true;

        // Wait for the emerge animation to finish
        yield return new WaitForSeconds(1.1f);

        FireProjectile();

        animator.SetTrigger("Retreat");

        yield return new WaitForSeconds(1);
        spriteRenderer.enabled = false;
        
        foreach (Collider2D collider in colliders)
        {
            collider.enabled = false;
        }

        rb.velocity = Vector3.zero;
    }

    private void FireProjectile()
    {
        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        playerPosition = GameObject.FindGameObjectWithTag("Player").transform.position;
        Vector2 direction = (playerPosition - transform.position).normalized;
        projectile.GetComponent<EarthWormProjectile>().SetDirection(direction);
        projectile.GetComponent<EarthWormProjectile>().damage = damage; ;
    }

    private Vector2 CalculateRandomPosition()
    {
        float minX = float.MaxValue, maxX = float.MinValue, minY = float.MaxValue, maxY = float.MinValue;
        foreach (Transform doorSpawnPoint in roomGenerator.availableDoorSpawnPoints)
        {
            Vector3 position = doorSpawnPoint.position;
            minX = Mathf.Min(minX, position.x);
            maxX = Mathf.Max(maxX, position.x);
            minY = Mathf.Min(minY, position.y);
            maxY = Mathf.Max(maxY, position.y);
        }

        float padding = 3f;
        minX += padding;
        maxX -= padding;
        minY += padding;
        maxY -= padding;

        Vector2 spawnPosition = Vector2.zero;
        spawnPosition = new Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY));

        return spawnPosition;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Rock"))
        {
            Destroy(collision.gameObject);
        }
    }
}
