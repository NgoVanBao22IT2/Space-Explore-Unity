using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverManager : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text finalScoreText;
    [SerializeField] private TMP_Text highScoreText;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button quitButton;

    void Start()
    {
        // Lấy score từ PlayerPrefs
        int finalScore = PlayerPrefs.GetInt("FinalScore", 0);
        int highScore = PlayerPrefs.GetInt("HighScore", 0);

        // Hiển thị score
        if (scoreText != null)
        {
            scoreText.text = "Your Score: " + finalScore.ToString();
        }

        if (finalScoreText != null)
        {
            finalScoreText.text = finalScore.ToString();
        }

        if (highScoreText != null)
        {
            highScoreText.text = "High Score: " + highScore.ToString();
        }

        // Setup buttons
        if (restartButton != null)
        {
            restartButton.onClick.AddListener(RestartGame);
        }

        if (mainMenuButton != null)
        {
            mainMenuButton.onClick.AddListener(GoToMainMenu);
        }

        if (quitButton != null)
        {
            quitButton.onClick.AddListener(QuitGame);
        }

        // Đảm bảo time scale về bình thường
        Time.timeScale = 1f;
    }

    public void RestartGame()
    {
        // Load lại scene game chính (thay "Game" bằng tên scene thực tế)
        SceneManager.LoadScene("Level1"); // Hoặc tên scene game của bạn
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}