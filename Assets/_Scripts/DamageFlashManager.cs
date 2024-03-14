using UnityEngine;
using System.Collections;

public class DamageFlashManager : MonoBehaviour
{
    public static DamageFlashManager Instance { get; private set; }

    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Color flashColor = Color.red;
    [SerializeField] private float flashDuration = 0.1f;
    [SerializeField] private int numberOfFlashes = 2;

    private Color originalColor;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }

        if (spriteRenderer == null) spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
    }

    public static void Flash()
    {
        if (Instance != null)
        {
            Instance.StartCoroutine(Instance.FlashCoroutine());
        }
    }

    private IEnumerator FlashCoroutine()
    {
        for (int i = 0; i < numberOfFlashes * 2; i++)
        {
            spriteRenderer.color = (spriteRenderer.color == originalColor) ? flashColor : originalColor;
            yield return new WaitForSeconds(flashDuration);
        }
        spriteRenderer.color = originalColor;
    }
}
