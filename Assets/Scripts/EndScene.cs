using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndScene : MonoBehaviour
{
    [SerializeField] private Text endText;
    [SerializeField] private Text scoreText;
    [SerializeField] private Text timeText;
    // Start is called before the first frame update
    private void Start()
    {
        GameSession gameSession = FindObjectOfType<GameSession>();
        var timePassed = FormatTime(gameSession.GetSoulRecoveredTime());
        if (gameSession.GetPlayerWon())
        {
            endText.text = "You found your soul!";
            timeText.text = $"Time passed until soul found: {timePassed}";
        }
        else
        {
            endText.text = "Your soul is lost forever in the maze...";
            timeText.text = $"Time passed until soul lost forever: {timePassed}";
        }

        scoreText.text = $"Your score: {gameSession.GetScore()} points";
    }

    private static string FormatTime(float secondsPassed)
    {
        var seconds = (int) secondsPassed % 60;
        var secondsText = seconds > 9 ? seconds.ToString() : $"0{seconds}";
        var minutes = seconds / 60;
        var minutesText = minutes > 9 ? minutes.ToString() : $"0{minutes}";
        return $"{minutesText}:{secondsText}";
    }
}
