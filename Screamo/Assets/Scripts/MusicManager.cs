using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    AudioSource calmMusic;
    AudioSource heavyMusic;

    public static MusicManager instance;

    public bool heavyCanPlay;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;

            calmMusic = this.transform.GetChild(0).GetComponent<AudioSource>();
            heavyMusic = this.transform.GetChild(1).GetComponent<AudioSource>();

            //DontDestroyOnLoad(gameObject);
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
            calmMusic.volume -= Time.deltaTime * 2;

            if (calmMusic.volume < 0)
            {
                calmMusic.volume = 0;
            }

            if (!heavyMusic.isPlaying && heavyCanPlay)
            {
                heavyMusic.Play();
                heavyMusic.time = calmMusic.time;
            }
            heavyMusic.volume = 1;
            heavyCanPlay = true;
        }
        else
        {
            heavyMusic.volume -= Time.deltaTime * 2;

            if (heavyMusic.volume < 0)
            {
                heavyMusic.volume = 0;
            }

            calmMusic.volume = 1;
        }
    }
}
