using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSession : MonoBehaviour
{
    private List<string> artifactFoundTime = new List<string>();
    private void Awake()
    {
        int gameSessionObjects = FindObjectsOfType<GameSession>().Length;
        if (gameSessionObjects > 1)
        {
            Destroy(gameObject);
        } else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    public void RecordFoundArtifact(string artifactName)
    {
        int secondsPassed = (int) Time.timeSinceLevelLoad;
        int seconds = secondsPassed % 60;
        string secondsText = seconds > 9 ? seconds.ToString() : $"0{seconds}";
        int minutes = secondsPassed / 60;
        string minutesText = minutes > 9 ? minutes.ToString() : $"0{minutes}";
        string artifactRecord = $"{artifactName} found at -> {minutesText}:{secondsText}";
        artifactFoundTime.Add(artifactRecord);
    }
}
