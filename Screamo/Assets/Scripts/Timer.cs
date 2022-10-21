using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    public static Timer instance;

    public float setTimer;

    public int displayTimer;

    [HideInInspector] public bool levelComplete;

    TextMeshProUGUI displayTimerUI;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        displayTimerUI = GameObject.Find("Timer UI").GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if(setTimer > 0 && !levelComplete)
        {
            setTimer -= Time.deltaTime;
        }


        if(setTimer < 0)
        {
            setTimer = 0;
        }

        if(setTimer == 0)
        {
            PlayerMovement.instance.Death();
        }

        displayTimer = ((int)setTimer);

        displayTimerUI.text = displayTimer.ToString();
    }
}
