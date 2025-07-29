using UnityEngine;
using UnityEngine.UI; // UI�� ����Ѵٸ� �ʿ� (��: ��Ʈ �̹��� ������Ʈ)

public class ResourceController : MonoBehaviour
{
    // �� ��Ʈ(���) ���� (Inspector���� ����)
    [SerializeField] private int maxLives = 9;

    // ���� ��Ʈ(���) ����
    [HideInInspector] // Inspector���� ���� (�ڵ�θ� ����)
    public int currentLives;

    // (���� ����) ��Ʈ UI �̹��� �迭 - UI ������Ʈ�� ����
    public GameObject[] heartUIImages; // Inspector�� ��Ʈ �̹��� ������Ʈ���� ������� �Ҵ�

    void Awake()
    {
        currentLives = maxLives; // ���� ���� �� �ִ� ������� ����
        UpdateHeartUI(); // UI �ʱ�ȭ
    }

    /// <summary>
    /// �÷��̾��� ����� �����մϴ�.
    /// amount�� ����̸� ��� �߰�, �����̸� ��� ����.
    /// </summary>
    /// <param name="amount">��� ��ȭ�� (��: -1�� ��� 1�� ����)</param>
    public void ChangeLives(int amount)
    {
        currentLives += amount;
        currentLives = Mathf.Clamp(currentLives, 0, maxLives); // ����� 0���� �ִ� ��� ���̷� ����

        Debug.Log("��� ��ȭ: " + amount + ", ���� ���: " + currentLives);

        UpdateHeartUI(); // ��� ��ȭ �� UI ������Ʈ

        if (currentLives <= 0)
        {
            Die(); // ��� ����� ������ ��� ó��
        }
    }

    /// <summary>
    /// ���� ��� ������ ���� UI ��Ʈ �̹����� ������Ʈ�մϴ�.
    /// </summary>
    private void UpdateHeartUI()
    {
        if (heartUIImages == null || heartUIImages.Length == 0)
        {
            Debug.LogWarning("Heart UI Images�� �Ҵ���� �ʾҽ��ϴ�. UI ������Ʈ�� �ǳʍ�.");
            return;
        }

        for (int i = 0; i < maxLives; i++)
        {
            if (i < currentLives)
            {
                // ���� ������� ���� �ε����� ��Ʈ�� Ȱ��ȭ (���̰�)
                heartUIImages[i].SetActive(true);
            }
            else
            {
                // ���� ������� ���ų� ���� �ε����� ��Ʈ�� ��Ȱ��ȭ (�����)
                heartUIImages[i].SetActive(false);
            }
        }
    }

    /// <summary>
    /// �÷��̾ ��� ����� �Ҿ��� �� ȣ��˴ϴ�.
    /// </summary>
    private void Die()
    {
        Debug.Log("���� ����! ��� ����� �Ҿ����ϴ�.");
        // �̰��� ���� ���� UI ǥ��, ���� ����� ���� ���� �߰��մϴ�.
        Time.timeScale = 0; // �ӽ÷� ������ ���� (���� ���ӿ����� �� ������ ó�� �ʿ�)
    }
}