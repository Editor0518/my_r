using System.Collections;
using UnityEngine;

public class ScreenShaker : MonoBehaviour
{
    public float shakeAmount = 0.4f;
    public float shakeDuration = 0.08f;
    public int shakeCount = 2;

    private Vector3 originalPosition;

    private void Awake()
    {
        originalPosition = transform.localPosition;
    }

    public void Shake()
    {
        StopAllCoroutines();
        StartCoroutine(ShakeRoutine());
    }

    private IEnumerator ShakeRoutine()
    {
        for (int i = 0; i < shakeCount; i++)
        {
            transform.localPosition = originalPosition + (Vector3)Random.insideUnitCircle * shakeAmount;
            yield return new WaitForSeconds(shakeDuration);
            transform.localPosition = originalPosition;
            yield return new WaitForSeconds(shakeDuration);
        }
    }
}