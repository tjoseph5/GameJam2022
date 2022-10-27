using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missiles : MonoBehaviour
{
    [SerializeField] float missileSpeed;
    Rigidbody2D mRb;

    Animator anim;

    private string currentState; //used in ChangeAnimationState function. Checks which animator state is currently active

    //Static strings used specifically for referencing player animator states
    const string missile = "Missiles";
    const string explosion = "Explosion";

    private void Awake()
    {
        mRb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        var dir = PlayerMovement.instance.transform.position;
        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        this.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        if (PlayerMovement.instance.canControl)
        {
            mRb.MovePosition(Vector2.MoveTowards(this.transform.position, PlayerMovement.instance.gameObject.transform.position, missileSpeed * Time.deltaTime));
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.tag != "Player")
        {
            this.tag = "Untagged";
            ChangeAnimationState(explosion);
            mRb.bodyType = RigidbodyType2D.Static;
            gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
        }
        else if(collision.collider.tag == "Player")
        {
            ChangeAnimationState(explosion);
            mRb.bodyType = RigidbodyType2D.Static;
            gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag != "Player")
        {
            this.tag = "Untagged";
            ChangeAnimationState(explosion);
            mRb.bodyType = RigidbodyType2D.Static;
            gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
        }
        else if(collision.tag == "Player")
        {
            ChangeAnimationState(explosion);
            mRb.bodyType = RigidbodyType2D.Static;
            gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
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

    void MissilesDestroyed()
    {
        gameObject.SetActive(false);
    }
}
