using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelTimerText : MonoBehaviour
{
    private Text _levelTimerText;

    void Start()
    {
        _levelTimerText = GetComponent<Text>();
    }

    private void Update()
    {
        UpdateLevelTimerText();
    }

    private void UpdateLevelTimerText()
    {
        int secondsPassed = (int) Time.timeSinceLevelLoad;
        int seconds = secondsPassed % 60;
        string secondsText = seconds > 9 ? seconds.ToString() : $"0{seconds}";
        int minutes = secondsPassed / 60;
        string minutesText = minutes > 9 ? minutes.ToString() : $"0{minutes}";
        _levelTimerText.text = $"{minutesText}:{secondsText}";
    }
}
