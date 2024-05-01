using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.Rendering.Universal;

[System.Serializable]
public class PlayerStats : MonoBehaviour, IDataPersistence
{
    public int maxHealth;
    public int health;
    public int damage;
    public float speed;
    public float firingSpeed;
    public int pierce;

    public Volume volume;
    private UnityEngine.Rendering.Universal.ChromaticAberration chromaticAberration;
    public float chromaticAberrationEffectDuration = 0.5f;

    public PlayerStats(int maxHealth = 6, int health = 6, int damage = 10, float speed = 5f, float firingSpeed = 2f, int pierce = 1)
    {
        this.maxHealth = maxHealth;
        this.health = health;
        this.damage = damage;
        this.speed = speed;
        this.firingSpeed = firingSpeed;
        this.pierce = pierce;
    }

    private void Start()
    {
        volume.profile.TryGet(out chromaticAberration);
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

        if (amount < 0)
        {
            StartCoroutine(AnimateChromaticAberration());
        }
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

    public void LoadData(GameData data)
    {
        this.maxHealth = data.maxHealth;
        this.health = data.health;
        this.damage = data.damage;
        this.speed = data.speed;
        this.firingSpeed = data.firingSpeed;
        this.pierce = data.pierce;
    }

    public void SaveData(GameData data) 
    {
        data.maxHealth = this.maxHealth;
        data.health = this.health;
        data.damage = this.damage;
        data.speed = this.speed;
        data.firingSpeed = this.firingSpeed;
        data.pierce = this.pierce;
    }

    private IEnumerator AnimateChromaticAberration()
    {
        float time = 0f;
        while (time < chromaticAberrationEffectDuration / 2)
        {
            // Increase intensity
            chromaticAberration.intensity.value = Mathf.Lerp(0f, 1f, time / (chromaticAberrationEffectDuration / 2));
            time += Time.deltaTime;
            yield return null;
        }

        while (time < chromaticAberrationEffectDuration)
        {
            // Decrease intensity
            chromaticAberration.intensity.value = Mathf.Lerp(1f, 0f, (time - chromaticAberrationEffectDuration / 2) / (chromaticAberrationEffectDuration / 2));
            time += Time.deltaTime;
            yield return null;
        }

        // Ensure it returns to 0
        chromaticAberration.intensity.value = 0f;
    }
}
