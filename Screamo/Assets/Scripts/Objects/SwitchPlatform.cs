using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchPlatform : MonoBehaviour
{
    [SerializeField] bool screamToSwitchOn;
    BoxCollider2D boxCollider2D;

    public List<Animator> childBlockSprites = new List<Animator>();
    

    private string currentState; //used in ChangeAnimationState function. Checks which animator state is currently active

    //Static strings used specifically for referencing player animator states
    const string dottedBlock = "Dotted Square";
    const string solidBlock = "Solid Square";

    void Awake()
    {
        foreach(Transform child in this.transform)
        {
            if (child.GetComponent<Animator>() && !childBlockSprites.Contains(child.GetComponent<Animator>()))
            {
                childBlockSprites.Add(child.GetComponent<Animator>());
            }
        }
    }

    void Start()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        switch (screamToSwitchOn)
        {
            case true:
                switch (UniversalScreamChecker.instance.isScreaming)
                {
                    case true:
                        boxCollider2D.enabled = true;
                        break;
                        
                    case false:
                        boxCollider2D.enabled = false;
                        break;
                }
                break;

            case false:
                switch (UniversalScreamChecker.instance.isScreaming)
                {
                    case true:
                        boxCollider2D.enabled = false;
                        break;

                    case false:
                        boxCollider2D.enabled = true;
                        break;
                }
                break;
        }

        switch (boxCollider2D.enabled)
        {
            case true:
                foreach(Animator anim in childBlockSprites)
                {
                    ChangeAnimationState(solidBlock);
                    anim.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
                }
                break;

            case false:
                foreach (Animator anim in childBlockSprites)
                {
                    ChangeAnimationState(dottedBlock);
                    anim.gameObject.GetComponent<SpriteRenderer>().color = Color.red;
                }
                break;
        }
    }

    public void ChangeAnimationState(string newState) //Allows animator to change between states without needing parameters or a lot of transitions within the animator controller
    {
        //prevents the same animations from interrupting itself
        if (currentState == newState) return;

        foreach(Animator anim in childBlockSprites)
        {
            //plays the animation
            anim.Play(newState);
        }

        //updates the currentState string
        currentState = newState;
    }
}
