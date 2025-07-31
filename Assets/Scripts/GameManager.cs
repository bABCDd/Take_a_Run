using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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

    private void Awake()
    {
        //�̱���
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
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
        uiManager.UpdateScore(score);
        if (uiManager == null)
            uiManager = FindObjectOfType<GameUIManager>();
    }

    public void TakeDamage(int amount)
    {
        life -= amount;
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
        //�� ��ȯ�� 3f��ŭ ������Ŵ
        Invoke("ReturnToStartScene", 1f);
    }

    void ReturnToStartScene()
    {
        SceneManager.LoadScene("StartScene");
    }
}
