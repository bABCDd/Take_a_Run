using UnityEngine;


public class ResourceController : MonoBehaviour
{
    [SerializeField] private int maxLives = 9;

    [HideInInspector]
    public int currentLives;

    void Awake()
    {
        currentLives = maxLives; // 게임 시작 시 최대 목숨으로 설정
    }

    public void ChangeLives(int amount)
    {
        currentLives += amount;
        currentLives = Mathf.Clamp(currentLives, 0, maxLives); 

        Debug.Log("목숨 변화: " + amount + ", 현재 목숨: " + currentLives);


        if (currentLives <= 0)
        {
            Die(); // 모든 목숨을 잃으면 사망 처리
        }
    }

    


    private void Die()
    {
        Debug.Log("게임 오버! 모든 목숨을 잃었습니다. GameManager에게 알립니다.");

        //게임 매니저 만들면 주석 풀기
        //if (GameManager.Instance != null)
        //{
        //    GameManager.Instance.GameOver(); // GameManager의 GameOver 함수 호출
        //}
        //else
        //{
        //    Debug.LogError("GameManager 인스턴스를 찾을 수 없습니다! 게임 오버 처리가 제대로 되지 않을 수 있습니다.");
  
        //}
    }
}