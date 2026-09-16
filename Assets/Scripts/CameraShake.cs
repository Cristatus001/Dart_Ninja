using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake Instance;

    private void Awake()
    {
        // Singleton pattern to access CameraShake from anywhere
        if (Instance == null)
            Instance = this;
    }

    // Shake the camera with a given intensity and duration
    public void ShakeCamera(float intensity, float duration)
    {
        StartCoroutine(Shake(intensity, duration));
    }

    private IEnumerator Shake(float intensity, float duration)
    {
        Vector3 originalPosition = transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            float xOffset = Random.Range(-1f, 1f) * intensity;
            float yOffset = Random.Range(-1f, 1f) * intensity;

            transform.position = originalPosition + new Vector3(xOffset, yOffset, 0);

            elapsedTime += Time.deltaTime;

            yield return null;
        }

        transform.position = originalPosition; // Reset camera position
    }
}
