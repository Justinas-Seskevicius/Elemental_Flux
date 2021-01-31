using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    [SerializeField] private int points;

    private Renderer _renderer;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        FindObjectOfType<GameSession>().IncreaseScore(points);
        Destroy(gameObject);
    }

    private void Awake()
    {
        // var isSoulLost = FindObjectOfType<GameSession>().IsSoulLost();
        _renderer = GetComponent<Renderer>();
        // if (!isSoulLost) return;
        // _renderer.enabled = false;
    }

    public void EnableRenderer(bool enable)
    {
        _renderer.enabled = enable;
    }
}
