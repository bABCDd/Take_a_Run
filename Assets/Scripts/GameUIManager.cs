using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class GameUIManager : MonoBehaviour
{
    [Header("UI References")]
    public TMP_Text scoreText;
    public TMP_Text finalScoreText;
    public TMP_Text playTimeText;
    public TMP_Text lifeCountText;

    public GameObject lifeIconPrefab;
    public Transform lifePanel;
    public GameObject gameOverPanel;
    public GameObject gameOverCanvas; 
    


    private int score = 0;
    private int life = 9;
    private float playTime = 0f;

    private List<GameObject> lifeIcons = new List<GameObject>();
    private bool isGameOver = false;
    private float scoreTimer = 0f;
    private Player player;


    private void Awake()
    {
        if (GameManager.instance != null)
            GameManager.instance.uiManager = this;
    }
    public void Start()
    {
        

        UpdateScore(score);
        SetLife(life);
        gameOverPanel?.SetActive(false);
        player = FindObjectOfType<Player>();
        if (player == null)
            Debug.LogError("Player�� ã�� �� �����ϴ�!");
    }

    void Update()
    {
        if (!isGameOver)
        {
            playTime += Time.deltaTime;

            // 1�ʸ��� ���� +1
            scoreTimer += Time.deltaTime;
            if (scoreTimer >= 1f)
            {
                AddScore(1);
                scoreTimer = 0f;
            }
        }
    }

    public void AddScore(int amount)
    {
        score += amount;
        UpdateScore(score);
    }

    public void UpdateScore(int newScore)
    {
        score = newScore;
        if (scoreText != null)
            scoreText.text = $"SCORE : {score:D2}";
    }

    public void TakeDamage(int amount)
    {
        life -= amount;
        SetLife(life);

        if (life <= 0)
        {
            GameOver();
        }
    }

    public void SetLife(int newLife)
    {
        int currentIconCount = lifeIcons.Count;

        // �ʿ� ���� ������ ����
        if (newLife < currentIconCount)
        {
            int diff = currentIconCount - newLife;
            for (int i = 0; i < diff; i++)
            {
                // ������ ������ ����
                GameObject iconToRemove = lifeIcons[lifeIcons.Count - 1];
                lifeIcons.RemoveAt(lifeIcons.Count - 1);
                Destroy(iconToRemove);
            }
        }
        // �����ϸ� �߰� (ȸ�� �ý��� �� ���)
        else if (newLife > currentIconCount)
        {
            int diff = newLife - currentIconCount;
            for (int i = 0; i < diff; i++)
            {
                GameObject icon = Instantiate(lifeIconPrefab, lifePanel);
                lifeIcons.Add(icon);
            }
        }

        // �ؽ�Ʈ�� ����
        lifeCountText.text = $"�� {newLife}";
        Debug.Log($"LifeIcons count: {lifeIcons.Count}");
    }


    void GameOver()
    {
        isGameOver = true;

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }

        if (finalScoreText != null)
            finalScoreText.text = $"���� ����: {score}";

        if (playTimeText != null)
            playTimeText.text = $"�÷��� �ð�: {FormatTime(playTime)}";

        Time.timeScale = 0f;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

    }
    public void ShowGameOverUI(int finalScore, float finalPlayTime)
    {
        if (gameOverCanvas != null)
        {
            gameOverCanvas.SetActive(true);
        }

        
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    string FormatTime(float seconds)
    {
        int min = (int)(seconds / 60);
        int sec = (int)(seconds % 60);
        return $"{min:D2}:{sec:D2}";
    }
    private void OnEnable()
    {
        if (GameManager.instance != null)
            GameManager.instance.uiManager = this;
    }
    public void UpdateLifeDisplay(int newLife)
    {
        int currentIconCount = lifeIcons.Count;

        // �ʿ� ���� ������ ����
        if (newLife < currentIconCount)
        {
            int diff = currentIconCount - newLife;
            for (int i = 0; i < diff; i++)
            {
                if (lifeIcons.Count > 0)
                {
                    GameObject iconToRemove = lifeIcons[lifeIcons.Count - 1];
                    lifeIcons.RemoveAt(lifeIcons.Count - 1);
                    Destroy(iconToRemove);
                }
            }
        }
        // �����ϸ� �߰� (ü�� ȸ��)
        else if (newLife > currentIconCount)
        {
            int diff = newLife - currentIconCount;
            for (int i = 0; i < diff; i++)
            {
                GameObject icon = Instantiate(lifeIconPrefab, lifePanel);
                lifeIcons.Add(icon);
            }
        }

        // �ؽ�Ʈ ����
        lifeCountText.text = $"�� {newLife}";
        Debug.Log($"[UI] ��Ʈ ǥ�� ���ŵ�: ���� ������ �� = {lifeIcons.Count}");
    }

}