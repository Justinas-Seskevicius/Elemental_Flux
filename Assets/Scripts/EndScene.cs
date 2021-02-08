using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndScene : MonoBehaviour
{
    [SerializeField] private Text endText;
    [SerializeField] private Text scoreText;
    [SerializeField] private Text scoreFromItems;
    [SerializeField] private Text scoreFromTime;
    [SerializeField] private Text timeText;
    // Start is called before the first frame update
    private void Start()
    {
        var gameSession = FindObjectOfType<GameSession>();
        var timeRemaining = Mathf.FloorToInt(gameSession.SecondsRemainingTillLoss);
        if (gameSession.GetPlayerWon())
        {
            endText.text = "You found your soul!";
            timeText.text = $"Time remaining: {timeRemaining} seconds";
            scoreFromItems.text = $"Points for collected items: {gameSession.GetScore()}";
            scoreFromTime.text = $"Bonus multiplier for remaining time: X {gameSession.GetScoreForRemainingTime()}";
        }
        else
        {
            endText.text = "Your soul is lost forever in the maze...";
            // timeText.text = $"Time passed until soul lost forever: {timePassed}";
            timeText.text = "";
        }

        scoreText.text = $"Final score: {gameSession.CalculateScore()} points";
    }

    // private static string FormatTime(float secondsPassed)
    // {
    //     // var seconds = (int) secondsPassed % 60;
    //     // var secondsText = seconds > 9 ? seconds.ToString() : $"0{seconds}";
    //     // var minutes = seconds / 60;
    //     // var minutesText = minutes > 9 ? minutes.ToString() : $"0{minutes}";
    //     // return $"{minutesText}:{secondsText}";
    // }
}
