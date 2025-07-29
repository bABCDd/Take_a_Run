using UnityEngine;


public class ResourceController : MonoBehaviour
{
    [SerializeField] private int maxLives = 9;

    [HideInInspector]
    public int currentLives;

    void Awake()
    {
        currentLives = maxLives; // ���� ���� �� �ִ� ������� ����
    }

    public void ChangeLives(int amount)
    {
        currentLives += amount;
        currentLives = Mathf.Clamp(currentLives, 0, maxLives); 

        Debug.Log("��� ��ȭ: " + amount + ", ���� ���: " + currentLives);


        if (currentLives <= 0)
        {
            Die(); // ��� ����� ������ ��� ó��
        }
    }

    


    private void Die()
    {
        Debug.Log("���� ����! ��� ����� �Ҿ����ϴ�. GameManager���� �˸��ϴ�.");

        //���� �Ŵ��� ����� �ּ� Ǯ��
        //if (GameManager.Instance != null)
        //{
        //    GameManager.Instance.GameOver(); // GameManager�� GameOver �Լ� ȣ��
        //}
        //else
        //{
        //    Debug.LogError("GameManager �ν��Ͻ��� ã�� �� �����ϴ�! ���� ���� ó���� ����� ���� ���� �� �ֽ��ϴ�.");
  
        //}
    }
}