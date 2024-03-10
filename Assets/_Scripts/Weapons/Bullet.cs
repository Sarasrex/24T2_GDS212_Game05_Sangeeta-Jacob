using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;
    private PlayerStats playerStats;

    private void Awake()
    {
        playerStats = GameObject.FindWithTag("Player").GetComponent<PlayerController>().playerStats;
    }

    public void SetDirection(Vector2 direction)
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.velocity = direction.normalized * speed;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle + 180));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {        
        if (collision.CompareTag("Enemy"))
        {
            GameObject enemy = collision.gameObject;
            if (enemy.TryGetComponent<BatController>(out BatController controller))
            {
                controller.health -= playerStats.damage;
            }
            Destroy(gameObject);
        }
        else if (collision.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }
}
