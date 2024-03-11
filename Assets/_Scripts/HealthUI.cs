using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    public GameObject fullHeartPrefab;
    public GameObject halfHeartPrefab;
    private int maxHealth;
    private int currentHealth;
    public Transform heartsContainer;

    public PlayerStats playerStats;

    private void Start()
    {
        maxHealth = playerStats.health;
        currentHealth = maxHealth;
        UpdateHealthUI();
    }

    public void UpdateHealth(int health)
    {
        currentHealth = health;
        UpdateHealthUI();
    }

    void UpdateHealthUI()
    {
        // Clear existing hearts
        foreach (Transform child in heartsContainer)
        {
            Destroy(child.gameObject);
        }

        // Calculate the number of full and half hearts needed
        int fullHearts = currentHealth / 2;
        int halfHearts = currentHealth % 2;

        // Instantiate full hearts
        for (int i = 0; i < fullHearts; i++)
        {
            Instantiate(fullHeartPrefab, heartsContainer);
        }

        // Instantiate a half heart if needed
        if (halfHearts > 0)
        {
            Instantiate(halfHeartPrefab, heartsContainer);
        }
    }
}
