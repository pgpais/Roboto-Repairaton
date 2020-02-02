using UnityEngine;
using System.Collections;

/// <summary>
/// Camera Controller that controls operations on the camera, such as shaking it.
/// </summary>
public class CameraController : MonoBehaviour
{
    private new Camera camera;
    private Vector3 originalPos;

    /// <summary>
    /// Start is called just before any of the Update methods is called the first time.
    /// </summary>
    private void Start()
    {
        camera = GetComponent<Camera>();
        originalPos = transform.position;
    }

    /// <summary>
    /// Shakes the camera with the given parameters.
    /// </summary>
    public void ShakeCamera(float duration, float magnitude)
    {
        StartCoroutine(IEShakeCamera(duration, magnitude));
    }

    /// <summary>
    /// Shakes the camera for a given duration and maginute.
    /// </summary>
    /// <returns></returns>
    IEnumerator IEShakeCamera(float duration, float magnitude)
    {
        float timeElapsed = 0;

        // Shakes the camera for the duration provided.
        while (timeElapsed < duration)
        {
            transform.position = originalPos;

            Vector2 shakeDiff;
            shakeDiff.x = Random.Range(-1, 1) * magnitude;
            shakeDiff.y = Random.Range(-1, 1) * magnitude;

            transform.position = transform.position + (Vector3)shakeDiff;
            timeElapsed += Time.unscaledDeltaTime;
            yield return null;
        }

        transform.position = originalPos;
    }
}
