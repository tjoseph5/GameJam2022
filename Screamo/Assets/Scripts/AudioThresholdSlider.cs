using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioThresholdSlider : MonoBehaviour
{
    [SerializeField] AudioLoudnessDetection detector;
    Slider audioThresholdAdjustor;
    Slider audioLoudnessMeter;

    float loudness;

    public float threshold;

    void Awake()
    {
        if(audioThresholdAdjustor == null)
        {
            audioThresholdAdjustor = this.transform.GetChild(1).GetComponent<Slider>();
        }

        if (audioLoudnessMeter == null)
        {
            audioLoudnessMeter = this.transform.GetChild(0).GetComponent<Slider>();
        }

        audioThresholdAdjustor.minValue = audioLoudnessMeter.minValue;
        audioThresholdAdjustor.maxValue = audioLoudnessMeter.maxValue;
    }

    void Update()
    {
        threshold = audioThresholdAdjustor.value;

        if (detector.thresholdHolder != threshold)
        {
            detector.thresholdHolder = threshold;
        }

        loudness = detector.GetLoudnessFromMicrophone() * 100;
        audioLoudnessMeter.value = loudness;
    }
}
