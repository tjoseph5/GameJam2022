using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeakerAirFlow : MonoBehaviour
{
    public List<Rigidbody2D> WindZoneRbs = new List<Rigidbody2D>(); //A list that contains all of the rigidbodies of objects that have interacted with a windzone

    Vector3 windDirection = Vector3.up; //the direction of the wind

    public float windStrength; //strength of each windzone.

    float windHolder;

    Animator anim;

    private string currentState; //used in ChangeAnimationState function. Checks which animator state is currently active

    //Static strings used specifically for referencing player animator states
    const string boomBox = "Boom Box";
    const string soundWaves = "Sound Waves";

    private void Start()
    {
        windHolder = windStrength;
        anim = this.transform.GetComponentInParent<Animator>();
    }

    private void Update()
    {
        WindZoneRbs.RemoveAll(WindZoneRbs => WindZoneRbs == null);

        if (!UniversalScreamChecker.instance.isScreaming)
        {
            windStrength = 0;
            ChangeAnimationState(boomBox);
        }
        else
        {
            windStrength = windHolder;
            ChangeAnimationState(soundWaves);
        }
    }

    //This adds any gameObject with a rigidbody component to the WindZoneRbs list on collision
    private void OnTriggerEnter2D(Collider2D other)
    {
        Rigidbody2D objectRigid = other.gameObject.GetComponent<Rigidbody2D>();

        if (objectRigid != null)
        {
            WindZoneRbs.Add(objectRigid);
        }
    }

    //This removes any gameObject with a rigidbody component to the WindZoneRbs list out of collision
    private void OnTriggerExit2D (Collider2D other)
    {
        Rigidbody2D objectRigid = other.gameObject.GetComponent<Rigidbody2D>();

        if (objectRigid != null)
        {
            WindZoneRbs.Remove(objectRigid);
        }
    }

    //This updates will blow any object with a rigid component away in the respected windzone's Z axis
    private void FixedUpdate()
    {
        windDirection = transform.up;

        if (WindZoneRbs.Count > 0)
        {
            foreach (Rigidbody2D rigid in WindZoneRbs)
            {
                rigid.AddForce(windDirection * windStrength);
            }
        }
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
