using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleFromMicrophone : MonoBehaviour
{
    public Vector2 minSclae;
    public Vector2 maxSclae;
    public AudioLoudnessDetection detector;

    public float loudnessSensibility = 100;
    public float threshold;

    public float loudnessRecorder;

    void Update()
    {
        float loudness = detector.GetLoudnessFromMicrophone() * 100;
        loudnessRecorder = loudness;

        if (loudness < threshold)
        {
            loudness = 0;
            Debug.Log("QUIET TIME");
        }
        else if (loudness > threshold)
        {
            Debug.Log("SCREAMING DETECTED");
            Debug.Log(loudness);
        }

        transform.localScale = Vector2.Lerp(minSclae, maxSclae, loudness);
    }
}
