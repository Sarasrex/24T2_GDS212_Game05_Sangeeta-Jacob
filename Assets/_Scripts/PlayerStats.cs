using UnityEngine;

[System.Serializable]
public class PlayerStats
{
    public int maxHealth;
    public int health;
    public int damage;
    public float speed;
    public float firingSpeed;
    public int pierce;

    public PlayerStats(int maxHealth = 6, int health = 6, int damage = 10, float speed = 5f, float firingSpeed = 2f, int pierce = 1)
    {
        this.maxHealth = maxHealth;
        this.health = health;
        this.damage = damage;
        this.speed = speed;
        this.firingSpeed = firingSpeed;
        this.pierce = pierce;
    }

    public void ModifyMaxHealth(int amount)
    {
        maxHealth += amount;
        maxHealth = Mathf.Max(maxHealth, 1);
    }

    public void ModifyHealth(int amount)
    {
        health += amount;
        health = Mathf.Max(health, 0);
    }

    public void ModifyDamage(int amount)
    {
        damage += amount;
    }

    public void ModifySpeed(float amount)
    {
        speed += amount;
    }

    public void ModifyFiringSpeed(float amount)
    {
        firingSpeed += amount;
        firingSpeed = Mathf.Max(firingSpeed, 0.1f);
    }

    public void ModifyPierce(int amount)
    {
        pierce += amount;
    }
}
