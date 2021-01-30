using System;
using System.Collections;
using System.Collections.Generic;
using Canvas;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSession : MonoBehaviour
{
    [SerializeField] private float levelTransitionWait = 2f;
    
    // State
    private List<string> _artifactFoundTime = new List<string>();
    [SerializeField] private int score = 0;
    [SerializeField] private float respawnInitiatedAtSeconds = 0f;

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
        _artifactFoundTime.Add(artifactRecord);
    }

    public void Respawn()
    {
        respawnInitiatedAtSeconds = Time.timeSinceLevelLoad;
        StartCoroutine(RespawnProcedure());

    }

    private IEnumerator RespawnProcedure()
    {
        yield return new WaitForSecondsRealtime(levelTransitionWait);
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    public void IncreaseScore(int points)
    {
        score += points;
        FindObjectOfType<ScoreText>().UpdateScoreText(score);
    }

    public void SoulRetrieved()
    {
        Debug.Log($"Respawn initiated after {respawnInitiatedAtSeconds} seconds");
        Debug.Log($"Soul retrieved after {Time.timeSinceLevelLoad} seconds");
    }
    
}
