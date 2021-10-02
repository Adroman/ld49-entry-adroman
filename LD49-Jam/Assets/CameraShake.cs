using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public float Duration;
    public float Magnitude;
    public AnimationCurve MagnitudeTimeline;
    
    public void StartShaking()
    {
        StartCoroutine(Shake());
    }

    private IEnumerator Shake()
    {
        var originalPosition = transform.position;

        float timeElapsed = 0;

        while (timeElapsed < Duration)
        {
            float currentMagnitude = Magnitude * MagnitudeTimeline.Evaluate(timeElapsed / Duration);
            
            float x = Random.Range(-1f, 1f) * currentMagnitude;
            float y = Random.Range(-1f, 1f) * currentMagnitude;

            transform.localPosition = new Vector3(x, y, originalPosition.z);
            timeElapsed += Time.deltaTime;

            yield return null;
        }

        transform.localPosition = originalPosition;
    }
}
