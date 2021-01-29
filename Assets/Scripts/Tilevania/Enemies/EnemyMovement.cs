using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{

    [SerializeField] float moveSpeed = 1f;
    Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(moveSpeed, 0f);
    }

    private void Update()
    {
        rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
    }

    bool IsFacingRight()
    {
        return transform.localScale.x > 0;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        transform.localScale = new Vector2(-(Mathf.Sign(transform.localScale.x)), 1f);
        moveSpeed = moveSpeed * -1;
        //rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
    }


    // ---- OLDER code using Raycast to check for walls. Needs a fix to work for checking when near edge :/
    //[SerializeField] float movementSpeed = 2f;
    //[SerializeField] LayerMask platformMask;

    //Rigidbody2D rb;
    //BoxCollider2D boxCollider;

    //void Start()
    //{
    //    rb = GetComponent<Rigidbody2D>();
    //    boxCollider = GetComponent<BoxCollider2D>();
    //}

    //void Update()
    //{
    //    Move();
    //    FlipSprite();
    //    IsTouchingWall();
    //}

    //private void FlipSprite()
    //{
    //    if (IsTouchingWall())
    //    {
    //        transform.localScale = new Vector2(-(Mathf.Sign(rb.velocity.x)), 1f);
    //    }
    //}

    //private void Move()
    //{
    //    if (IsFacingRight())
    //    {
    //        rb.velocity = new Vector2(movementSpeed, rb.velocity.y);
    //    }
    //    else
    //    {
    //        rb.velocity = new Vector2(-movementSpeed, rb.velocity.y);
    //    }
    //}

    //private bool IsFacingRight()
    //{
    //    return transform.localScale.x > 0;
    //}

    //private bool IsTouchingWall()
    //{
    //    Vector2 direction;
    //    if (IsFacingRight())
    //    {
    //        direction = Vector2.right;
    //    } else
    //    {
    //        direction = Vector2.left;
    //    }
    //    float extraHeightText = 0.1f;
    //    RaycastHit2D raycastRightHit =
    //        Physics2D.Raycast(boxCollider.bounds.center, direction, boxCollider.bounds.extents.y + extraHeightText, platformMask);
    //    // DEBUG ray cast
    //    //Color rayColor;
    //    //if (raycastRightHit.collider != null)
    //    //{
    //    //    rayColor = Color.green;
    //    //}
    //    //else
    //    //{
    //    //    rayColor = Color.red;
    //    //}
    //    //Debug.DrawRay(boxCollider.bounds.center, direction * (boxCollider.bounds.extents.y + extraHeightText), rayColor);
    //    return raycastRightHit.collider != null;
    //}

}
