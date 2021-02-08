using System;
using System.Collections;
using System.Collections.Generic;
using Boo.Lang.Runtime;
using Canvas;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSession : MonoBehaviour
{
    [SerializeField] private GameObject soulFormCameras;
    [SerializeField] private GameObject soulForm;
    [SerializeField] private GameObject runnerCameras;
    [SerializeField] private GameObject runner;
    [SerializeField] private CollectableSpawner[] collectableSpawners;

    [SerializeField] private float levelTransitionWait = 2f;
    [SerializeField][Range(0, 45)] private int numberOfCollectablesToSpawn;
    [SerializeField] private float maxSecondsUntilSoulIsLost = 45f;
    [SerializeField][Range(0f, 1f)] private float timeMultiplier = 0.15f;

    // State
    [SerializeField] private int score = 0;
    // [SerializeField] private float respawnInitiatedAtSeconds = 0f;
    // [SerializeField] private float soulRecoveredAtSeconds;
    private bool _playerWon = false;
    private RunnerController _runnerController;
    private SoulForm _soulForm;
    private bool _soulPlaced = false;
    private bool _stopTimer = false;
    public float SecondsRemainingTillLoss { get; private set; }

    private void Awake()
    {
        SecondsRemainingTillLoss = maxSecondsUntilSoulIsLost;
        var gameSessionObjects = FindObjectsOfType<GameSession>().Length;
        if (gameSessionObjects > 1)
        {
            Destroy(gameObject);
        } else
        {
            DontDestroyOnLoad(gameObject);
        }
        // if (_soulLost)
        // {
            // soulForm.SetActive(false);
            // soulFormCameras.SetActive(false);
            // runnerCameras.SetActive(true);
            // runner.SetActive(true);
            // GameObject.Find("SoulForm Cameras").SetActive(false);
            // GameObject.Find("Runner Cameras").SetActive(true);
            // GameObject.Find("Runner").SetActive(true);
        // }

        if (numberOfCollectablesToSpawn > collectableSpawners.Length)
        {
            throw new RuntimeException($"NumberOfCollectablesToSpawn [{numberOfCollectablesToSpawn}] is greater than spawner array length[{collectableSpawners.Length}]");
        }
    }

    private void Start()
    {
        _soulForm = FindObjectOfType<SoulForm>();
        Shuffle(collectableSpawners);
        for (var i = 0; i < numberOfCollectablesToSpawn; i++)
        {
            collectableSpawners[i].InstantiateCollectable();
        }
    }

    private void Update()
    {
        if (!_stopTimer && TimeRanOut())
        {
            PlayerTookTooLong();
        }
    }

    private bool TimeRanOut()
    {
        if (!(SecondsRemainingTillLoss > 0)) return true;
        SecondsRemainingTillLoss -= Time.deltaTime;
        return false;
    }
    
    public void PlaceSoul()
    {
        // _stopTimer = true;
        _soulPlaced = true;
        StartCoroutine(SoulPlacedProcedure());
    }

    private IEnumerator SoulPlacedProcedure()
    {
        _stopTimer = true;
        yield return new WaitForSecondsRealtime(levelTransitionWait);
        soulFormCameras.SetActive(false);
        runnerCameras.SetActive(true);
        runner.SetActive(true);
        _runnerController = runner.GetComponent<RunnerController>();
        foreach (var collectable in FindObjectsOfType<Collectable>())
        {
            collectable.EnableRenderer(false);
        }
        _stopTimer = false;
    }

    public void IncreaseScore(int points)
    {
        score += points;
        FindObjectOfType<ScoreText>().UpdateScoreText(score);
    }

    public void SoulFound()
    {
        _playerWon = true;
        // soulRecoveredAtSeconds = Time.timeSinceLevelLoad;
        StartCoroutine(TransitionToEndScene());
    }

    private IEnumerator TransitionToEndScene()
    {
        _stopTimer = true;
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("Scenes/End");
    }

    public int GetScore()
    {
        return score;
    }

    public int GetScoreForRemainingTime()
    {
        return Mathf.FloorToInt(1 + SecondsRemainingTillLoss * timeMultiplier);
    }
    
    public int CalculateScore()
    {
        return score * GetScoreForRemainingTime();
    }

    public bool GetPlayerWon()
    {
        return _playerWon;
    }

    private void PlayerTookTooLong()
    {
        if (!_soulPlaced)
        {
            _soulForm.FreezeMovementInput();
        }
        else
        {
            _runnerController.FreezeMovementInput();
        }
        _playerWon = false;
        StartCoroutine(TransitionToEndScene());
    }

    private static void Shuffle<T>(IList<T> ts) {
        var count = ts.Count;
        var last = count - 1;
        for (var i = 0; i < last; ++i) {
            var r = UnityEngine.Random.Range(i, count);
            var tmp = ts[i];
            ts[i] = ts[r];
            ts[r] = tmp;
        }
    }
}
