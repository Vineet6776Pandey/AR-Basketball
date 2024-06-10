using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public Text scoreText;
    public Text timerText;
    public GameObject hoopManagerGameObject;
    public Canvas mainCanvas;
    public GameObject plusOnePrefab;
    public GameObject gameOverPanel;
    public Text gameOverText;
    public Text gameOverScoreText;
    public Text gameOverThrowsText;

    private int score = 0;
    private int throws = 0;
    private float timeRemaining = 60f;
    private bool isGameOver = false;
    private HoopManager hoopManager;

    void Start()
    {
        UpdateScoreText();
        StartCoroutine(Timer());
        gameOverPanel.SetActive(false);
        hoopManager = hoopManagerGameObject.GetComponent<HoopManager>();
        Invoke("ChangeHoopPosition", 20f); // Change hoop position after 20 seconds
    }

    void Update()
    {
        if (isGameOver)
            return;

        timerText.text = "Time: " + Mathf.Ceil(timeRemaining).ToString();
    }

    IEnumerator Timer()
    {
        while (timeRemaining > 0)
        {
            yield return new WaitForSeconds(1f);
            timeRemaining--;

            if (timeRemaining <= 0)
            {
                isGameOver = true;
                EndGame();
            }
        }
    }

    public void IncrementScore()
    {
        if (isGameOver)
            return;

        score++;
        UpdateScoreText();
        ShowPlusOne();
        StartNewThrow();
    }

    void UpdateScoreText()
    {
        scoreText.text = "Score: " + score.ToString();
    }

    void ShowPlusOne()
    {
        GameObject plusOneInstance = Instantiate(plusOnePrefab, mainCanvas.transform);
        RectTransform plusOneRect = plusOneInstance.GetComponent<RectTransform>();
        if (plusOneRect == null)
        {
            Debug.LogError("plusOnePrefab does not have a RectTransform component.");
            return;
        }
        RectTransform canvasRect = mainCanvas.GetComponent<RectTransform>();
        Vector2 canvasCenter = new Vector2(-150, 400);
        plusOneRect.anchoredPosition = canvasCenter;
        plusOneInstance.SetActive(true);
        Destroy(plusOneInstance, 1f);
    }

    void EndGame()
    {
        gameOverText.text = "Game Over";
        gameOverScoreText.text = "Score: " + score.ToString();
        gameOverThrowsText.text = "Throws: " + throws.ToString();
        gameOverPanel.SetActive(true);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene(1);
    }

    public void StartNewThrow()
    {
        throws++;
    }

    void ChangeHoopPosition()
    {
        hoopManager.DisappearAndReappear(); // Trigger hoop position change
    }
}