using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UniversalScreamChecker : MonoBehaviour
{
    public static UniversalScreamChecker instance;

    public bool isScreaming;

    public List<Animator> animators = new List<Animator>();

    //int sceneID;

    public float animationSpeedUp;

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
    }
    void Start()
    {
        //sceneID = SceneManager.GetActiveScene().buildIndex;
        //animators.Clear();
    }

    void Update()
    {
        #region Screaming Bool Initializer
        if (AudioThresholdSlider.instance.loudness >= AudioThresholdSlider.instance.threshold)
        {
            isScreaming = true;
        }
        else
        {
            isScreaming = false;
        }
        #endregion

        foreach(Animator animator in GameObject.FindObjectsOfType<Animator>())
        {
            if (!animators.Contains(animator))
            {
                animators.Add(animator);
            }
        }

        /*if(sceneID != SceneManager.GetActiveScene().buildIndex)
        {
            animators.Clear();
            sceneID = SceneManager.GetActiveScene().buildIndex;
        }*/

        if (isScreaming)
        {
            foreach(Animator animator in animators)
            {
                animator.speed += Time.deltaTime * 4;

                if(animator.speed > animationSpeedUp)
                {
                    animator.speed = animationSpeedUp;
                }
            }
        }
        else
        {
            foreach (Animator animator in animators)
            {
                animator.speed -= Time.deltaTime * 4;

                if(animator.speed < 1)
                {
                    animator.speed = 1;
                }
            }
        }
    }
}
