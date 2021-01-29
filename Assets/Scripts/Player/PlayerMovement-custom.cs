using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float movSpeed;                //The movement speed when grounded
    [SerializeField] float airMovSpeed;             //The movement speed when in the air
    [SerializeField] float movAccel;                //The maximum change in velocity the player can do on the ground. This determines how responsive the character will be when on the ground.
    [SerializeField] float airMovAccel;             //The maximum change in velocity the player can do in the air. This determines how responsive the character will be in the air.

    [Header("Jump")]
    [SerializeField] float initialJumpForce;        //The force applied to the player when starting to jump
    [SerializeField] float holdJumpForce;           //The force applied to the character when holding the jump button
    [SerializeField] float maxJumpTime;             //The maximum amount of time the player can hold the jump button

    [Header("Ground detection")]
    [SerializeField] float groundCastRadius;        //Radius of the circle when doing the circle cast to check for the ground
    [SerializeField] float groundCastDist;          //Distance of the circle cast

    [Header("Misc")]
    [SerializeField] float gravityMultiplier = 2.7f;

    //Rigidbody cache
    new Rigidbody2D rigidbody;

    bool isGrounded;

    void Start()
    {
        //Setup our rigidbody cache variable
        rigidbody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        rigidbody.AddForce(gravityMultiplier * Physics2D.gravity * rigidbody.mass, ForceMode2D.Force);

        isGrounded = DoGroundCheck();
        Debug.Log("Is grounded? " + isGrounded);
        Move(new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")));

        if (Input.GetButtonDown("Jump"))
        {
            Jump();
        }
    }

    void Move(Vector2 _dir)
    {
        Vector2 velocity = rigidbody.velocity;

        //calculate the ground direction based on the ground normal
        Vector2 groundDir = Vector2.Perpendicular(DoGroundCast()).normalized;
        groundDir.x *= -1; //Vector2.Perpendicular rotates the vector 90 degrees counter clockwise, inverting X. So here we invert X back to normal

        //The velocity we want our character to have. We get the movement direction, the ground direction and the speed we want (ground speed or air speed)
        Vector2 targetVelocity = groundDir * _dir * (isGrounded ? movSpeed : airMovSpeed);

        //The change in velocity we need to perform to achieve our target velocity
        Vector2 velocityDelta = targetVelocity - velocity;

        //The maximum change in velocity we can do
        float maxDelta = isGrounded ? movAccel : airMovAccel;

        //Clamp the velocity delta to our maximum velocity change
        velocityDelta.x = Mathf.Clamp(velocityDelta.x, -maxDelta, maxDelta);

        //We don't want to move the character vertically
        velocityDelta.y = 0;

        //Apply the velocity change to the character
        rigidbody.AddForce(velocityDelta * rigidbody.mass, ForceMode2D.Impulse);
    }

    bool DoGroundCheck()
    {
        //If DoGroundCast returns Vector2.zero (it's the same as Vector2(0, 0)) it means it didn't hit the ground and therefore we are not grounded.
        return DoGroundCast() != Vector2.zero;
    }

    Vector2 DoGroundCast()
    {
        //We will use this array to get what the CircleCast returns. The size of this array determines how many results we will get.
        //Note that we have a size of 2, that's because we are always going to get the player as the first element, since the cast
        //has its origin inside the player's collider.
        RaycastHit2D[] hits = new RaycastHit2D[2];

        if (Physics2D.CircleCast(transform.position, groundCastRadius, Vector3.down, new ContactFilter2D(), hits, groundCastDist) > 1)
        {
            return hits[1].normal;
        }

        return Vector2.zero;
    }

    void Jump()
    {
        if (!isGrounded)
        {
            return;
        }

        rigidbody.AddForce(Vector3.up * initialJumpForce * rigidbody.mass, ForceMode2D.Impulse);

        StartCoroutine(JumpCoroutine());
    }

    IEnumerator JumpCoroutine()
    {
        //true if the player is holding the Jump button down
        bool wantsToJump = true;

        //Counts for how long we've been jumping
        float jumpTimeCounter = 0;

        while (wantsToJump && jumpTimeCounter <= maxJumpTime)
        {
            jumpTimeCounter += Time.deltaTime;

            //check if the player still wants to jump
            wantsToJump = Input.GetButton("Jump");

            rigidbody.AddForce(Vector3.up * holdJumpForce * rigidbody.mass * maxJumpTime / jumpTimeCounter);

            yield return null;
        }
    }
}
