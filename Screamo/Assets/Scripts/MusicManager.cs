using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    AudioSource calmMusic;
    AudioSource heavyMusic;

    public static MusicManager instance;

    public bool heavyCanPlay;
    [Range(0,1)] public float volume;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;

            calmMusic = this.transform.GetChild(0).GetComponent<AudioSource>();
            heavyMusic = this.transform.GetChild(1).GetComponent<AudioSource>();

            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        heavyMusic.volume = 0;
    }

    void Update()
    {
        if (UniversalScreamChecker.instance.isScreaming)
        {
            calmMusic.volume -= Time.deltaTime * 4;

            if (calmMusic.volume < 0)
            {
                calmMusic.volume = 0;
            }

            if (!heavyMusic.isPlaying && heavyCanPlay)
            {
                heavyMusic.Play();
                heavyMusic.time = calmMusic.time;
            }
            if (heavyMusic.volume < volume)
            {
                heavyMusic.volume += Time.deltaTime * 4;
            }
            heavyCanPlay = true;
        }
        else
        {
            heavyMusic.volume -= Time.deltaTime * 4;

            if (heavyMusic.volume < 0)
            {
                heavyMusic.volume = 0;
            }

            if(calmMusic.volume < volume)
            {
                calmMusic.volume += Time.deltaTime * 4;
            }
        }
    }
}
