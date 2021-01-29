using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    // Config
    [SerializeField] float runSpeed = 5f;
    [SerializeField] float jumpSpeed = 5f;
    [SerializeField] float climbSpeed = 1.5f;
    [SerializeField] LayerMask platformLayerMask;
    [SerializeField] LayerMask ladderLayerMask;
    [SerializeField] LayerMask enemyLayerMask;
    [SerializeField] LayerMask hazzardsLayerMask;
    [SerializeField] Vector2 deathKick = new Vector2(10f, 8f);

    // State
    bool isAlive = true;


    PlayerControls controls;
    Vector2 move;
    Rigidbody2D rb;
    Animator animator;
    Collider2D capsuleCollider;
    float originalGravity;
    float originalAngularDrag;

    void Awake()
    {
        controls = new PlayerControls();
        controls.Player.Move.performed += ctx => move =
        ctx.ReadValue<Vector2>();
        controls.Player.Move.canceled += ctx => move = Vector2.zero;
        controls.Player.Jump.performed += ctx => Jump();
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        capsuleCollider = GetComponent<Collider2D>();
        originalGravity = rb.gravityScale;
        originalAngularDrag = rb.angularDrag;
    }

    private void OnEnable()
    {
        controls.Player.Enable();
    }
    private void OnDisable()
    {
        controls.Player.Disable();
    }

    void FixedUpdate()
    {
        if (!isAlive) { return; }
        Run();
        FlipSprite();
        ClimbLadder();
        PlayerDeath();
    }

    private void Run()
    {
        Vector2 velocity = new Vector2(move.x * runSpeed, rb.velocity.y);
        rb.velocity = velocity;

        animator.SetBool("Running", playerHasHorizontalSpeed());
    }

    private void FlipSprite()
    {
        if (playerHasHorizontalSpeed())
        {
            transform.localScale = new Vector2(Mathf.Sign(rb.velocity.x), 1f);
        }
        
    }
    private bool playerHasHorizontalSpeed()
    {
        return Mathf.Abs(rb.velocity.x) > Mathf.Epsilon;
    }

    private void Jump()
    {
        if (IsGrounded())
        {
            Vector2 jumpVelocityToAdd = new Vector2(0f, jumpSpeed);
            rb.velocity += jumpVelocityToAdd;
        }   
    }

    private bool IsGrounded()
    {
        float extraHeightText = 0.04f;
        RaycastHit2D raycastHit =
            Physics2D.CapsuleCast(capsuleCollider.bounds.center, capsuleCollider.bounds.size, CapsuleDirection2D.Vertical, 0, Vector2.down, extraHeightText, platformLayerMask);
        //Color rayColor;
        //if (raycastHit.collider != null)
        //{
        //    rayColor = Color.green;
        //} else
        //{
        //    rayColor = Color.red;
        //}
        //Debug.DrawRay(collider.bounds.center + new Vector3(collider.bounds.extents.x, 0), Vector2.down * (collider.bounds.extents.y + extraHeightText), rayColor);
        //Debug.DrawRay(collider.bounds.center - new Vector3(collider.bounds.extents.x, 0), Vector2.down * (collider.bounds.extents.y + extraHeightText), rayColor);
        //Debug.DrawRay(collider.bounds.center - new Vector3(collider.bounds.extents.x, collider.bounds.extents.y), Vector2.right * (collider.bounds.extents.y + extraHeightText), rayColor);
        //Debug.Log(raycastHit.collider);
        return raycastHit.collider != null;
    }

    private void ClimbLadder()
    {
        bool playerPressingUpOrDown = Mathf.Abs(move.y) > Mathf.Epsilon;
        bool isClimbing = IsTouchingLadder() && playerPressingUpOrDown;

        if (IsOnLadder())
        {
            rb.gravityScale = 0f;
            rb.angularDrag = 0f;
            rb.velocity = new Vector2(rb.velocity.x, 0f);
        } else
        {
            rb.gravityScale = originalGravity;
            rb.angularDrag = originalAngularDrag;
        }

        if (isClimbing)
        {
            Vector2 velocity = new Vector2(rb.velocity.x, move.y * climbSpeed);
            rb.velocity = velocity;

        }

        animator.SetBool("Climbing", IsOnLadder());
    }

    private bool IsTouchingLadder()
    {
        return capsuleCollider.IsTouchingLayers(ladderLayerMask);
    }

    private bool IsOnLadder()
    {
        return IsTouchingLadder() && !IsGrounded();
    }

    private bool IsTouchingEnemy()
    {
        return capsuleCollider.IsTouchingLayers(enemyLayerMask);
    }
    private bool IsTouchingHazzards()
    {
        return capsuleCollider.IsTouchingLayers(hazzardsLayerMask);
    }

    private void PlayerDeath()
    {
        if (IsTouchingEnemy() || IsTouchingHazzards())
        {
            isAlive = false;
            animator.SetTrigger("Dying");
            rb.velocity = deathKick;
            // FindObjectOfType<GameSession>().ProcessPlayerDeath();
        }
    }

}
