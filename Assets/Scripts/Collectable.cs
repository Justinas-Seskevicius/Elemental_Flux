using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    [SerializeField] private int points;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        FindObjectOfType<GameSession>().IncreaseScore(points);
        Destroy(gameObject);
    }
    
}
