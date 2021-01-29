using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Artifact : MonoBehaviour
{
    [SerializeField] private string artifactName;
    private void OnTriggerEnter2D(Collider2D other)
    {
        FindObjectOfType<GameSession>().RecordFoundArtifact(artifactName);
        Destroy(gameObject);
    }
}
