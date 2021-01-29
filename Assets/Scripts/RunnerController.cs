using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

public class RunnerController : MonoBehaviour
{
    [SerializeField] private float runSpeed = 4f;
    
    private Rigidbody2D rb;
    private PlayerControls controls;
    private Vector2 move;

    void Awake()
    {
        controls = new PlayerControls();
        controls.Player.Move.performed += ctx => move =
            ctx.ReadValue<Vector2>();
        controls.Player.Move.canceled += ctx => move = Vector2.zero;
    }
    
    private void OnEnable()
    {
        controls.Player.Enable();
    }
    private void OnDisable()
    {
        controls.Player.Disable();
    }
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        Vector2 velocity = new Vector2(move.x * runSpeed, move.y * runSpeed);
        rb.velocity = velocity;
        // transform.Translate(Vector2.up * (move.y * runSpeed * Time.deltaTime));
        // transform.Translate(Vector2.right * (move.x * runSpeed * Time.deltaTime));
    }
    
}
