using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    Animator anim;

    public InputActionReference enter;
    public InputActionReference exit;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    void Start()
    {
        
    }


    void Update()
    {
        if (enter.action.triggered)
        {
            anim.SetTrigger("next");
        }

        if (exit.action.triggered)
        {
            Application.Quit();
        }
    }

    private void OnEnable()
    {
        enter.action.Enable();
        exit.action.Enable();
    }

    private void OnDisable()
    {
        enter.action.Disable();
        exit.action.Disable();
    }

    void NextScene()
    {
        AudioLoudnessDetection.instance.NextScene();
    }
}
