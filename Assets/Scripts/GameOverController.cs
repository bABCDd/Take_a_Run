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
    public GameObject gameOverCanvas; // �� �ȿ� ��� �̹��� ���Ե�

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
        // ��: ���� �޴��� ���ư���
        // SceneManager.LoadScene("MainMenu");

        // �Ǵ� ���� ����
        Application.Quit();
    }

    private string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60f);
        int seconds = Mathf.FloorToInt(time % 60f);
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
