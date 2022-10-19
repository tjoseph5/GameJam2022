using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AudioThresholdSlider : MonoBehaviour
{
    public static AudioThresholdSlider instance;

    //[SerializeField] AudioLoudnessDetection detector;
    Slider audioThresholdAdjustor;
    Slider audioLoudnessMeter;

    [HideInInspector] public float loudness;

    [HideInInspector] public float threshold;

    public TextMeshProUGUI recordingDeviceName;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        if (audioThresholdAdjustor == null)
        {
            audioThresholdAdjustor = this.transform.GetChild(1).GetComponent<Slider>();
        }

        if (audioLoudnessMeter == null)
        {
            audioLoudnessMeter = this.transform.GetChild(0).GetComponent<Slider>();
        }

        if(recordingDeviceName == null)
        {
            recordingDeviceName = this.transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        }

        audioThresholdAdjustor.minValue = audioLoudnessMeter.minValue;
        audioThresholdAdjustor.maxValue = audioLoudnessMeter.maxValue;
    }

    void Start()
    {
        threshold = AudioLoudnessDetection.instance.thresholdHolder;

        if (Microphone.devices[0] != null)
        {
            recordingDeviceName.text = "Recording Device: " + AudioLoudnessDetection.instance.publicMicrophoneName;
        }
        else if (Microphone.devices[0] == null)
        {
            recordingDeviceName.text = "Unable to find microphone!";
        }
    }

    void Update()
    {
        threshold = audioThresholdAdjustor.value;

        if (AudioLoudnessDetection.instance.thresholdHolder != threshold)
        {
            AudioLoudnessDetection.instance.thresholdHolder = threshold;
        }

        loudness = AudioLoudnessDetection.instance.GetLoudnessFromMicrophone() * 100;
        audioLoudnessMeter.value = loudness;
    }
}
