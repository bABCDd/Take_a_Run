using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartButton : MonoBehaviour
{
    public GameObject startButton;

    private void Start()
    {
        Time.timeScale = 0f;
        if (startButton != null)
        {
            startButton.SetActive(true);
        }
    }

    public void OnStartButtonClicked()
    {
        Time.timeScale = 1f;
        if (startButton != null)
        {
            startButton.SetActive(false);
        }
    }


}
