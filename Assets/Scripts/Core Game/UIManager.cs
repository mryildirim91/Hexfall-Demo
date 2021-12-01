using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Mryildirim.CoreGame
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance { get; private set; }
        
        private int _score, _highScore;
        [SerializeField] private Text _scoreText, _highscoreText;

        private void Awake()
        {
            if(Instance == null)
                Instance = this;
        }

        private void Start()
        {
            _scoreText.text = "Score: " + _score;
            _highScore = PlayerPrefs.GetInt("HighScore");
            _highscoreText.text = "High Score: " + _highScore;
            
        }
        
        public void UpdateScore(int score)
        {
            _score += score;
            
            if(_scoreText)
                _scoreText.text = "Score: " + _score;
            
            if (_score > _highScore)
            {
                PlayerPrefs.SetInt("HighScore", _score);
                _highScore = _highScore = PlayerPrefs.GetInt("HighScore");
                
                if(_highscoreText)
                    _highscoreText.text = "High Score: " + _highScore;
            }
        }

        public void RestartGame()
        {
            SceneManager.LoadSceneAsync(0);
        }
    }
}
