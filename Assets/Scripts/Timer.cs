using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField] private int maxSecondsToLoseSoul = 15;
    [SerializeField] private int maxSecondsToRecoverSoul = 40;

    private int _secondsToRecoverSoul;

    private GameSession _gameSession;

    private void Start()
    {
        _gameSession = FindObjectOfType<GameSession>();
        _secondsToRecoverSoul = maxSecondsToLoseSoul + maxSecondsToRecoverSoul;
    }

    private void Update()
    {
        var secondsPassed = Time.timeSinceLevelLoad;
        if (maxSecondsToLoseSoul < secondsPassed && !_gameSession.IsSoulLost())
        {
            _gameSession.TookTooLongToLoseSoul();
        } else if (_secondsToRecoverSoul < secondsPassed && _gameSession.IsSoulLost())
        {
            _gameSession.TookTooLongToRecoverSoul();
        }
    }
}
