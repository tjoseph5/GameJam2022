using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] InputActionReference movementControl;
    [SerializeField] InputActionReference jumpControl;

    Vector2 movementVector;

    public float moveSpeed;
    public float slowSpeed;
    public float fastSpeed;
    public float jumpForce;

    public bool canControl = true;

    Rigidbody2D rb;

    public bool isJumping;
    bool jumpWasPressed;

    public bool facingRight = true;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
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
        if (movementVector.x > 0 && !facingRight && !isJumping)
        {
            Flip();
        }
        else if (movementVector.x < 0 && facingRight && !isJumping)
        {
            Flip();
        }
        #endregion

        if (!UniversalScreamChecker.instance.isScreaming)
        {
            moveSpeed = slowSpeed;
        }
        else
        {
            moveSpeed = fastSpeed;
        }
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
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.tag == "Ground")
        {
            isJumping = false;
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag == "Ground")
        {
            isJumping = true;
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
