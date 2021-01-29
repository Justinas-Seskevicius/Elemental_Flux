using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] AudioClip coinPickUpSfx;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        AudioSource.PlayClipAtPoint(coinPickUpSfx, Camera.main.transform.position);
        // FindObjectOfType<GameSession>().IncreaseCoinCounter();
        Destroy(gameObject);
    }
}
