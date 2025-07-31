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
        //버튼을 클릭 시 메인 씬으로 전환

        UnityEngine.SceneManagement.SceneManager.LoadScene("MainScene");
    }
}
