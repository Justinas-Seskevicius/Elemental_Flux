using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CollectableSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] collectables;

    public void InstantiateCollectable()
    {
        var randomNumber = Random.Range(0, collectables.Length);
        Instantiate(collectables[randomNumber], transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
