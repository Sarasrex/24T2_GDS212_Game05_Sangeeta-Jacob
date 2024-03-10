using UnityEngine;

public class Gun : MonoBehaviour, IWeapon
{
    [SerializeField] private GameObject bulletPrefab;
    private float lastFireTime = 0;
    private PlayerStats playerStats;

    private void Awake()
    {
        playerStats = GetComponent<PlayerController>().playerStats;
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
            Instantiate(bulletPrefab, transform.position, Quaternion.identity)
                .GetComponent<Bullet>().SetDirection(direction);
            lastFireTime = Time.time;
        }
    }
}
