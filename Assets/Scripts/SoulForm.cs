using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulForm : MonoBehaviour
{
    [SerializeField] private float flyingSpeed = 4f;
    [SerializeField] private GameObject soulOrb;
    [SerializeField] private GameObject vortexEffect;
    [SerializeField] private LayerMask platformLayerMask;

    private PlayerControls _controls;
    private Vector2 _move;
    private Rigidbody2D _rb;
    private CircleCollider2D _circleCollider2D;
    private bool _freezeMovementInput = false;
    
    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _circleCollider2D = GetComponent<CircleCollider2D>();
        _controls = new PlayerControls();
        _controls.Player.Move.performed += ctx => _move = ctx.ReadValue<Vector2>();
        _controls.Player.Move.canceled += ctx => _move = Vector2.zero;
        _controls.Player.Respawn.performed += ctx => PlaceSoul();
        _controls.Player.Quit.performed += ctx => QuitGame();
    }

    // private void Start()
    // {
    //     if (FindObjectOfType<GameSession>().IsSoulLost())
    //     {
    //         Destroy(gameObject);
    //     }
    // }

    private void OnEnable()
    {
        _controls.Player.Enable();
    }
    private void OnDisable()
    {
        _controls.Player.Disable();
    }
    
    private void FixedUpdate()
    {
        if(_freezeMovementInput) return;
        Move();
    }

    private void Move()
    {
        var velocity = new Vector2(_move.x * flyingSpeed, _move.y * flyingSpeed);
        _rb.velocity = velocity;
    }

    private void PlaceSoul()
    {
        if (CollidingWithWalls()) return;

        var soulObjects = FindObjectsOfType<SoulOrb>().Length;
        if (soulObjects != 0) return;
        FreezeMovementInput();
        // var position = new Vector3(transform.localPosition.x, transform.localPosition.y + 1f, 0f);
        var position = transform.localPosition;
        Instantiate(soulOrb, position, Quaternion.identity);
        Instantiate(vortexEffect, position, Quaternion.identity);
        FindObjectOfType<GameSession>().PlaceSoul();
        Destroy(gameObject);
    }

    public void FreezeMovementInput()
    {
        _freezeMovementInput = true;
        _rb.velocity = new Vector2(0, 0);
    }

    private static void QuitGame()
    {
        Application.Quit();
    }

    private bool CollidingWithWalls()
    {
        var raycastHit2D = Physics2D.CircleCast(transform.position,(_circleCollider2D.radius * 1.15f),
            Vector2.zero, 0f, platformLayerMask);
        return raycastHit2D.collider != null;
    }

    // private void OnDrawGizmosSelected()
    // {
    //     Gizmos.color = Color.magenta;
    //     Gizmos.DrawSphere(transform.position, _circleCollider2D.radius * 1.15f);
    // }
}
