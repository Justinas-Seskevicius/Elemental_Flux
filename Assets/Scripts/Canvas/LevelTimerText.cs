using UnityEngine;
using UnityEngine.UI;

namespace Canvas
{
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
            var secondsPassed = (int) Time.timeSinceLevelLoad;
            var seconds = secondsPassed % 60;
            var secondsText = seconds > 9 ? seconds.ToString() : $"0{seconds}";
            var minutes = secondsPassed / 60;
            var minutesText = minutes > 9 ? minutes.ToString() : $"0{minutes}";
            _levelTimerText.text = $"{minutesText}:{secondsText}";
        }
    }
}
