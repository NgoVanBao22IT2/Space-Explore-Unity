using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public float worldSpeed;

    [SerializeField] private int score = 0;
    public int Score { get { return score; } }

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    void Start()
    {
        // Khởi tạo score UI
        if (UIController.Instance != null)
        {
            UIController.Instance.UpdateScore(score);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P) || Input.GetButtonDown("Fire3"))
        {
            Pause();
        }
    }

    public void Pause()
    {
        if (UIController.Instance.pausePanel.activeSelf == false)
        {
            UIController.Instance.pausePanel.SetActive(true);
            Time.timeScale = 0f;
        }
        else
        {
            UIController.Instance.pausePanel.SetActive(false);
            Time.timeScale = 1f;
            PlayerController.Instance.ExitBoost();

        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void GameOver()
    {
        // Lưu score hiện tại và high score
        Debug.Log("Game Over! Saving Final Score: " + score);
        PlayerPrefs.SetInt("FinalScore", score);
        SaveHighScore();
        StartCoroutine(ShowGameOverScreen());
    }

    IEnumerator ShowGameOverScreen()
    {
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene("GameOver");

    }

    public void AddScore(int points)
    {
        score += points;
        if (UIController.Instance != null)
        {
            UIController.Instance.UpdateScore(score);
        }
    }

    // Method để lưu high score nếu cần
    public void SaveHighScore()
    {
        int currentHighScore = PlayerPrefs.GetInt("HighScore", 0);
        if (score > currentHighScore)
        {
            PlayerPrefs.SetInt("HighScore", score);
            Debug.Log("New High Score: " + score);
        }
        else
        {
            Debug.Log("Current High Score remains: " + currentHighScore);
        }
        PlayerPrefs.Save();
    }
}
