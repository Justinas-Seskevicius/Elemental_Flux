using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

public class RunnerController : MonoBehaviour
{
    [SerializeField] private float runSpeed = 4f;
    [SerializeField] private GameObject soul;

    private enum MovementSideEnum
    {
        Up, Down, Side
    }
    
    private Rigidbody2D _rb;
    private PlayerControls _controls;
    private Vector2 _move;
    private Animator _animator;

    // State
    private MovementSideEnum _movementSide = MovementSideEnum.Down;
    
    void Awake()
    {
        _controls = new PlayerControls();
        _controls.Player.Move.performed += ctx => _move =
            ctx.ReadValue<Vector2>();
        _controls.Player.Move.canceled += ctx => _move = Vector2.zero;
        _controls.Player.Respawn.performed += ctx => Respawn();
    }
    
    private void OnEnable()
    {
        _controls.Player.Enable();
    }
    private void OnDisable()
    {
        _controls.Player.Disable();
    }
    
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        Move();
        FlipSprite();
    }

    private void Move()
    {
        var velocity = new Vector2(_move.x * runSpeed, _move.y * runSpeed);
        _rb.velocity = velocity;
        // transform.Translate(Vector2.up * (move.y * runSpeed * Time.deltaTime));
        // transform.Translate(Vector2.right * (move.x * runSpeed * Time.deltaTime));
        
        if (PlayerIsMoving())
        {
            // check which side he is moving and set MovementSide;
            if (PlayerHasHorizontalSpeed())
            {
                _movementSide = MovementSideEnum.Side;
            } else if (PlayerIsMovingUp())
            {
                _movementSide = MovementSideEnum.Up;
            }
            else
            {
                _movementSide = MovementSideEnum.Down;
            }

            SetIdleAnimationsToFalse();
        }
        else // Player is NOT moving and we should set IDLE state depending on MovementSide
        {
            SetIdleAnimationsToFalse();
            switch (_movementSide)
            {
                case MovementSideEnum.Up:
                    _animator.SetBool("Idle Back", true);
                    break;
                case MovementSideEnum.Down:
                    _animator.SetBool("Idle Front", true);
                    break;
                case MovementSideEnum.Side:
                    _animator.SetBool("Idle Side", true);
                    break;
                default:
                    throw new NotImplementedException("Unsupported movement side -> " + _movementSide);
            }
        }
        _animator.SetBool("Walking Side", PlayerHasHorizontalSpeed());
        _animator.SetBool("Walking Front", PlayerIsMovingDown());
        _animator.SetBool("Walking Back", PlayerIsMovingUp());
        
        
    }
    
    private void FlipSprite()
    {
        if (PlayerHasHorizontalSpeed())
        {
            transform.localScale = new Vector2(Mathf.Sign(_rb.velocity.x), transform.localScale.y);
        }
        
    }
    
    private bool PlayerHasHorizontalSpeed()
    {
        return Mathf.Abs(_rb.velocity.x) > Mathf.Epsilon;
    }

    private bool PlayerHasVerticalSpeed()
    {
        return Mathf.Abs(_rb.velocity.y) > Mathf.Epsilon;
    }

    private bool PlayerIsMovingUp()
    {
        return _rb.velocity.y > Mathf.Epsilon;
    }
    
    private bool PlayerIsMovingDown()
    {
        return _rb.velocity.y < 0;
    }

    private bool PlayerIsMoving()
    {
        return PlayerHasHorizontalSpeed() || PlayerHasVerticalSpeed();
    }

    private void SetIdleAnimationsToFalse()
    {
        _animator.SetBool("Idle Back", false);
        _animator.SetBool("Idle Front", false);
        _animator.SetBool("Idle Side", false);
    }
    
    private void Respawn()
    {
        var soulObjects = FindObjectsOfType<Soul>().Length;
        if (soulObjects != 0) return;
        Instantiate(soul, transform.localPosition, Quaternion.identity);
        FindObjectOfType<GameSession>().Respawn();
    }
    
}
