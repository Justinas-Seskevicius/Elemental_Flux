using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulOrb : MonoBehaviour
{
    private void Awake()
    {
        int soulObjects = FindObjectsOfType<SoulOrb>().Length;
        if (soulObjects > 1)
        {
            Destroy(gameObject);
        } else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        FindObjectOfType<RunnerController>().SoulRetrieved();
        Destroy(gameObject);
    }
}
