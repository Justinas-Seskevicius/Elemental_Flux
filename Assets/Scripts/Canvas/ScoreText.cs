using UnityEngine;
using UnityEngine.UI;

namespace Canvas
{
    public class ScoreText : MonoBehaviour
    {
        private Text _scoreText;
    
        private void Start()
        {
            _scoreText = GetComponent<Text>();
        }

        public void UpdateScoreText(int score)
        {
            _scoreText.text = $"Score: {score}";
        }
    }
}
