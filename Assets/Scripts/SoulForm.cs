using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulForm : MonoBehaviour
{
    [SerializeField] private float flyingSpeed = 4f;
    [SerializeField] private GameObject soulOrb;
    [SerializeField] private GameObject vortexEffect;
    
    
    private PlayerControls _controls;
    private Vector2 _move;
    private Rigidbody2D _rb;
    private bool _freezeMovementInput = false;
    
    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _controls = new PlayerControls();
        _controls.Player.Move.performed += ctx => _move = ctx.ReadValue<Vector2>();
        _controls.Player.Move.canceled += ctx => _move = Vector2.zero;
        _controls.Player.Respawn.performed += ctx => LoseSoul();
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

    private void LoseSoul()
    {
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
}
