using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameUIManager : MonoBehaviour
{
    [Header("UI References")]
    public TMP_Text scoreText;
    public TMP_Text finalScoreText;
    public TMP_Text playTimeText;

    public GameObject lifeIconPrefab;
    public Transform lifePanel;
    public GameObject gameOverPanel;

    private int score = 0;
    private int life = 9;
    private float playTime = 0f;

    private List<GameObject> lifeIcons = new List<GameObject>();
    private bool isGameOver = false;
    private float scoreTimer = 0f; // ���� Ÿ�̸�
    void Start()
    {
        UpdateScoreUI();
        SetLife(life);
        gameOverPanel.SetActive(false);
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
        UpdateScoreUI();
    }

    void UpdateScoreUI()
    {
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

    void SetLife(int newLife)
    {
        // ���� ������ ����
        foreach (var icon in lifeIcons)
            Destroy(icon);
        lifeIcons.Clear();

        // �� ������ ����
        for (int i = 0; i < newLife; i++)
        {
            GameObject icon = Instantiate(lifeIconPrefab, lifePanel);
            lifeIcons.Add(icon);
        }
    }

    void GameOver()
    {
        isGameOver = true;

        // ���â ����
        gameOverPanel.SetActive(true);
        finalScoreText.text = $"���� ����: {score}";
        playTimeText.text = $"�÷��� �ð�: {FormatTime(playTime)}";

        Time.timeScale = 0f; // ���� ����
    }

    public void RestartGame()
    {
        Time.timeScale = 1f; // ����� �ð� �ٽ� ����
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    string FormatTime(float seconds)
    {
        int min = (int)(seconds / 60);
        int sec = (int)(seconds % 60);
        return $"{min:D2}:{sec:D2}";
    }
}
