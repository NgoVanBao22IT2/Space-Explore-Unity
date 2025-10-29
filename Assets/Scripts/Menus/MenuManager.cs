using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text highScoreText;

    void Start()
    {
        Time.timeScale = 1f;

        // Nếu đang ở GameOver scene, hiển thị score
        if (SceneManager.GetActiveScene().name == "GameOver")
        {
            DisplayScore();
        }
    }

    void DisplayScore()
    {
        // Lấy score từ PlayerPrefs
        int finalScore = PlayerPrefs.GetInt("FinalScore", 0);
        int highScore = PlayerPrefs.GetInt("HighScore", 0);

        Debug.Log("Final Score: " + finalScore + ", High Score: " + highScore);

        // Hiển thị score hiện tại
        if (scoreText != null)
        {
            scoreText.text = "FINAL SCORE: " + finalScore.ToString();
        }
        else
        {
            Debug.LogWarning("scoreText is not assigned!");
        }

        // Hiển thị high score với label
        if (highScoreText != null)
        {
            highScoreText.text = "HIGH SCORE: " + highScore.ToString();
        }
        else
        {
            Debug.LogWarning("highScoreText is not assigned!");
        }
    }

    public void NewGame()
    {
        SceneManager.LoadScene("Level1");
    }

    public void QuitGame()
    {
        
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
