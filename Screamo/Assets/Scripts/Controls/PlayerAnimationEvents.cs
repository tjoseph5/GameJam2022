using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationEvents : MonoBehaviour
{
    void SceneRestart()
    {
        AudioLoudnessDetection.instance.RestartScene();
    }
    public void TransitiontoNextScene()
    {
        AudioLoudnessDetection.instance.NextScene();
    }
}
