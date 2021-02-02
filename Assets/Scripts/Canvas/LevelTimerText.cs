using UnityEngine;
using UnityEngine.UI;

namespace Canvas
{
    public class LevelTimerText : MonoBehaviour
    {
        private Text _levelTimerText;
        private GameSession _gameSession;

        void Start()
        {
            _levelTimerText = GetComponent<Text>();
            _gameSession = FindObjectOfType<GameSession>();
        }

        private void Update()
        {
            UpdateLevelTimerText();
        }

        private void UpdateLevelTimerText()
        {
            var secondsPassed = (int) _gameSession.SecondsRemainingTillLoss;
            var seconds = secondsPassed % 60;
            // var secondsText = seconds > 9 ? seconds.ToString() : $"0{seconds}";
            // var minutes = secondsPassed / 60;
            // var minutesText = minutes > 9 ? minutes.ToString() : $"0{minutes}";
            // _levelTimerText.text = $"Time remaining: {minutesText}:{secondsText} till soul is lost!";
            _levelTimerText.text = $"{seconds} seconds remaining!";
        }
    }
}
