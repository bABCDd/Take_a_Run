using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class GameOverController : MonoBehaviour
{
    public GameObject gameOverPanel;
    public TMP_Text gameOverText;
    public TMP_Text scoreText;
    public TMP_Text timeText;
    public GameObject gameOverCanvas; // 이 안에 배경 이미지 포함됨

    public void ShowGameOver(int finalScore, float playTime)
    {
        gameOverPanel.SetActive(true);
        gameOverCanvas.SetActive(true);

        gameOverText.text = "GAME OVER";
        scoreText.text = $"Score: {finalScore}";
        timeText.text = $"Time: {FormatTime(playTime)}";

        StartCoroutine(WaitAndExit());
    }

    private IEnumerator WaitAndExit()
    {
        yield return new WaitForSeconds(5f);
        // 예: 메인 메뉴로 돌아가기
        // SceneManager.LoadScene("MainMenu");

        // 또는 게임 종료
        Application.Quit();
    }

    private string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60f);
        int seconds = Mathf.FloorToInt(time % 60f);
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
