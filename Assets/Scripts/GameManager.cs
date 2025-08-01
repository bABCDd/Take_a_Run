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
        //싱글톤
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
    IEnumerator Start()
    {
        // GameUIManager가 생성되기를 기다림 (프레임 끝까지 대기)
        yield return new WaitForEndOfFrame();

        if (uiManager == null)
        {
            uiManager = FindObjectOfType<GameUIManager>();
            if (uiManager == null)
            {
                Debug.LogError("GameUIManager가 씬에 없습니다!");
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

        // 1초마다 점수 증가
        if (Time.frameCount % 60 == 0) // 간단한 방법
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
        //씬 전환을 3f만큼 지연시킴
        Invoke("ReturnToStartScene", 1f);
    }

    void ReturnToStartScene()
    {
        SceneManager.LoadScene("StartScene");
    }
}
