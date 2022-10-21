using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missiles : MonoBehaviour
{
    [SerializeField] float missileSpeed;
    Rigidbody2D mRb;

    private void Awake()
    {
        mRb = GetComponent<Rigidbody2D>();
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
        Destroy(this.gameObject);
    }
}
