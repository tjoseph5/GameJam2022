using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement instance;

    [SerializeField] InputActionReference movementControl;
    [SerializeField] InputActionReference jumpControl;

    Vector2 movementVector;

    /*[HideInInspector]*/ public float moveSpeed;
    public float slowSpeed;
    public float fastSpeed;
    public float jumpForce;

    public bool canControl = true;

    Rigidbody2D rb;

    public bool isJumping;
    bool jumpWasPressed;

    public bool facingRight = true;

    Animator anim;

    private string currentState; //used in ChangeAnimationState function. Checks which animator state is currently active

    //Static strings used specifically for referencing player animator states
    const string playerIdle = "P_Idle";
    const string playerJump = "P_Jump";
    const string playerWalk = "P_Walk";
    const string playerRun = "P_Run";
    const string playerDeath = "P_Death";
    const string playerFall2 = "P_Falling2";
    public const string playerVictory = "P_Victory";


    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        rb = GetComponent<Rigidbody2D>();
        anim = this.transform.GetChild(0).GetComponent<Animator>();
    }

    void Start()
    {
        
    }

    void Update()
    {
        #region Movement Stuff
        if (!isJumping)
        {
            if (jumpControl.action.triggered)
            {
                jumpWasPressed = true;
            }
        }

        movementVector = new Vector2(Mathf.Round(movementControl.action.ReadValue<Vector2>().x), Mathf.Round(movementControl.action.ReadValue<Vector2>().y));

        //rotate if facing the wrong way
        if (movementVector.x > 0 && !facingRight)
        {
            Flip();
        }
        else if (movementVector.x < 0 && facingRight)
        {
            Flip();
        }
        #endregion

        if (!UniversalScreamChecker.instance.isScreaming)
        {
            moveSpeed -= Time.deltaTime * 10;

            if(moveSpeed < slowSpeed)
            {
                moveSpeed = slowSpeed;
            }
        }
        else
        {
            moveSpeed += Time.deltaTime * 10;

            if (moveSpeed > fastSpeed)
            {
                moveSpeed = fastSpeed;
            }
        }

        #region Animation Stuff
        if (canControl)
        {
            if (!isJumping)
            {
                if (movementVector.x != 0)
                {
                    switch (UniversalScreamChecker.instance.isScreaming)
                    {
                        case true:
                            if (anim.speed > UniversalScreamChecker.instance.animationSpeedUp / 2)
                            {
                                ChangeAnimationState(playerRun);

                                if (moveSpeed < fastSpeed / 2)
                                {
                                    //moveSpeed = fastSpeed / 2;
                                }
                            }
                            break;
                        case false:
                            if (anim.speed < UniversalScreamChecker.instance.animationSpeedUp / 2 || anim.speed == 1)
                            {
                                ChangeAnimationState(playerWalk);

                                if (moveSpeed > slowSpeed * 2)
                                {
                                    moveSpeed = slowSpeed * 2;
                                }
                            }
                            break;
                    }
                }
                else if (movementVector.x == 0)
                {
                    ChangeAnimationState(playerIdle);
                }
            }
            else if (isJumping)
            {
                ChangeAnimationState(playerJump);
            }
        }
        #endregion
    }

    void FixedUpdate()
    {
        if (canControl)
        {
            rb.velocity = new Vector2(movementVector.x * moveSpeed, rb.velocity.y);

            if (jumpWasPressed)
            {
                jumpWasPressed = false;
                rb.velocity = new Vector2(rb.velocity.x, 0);
                rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            }
        }
    }

    void Flip() //Function that is called whenever the player change X axis for movementVector. Also resets sprinting variables
    {
        if (canControl)
        {
            facingRight = !facingRight;
            transform.Rotate(0, 180, 0);
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

    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.tag == "Ground")
        {
            isJumping = false;
        }


        if (col.tag == "Bad")
        {
            Death();
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag == "Ground")
        {
            isJumping = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Bad")
        {
            Death();
        }

        if (collision.collider.tag == "Goal")
        {
            if (!anim.GetCurrentAnimatorStateInfo(0).IsName(playerDeath))
            {
                Victory();

                if (collision.collider.GetComponent<SpoolGoal>())
                {
                    collision.collider.GetComponent<SpoolGoal>().ChangeAnimationState("Spool Collected");
                }
            }
        }
    }
    public void Death()
    {
        canControl = false;
        rb.velocity = new Vector2(0, rb.velocity.y);
        ChangeAnimationState(playerDeath);
    }
    public void Victory()
    {
        canControl = false;
        rb.bodyType = RigidbodyType2D.Static;
        if (isJumping)
        {
            ChangeAnimationState(playerFall2);
        }
        else if (!isJumping)
        {
            ChangeAnimationState(playerIdle);
        }

        GameObject.FindObjectOfType<Timer>().levelComplete = true;

        foreach(GameObject go in GameObject.FindGameObjectsWithTag("Bad"))
        {
            if (go.GetComponent<Rigidbody2D>())
            {
                go.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
                go.tag = "Untagged";
            }
        }
    }

    #region On Enable/Disable
    private void OnEnable()
    {
        movementControl.action.Enable();
        jumpControl.action.Enable();
    }
    private void OnDisable()
    {
        movementControl.action.Disable();
        jumpControl.action.Disable();
    }
    #endregion
}
