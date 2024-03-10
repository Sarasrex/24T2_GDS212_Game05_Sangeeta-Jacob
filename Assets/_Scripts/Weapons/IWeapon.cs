using UnityEngine;

public interface IWeapon
{
    GameObject BulletPrefab { get; set; }
    void Fire(Vector2 direction);
}
