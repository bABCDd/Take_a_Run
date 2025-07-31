using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private bool isGameOver = false;

    private void Awake()
    {
        //ΩÃ±€≈Ê
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

    public void GameOver()
    {
        if (isGameOver)
            return;

        isGameOver = true;

        Debug.Log("Game Over");
        //æ¿ ¿¸»Ø¿ª 3f∏∏≈≠ ¡ˆø¨Ω√≈¥
        Invoke("ReturnToStartScene", 3f);
    }

    void ReturnToStartScene()
    {
        SceneManager.LoadScene("StartScene");
    }
}
