using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpoolGoal : MonoBehaviour
{
    Animator anim;

    private string currentState; //used in ChangeAnimationState function. Checks which animator state is currently active

    //Static strings used specifically for referencing player animator states
    const string boomBox = "Boom Box";

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    void TriggerPlayerVictoryAnimation()
    {
        PlayerMovement.instance.ChangeAnimationState("P_Victory");
    }

    public void ChangeAnimationState(string newState) //Allows animator to change between states without needing parameters or a lot of transitions within the animator controller
    {
        //prevents the same animations from interrupting itself
        if (currentState == newState) return;

        //plays the animation
        anim.Play(newState);

        //updates the currentState string
        currentState = newState;
    }
}
