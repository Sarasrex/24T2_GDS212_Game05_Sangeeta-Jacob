using UnityEngine;

public class Gun : MonoBehaviour, IWeapon
{
    [SerializeField] private GameObject bulletPrefab;
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
        get => bulletPrefab;
        set => bulletPrefab = value;
    }

    public void Fire(Vector2 direction)
    {
        if (Time.time - lastFireTime >= 1f / playerStats.firingSpeed)
        {
            var bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity).GetComponent<Bullet>();
            Vector2 combinedDirection = direction + (playerRb.velocity.normalized/2);
            bullet.SetDirection(combinedDirection);
            lastFireTime = Time.time;
        }
    }
}
