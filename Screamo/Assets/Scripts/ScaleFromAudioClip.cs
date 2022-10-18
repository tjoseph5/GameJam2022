using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleFromAudioClip : MonoBehaviour
{
    public AudioSource audioSource;
    public Vector2 minSclae;
    public Vector2 maxSclae;
    public AudioLoudnessDetection detector;

    public float loudnessSensibility = 100;
    public float threshold = 0.1f;

    void Update()
    {
        float loudness = detector.GetLoudnessFromAudioClip(audioSource.timeSamples, audioSource.clip) * loudnessSensibility;

        if (loudness < threshold)
        {
            loudness = 0;
        }

        transform.localScale = Vector2.Lerp(minSclae, maxSclae, loudness);
    }
}
