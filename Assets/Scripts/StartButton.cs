using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartButton : MonoBehaviour
{
    public GameObject startButton;

    private void Start()
    {
        if (startButton != null)
        {
            startButton.SetActive(true);
        }
    }

    public void OnStartButtonClicked()
    {
        //��ư�� Ŭ�� �� ���� ������ ��ȯ

        UnityEngine.SceneManagement.SceneManager.LoadScene("MainScene");
    }
}
