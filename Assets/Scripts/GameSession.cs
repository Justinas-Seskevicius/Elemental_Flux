﻿using System;
using System.Collections;
using System.Collections.Generic;
using Canvas;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSession : MonoBehaviour
{
    [SerializeField] private GameObject soulFormCameras;
    [SerializeField] private GameObject soulForm;
    [SerializeField] private GameObject runnerCameras;
    [SerializeField] private GameObject runner;
    
    [SerializeField] private float levelTransitionWait = 2f;
    
    // State
    [SerializeField] private int score = 0;
    [SerializeField] private float respawnInitiatedAtSeconds = 0f;
    private bool _soulLost = false;

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

        if (_soulLost)
        {
            // soulForm.SetActive(false);
            // soulFormCameras.SetActive(false);
            // runnerCameras.SetActive(true);
            // runner.SetActive(true);
            // GameObject.Find("SoulForm Cameras").SetActive(false);
            // GameObject.Find("Runner Cameras").SetActive(true);
            // GameObject.Find("Runner").SetActive(true);
        }
    }

    // public void RecordFoundArtifact(string artifactName)
    // {
    //     int secondsPassed = (int) Time.timeSinceLevelLoad;
    //     int seconds = secondsPassed % 60;
    //     string secondsText = seconds > 9 ? seconds.ToString() : $"0{seconds}";
    //     int minutes = secondsPassed / 60;
    //     string minutesText = minutes > 9 ? minutes.ToString() : $"0{minutes}";
    //     string artifactRecord = $"{artifactName} found at -> {minutesText}:{secondsText}";
    //     _artifactFoundTime.Add(artifactRecord);
    // }

    public void LoseSoul()
    {
        respawnInitiatedAtSeconds = Time.timeSinceLevelLoad;
        StartCoroutine(SoulLossProcedure());

    }

    private IEnumerator SoulLossProcedure()
    {
        yield return new WaitForSecondsRealtime(levelTransitionWait);
        _soulLost = true;
        // soulForm.SetActive(false);
        soulFormCameras.SetActive(false);
        runnerCameras.SetActive(true);
        runner.SetActive(true);
        foreach (var collectable in FindObjectsOfType<Collectable>())
        {
            collectable.EnableRenderer(false);
        }
        // Scene scene = SceneManager.GetActiveScene();
        // SceneManager.LoadScene(scene.name);
    }

    public void IncreaseScore(int points)
    {
        score += points;
        FindObjectOfType<ScoreText>().UpdateScoreText(score);
    }

    public void SoulFound()
    {
        Debug.Log($"Respawn initiated after {respawnInitiatedAtSeconds} seconds");
        Debug.Log($"Soul retrieved after {Time.timeSinceLevelLoad} seconds");
    }

    public bool IsSoulLost()
    {
        return _soulLost;
    }
    
}
