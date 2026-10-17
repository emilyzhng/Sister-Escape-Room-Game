
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    public Snake snake;
    public GameObject startButton;
    public GameObject endScreen;
    public GameObject scoreText;
    public GameObject grid;

    public int foodsToWin = 10;
    public GameObject popup;

    public int foodsCollected = 0;
    public bool gameStarted = false;
    public bool gameEnded = false;

    private void Start()
    {
        Time.timeScale = 1f;

        if (endScreen != null)
            endScreen.SetActive(false);

        if (startButton != null)
            startButton.SetActive(true);

        if (grid != null)
            grid.SetActive(false);

        UpdateScore();
        Time.timeScale = 0f;
    }

    public void StartGame()
    {
        if (gameStarted || gameEnded)
            return;

        gameStarted = true;

        if (startButton != null)
            startButton.SetActive(false);

        if (grid != null)
            grid.SetActive(true);

        Time.timeScale = 1f;
    }

    public void CollectFood()
    {
        if (!gameStarted || gameEnded)
            return;

        foodsCollected++;
        UpdateScore();

        if (foodsCollected >= foodsToWin)
            EndGame();
    }

    public void ResetScore()
    {
        foodsCollected = 0;
        UpdateScore();
    }

    private void UpdateScore()
    {
        if (scoreText == null)
            return;

        TMP_Text tmpText = scoreText.GetComponent<TMP_Text>();

        if (tmpText != null)
        {
            tmpText.text = "Food: " + foodsCollected + " / " + foodsToWin;
            return;
        }

        Text text = scoreText.GetComponent<Text>();

        if (text != null)
            text.text = "Food: " + foodsCollected + " / " + foodsToWin;
    }

    private void EndGame()
    {
        gameEnded = true;
        gameStarted = false;

        if (endScreen != null)
            endScreen.SetActive(true);

        Time.timeScale = 0f;
        grid.SetActive(false);

    }

    public void RestartGame()
    {
        Time.timeScale = 1f;

        foodsCollected = 0;
        gameStarted = false;
        gameEnded = false;

        if (endScreen != null)
            endScreen.SetActive(false);

        if (startButton != null)
            startButton.SetActive(true);

        if (grid != null)
            grid.SetActive(false);

        if (snake != null)

        UpdateScore();
        Time.timeScale = 0f;
    }

    public void xbutton()
    {
        RestartGame();
        if (popup != null)
        {
            popup.SetActive(false);

        }
        if (startButton != null)
        {
            startButton.SetActive(true);
        }

        if (grid != null)
        {
            grid.SetActive(false);
        }

        if (endScreen != null)
        {
            endScreen.SetActive(false);
        }
        Time.timeScale = 0f;
    }
}
