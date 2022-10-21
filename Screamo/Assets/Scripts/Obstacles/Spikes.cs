using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour
{
    Rigidbody2D rb;
    public BoxCollider2D boxCollider2D;
    public float distance;

    bool isFalling = false;

    [SerializeField] float fallSpeed;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        boxCollider2D = GetComponent<BoxCollider2D>();
    }

    void Start()
    {
        rb.gravityScale = 0;
    }

    void Update()
    {
        Physics2D.queriesStartInColliders = false;
        if(isFalling == false)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, distance);

            Debug.DrawRay(transform.position, Vector2.down * distance, Color.red);

            if(hit.transform != null)
            {
                if(hit.transform.tag == "Player")
                {
                    rb.gravityScale = fallSpeed;
                    isFalling = true;
                }
            }
        }

        switch (UniversalScreamChecker.instance.isScreaming)
        {
            case false:
                rb.bodyType = RigidbodyType2D.Dynamic;
                break;
            case true:
                rb.bodyType = RigidbodyType2D.Static;
                break;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(this.gameObject);
    }
}
