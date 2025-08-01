using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
public enum GameState { Ready, Playing, GameOver }
public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private bool isGameOver = false;
    public GameUIManager uiManager;
    public GameState gameState = GameState.Ready;


    private float playTime = 0f;
    private int score = 0;
    private int life = 9;
    public GameObject gameOverPanel;
    public Text scoreText;
    public Text timeText;

    private void Awake()
    {
        //�̱���
        if (instance == null)
        {
            instance = this;
            
        }

    }
    IEnumerator Start()
    {
        // GameUIManager�� �����Ǳ⸦ ��ٸ� (������ ������ ���)
        yield return new WaitForEndOfFrame();

        if (uiManager == null)
        {
            uiManager = FindObjectOfType<GameUIManager>();
            if (uiManager == null)
            {
                Debug.LogError("Not founded GameUIManager");
                yield break;
            }
        }

        gameState = GameState.Playing;
        uiManager.UpdateScore(score);
        uiManager.SetLife(life);
    
   
    }
    void Update()
    {
        if (gameState != GameState.Playing) return;

        playTime += Time.deltaTime;

        // 1�ʸ��� ���� ����
        if (Time.frameCount % 60 == 0) // ������ ���
        {
            AddScore(1);
        }
    }
    public void AddScore(int amount)
    {
        score += amount;
      
        if (uiManager == null)
            uiManager = FindObjectOfType<GameUIManager>();

        if (uiManager != null)
            uiManager.UpdateScore(score);
    }

    public void TakeDamage(int amount)
    {   
        life -= amount;
        if (uiManager == null)
            uiManager = FindObjectOfType<GameUIManager>();

        if (uiManager != null)
            uiManager.SetLife(life);



        if (life <= 0)
            GameOver();
    }
    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void GameOver()
    {
        if (isGameOver)
            return;

        isGameOver = true;

        Debug.Log("Game Over");
        gameState = GameState.GameOver;
        gameOverPanel.SetActive(true);
        
        //�� ��ȯ�� 5f��ŭ ������Ŵ
        Invoke("ReturnToStartScene", 5f);
    }
    public void ShowGameOverUI(int score, float playTime)
    {
        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);

        if (scoreText != null)
            scoreText.text = $"Score: {score}";

        if (timeText != null)
        {
            int minutes = Mathf.FloorToInt(playTime / 60f);
            int seconds = Mathf.FloorToInt(playTime % 60f);
            timeText.text = $"Time: {minutes:00}:{seconds:00}";
        }
    }

    void ReturnToStartScene()
    {
        SceneManager.LoadScene("StartScene");
    }
}
