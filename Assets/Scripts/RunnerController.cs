using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

public class RunnerController : MonoBehaviour
{
    [SerializeField] private float runSpeed = 4f;
    [SerializeField] private GameObject soulOrb;

    private enum MovementSideEnum
    {
        Up, Down, Side
    }
    
    private Rigidbody2D _rb;
    private PlayerControls _controls;
    private Vector2 _move;
    private Animator _animator;
    private FieldOfView.Scripts.FieldOfView _fieldOfView;

    // State
    private MovementSideEnum _movementSide = MovementSideEnum.Down;
    private bool _freezeMovementInput = false;
    
    // Calculated ID int values for Animator
    private static readonly int IdleBack = Animator.StringToHash("Idle Back");
    private static readonly int IdleFront = Animator.StringToHash("Idle Front");
    private static readonly int IdleSide = Animator.StringToHash("Idle Side");
    private static readonly int WalkingSide = Animator.StringToHash("Walking Side");
    private static readonly int WalkingFront = Animator.StringToHash("Walking Front");
    private static readonly int WalkingBack = Animator.StringToHash("Walking Back");

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
        _fieldOfView = FindObjectOfType<FieldOfView.Scripts.FieldOfView>();
        _fieldOfView.SetOrigin(transform.position);
        _fieldOfView.SetAimDirection(Vector3.down);
    }

    private void FixedUpdate()
    {
        if (_freezeMovementInput) return;
        Move();
        FlipSprite();
        _fieldOfView.SetOrigin(transform.position);
    }

    private void Move()
    {
        // In case you want to disable diagonal movement
        //
        // if (_move.x > Mathf.Epsilon || _move.x < 0)
        // {
        //     _rb.velocity = new Vector2(_move.x * runSpeed, 0f);
        // }
        // else
        // {
        //     _rb.velocity = new Vector2(0f, _move.y * runSpeed);
        // }
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
                _fieldOfView.SetAimDirection(new Vector3(Mathf.Sign(_rb.velocity.x), 0, 0));
            } else if (PlayerIsMovingUp())
            {
                _movementSide = MovementSideEnum.Up;
                _fieldOfView.SetAimDirection(Vector3.up);
            }
            else
            {
                _movementSide = MovementSideEnum.Down;
                _fieldOfView.SetAimDirection(Vector3.down);
            }

            SetIdleAnimationsToFalse();
        }
        else // Player is NOT moving and we should set IDLE state depending on MovementSide
        {
            SetIdleAnimation();
        }
        _animator.SetBool(WalkingSide, PlayerHasHorizontalSpeed());
        _animator.SetBool(WalkingFront, PlayerIsMovingDown());
        _animator.SetBool(WalkingBack, PlayerIsMovingUp());
        
        
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
        _animator.SetBool(IdleBack, false);
        _animator.SetBool(IdleFront, false);
        _animator.SetBool(IdleSide, false);
    }
    
    private void Respawn()
    {
        var soulObjects = FindObjectsOfType<SoulOrb>().Length;
        if (soulObjects != 0) return;
        StopMovementAndFreezeInput();
        var position = new Vector3(transform.localPosition.x, transform.localPosition.y + 1f, 0f);
        Instantiate(soulOrb, position, Quaternion.identity);
        FindObjectOfType<GameSession>().LoseSoul();
    }

    private void StopMovementAndFreezeInput()
    {
        _freezeMovementInput = true;
        _rb.velocity = new Vector2(0, 0);
        _animator.SetBool(WalkingSide, false);
        _animator.SetBool(WalkingFront, false);
        _animator.SetBool(WalkingBack, false);
        SetIdleAnimation();
    }

    private void SetIdleAnimation()
    {
        SetIdleAnimationsToFalse();
        switch (_movementSide)
        {
            case MovementSideEnum.Up:
                _animator.SetBool(IdleBack, true);
                break;
            case MovementSideEnum.Down:
                _animator.SetBool(IdleFront, true);
                break;
            case MovementSideEnum.Side:
                _animator.SetBool(IdleSide, true);
                break;
            default:
                throw new NotImplementedException("Unsupported movement side -> " + _movementSide);
        }
    }
    
    public void SoulRetrieved()
    {
        StopMovementAndFreezeInput();
        FindObjectOfType<GameSession>().SoulFound();
    }
    
}
