using UnityEngine;

public class ScreenShakeController : MonoBehaviour
{
    public static ScreenShakeController Instance;

    private float shakeTimeRemaining, shakePower, shakeFadeTime;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if (shakeTimeRemaining > 0)
        {
            shakeTimeRemaining -= Time.deltaTime;
            float xShake = Random.Range(-1f, 1f) * shakePower;
            float yShake = Random.Range(-1f, 1f) * shakePower;
            transform.localPosition = new Vector3(xShake, yShake, transform.position.z);

            shakePower = Mathf.MoveTowards(shakePower, 0f, shakeFadeTime * Time.deltaTime);
        }
        else
        {
            transform.localPosition = new Vector3(0f, 0f, transform.position.z);
        }
    }

    public void StartShake(float duration, float power)
    {
        shakeTimeRemaining = duration;
        shakePower = power;
        shakeFadeTime = power / duration;
    }
}
