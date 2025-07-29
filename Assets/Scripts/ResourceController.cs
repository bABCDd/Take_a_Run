using UnityEngine;
using UnityEngine.UI; // UI를 사용한다면 필요 (예: 하트 이미지 업데이트)

public class ResourceController : MonoBehaviour
{
    // 총 하트(목숨) 개수 (Inspector에서 설정)
    [SerializeField] private int maxLives = 9;

    // 현재 하트(목숨) 개수
    [HideInInspector] // Inspector에서 숨김 (코드로만 조절)
    public int currentLives;

    // (선택 사항) 하트 UI 이미지 배열 - UI 업데이트를 위해
    public GameObject[] heartUIImages; // Inspector에 하트 이미지 오브젝트들을 순서대로 할당

    void Awake()
    {
        currentLives = maxLives; // 게임 시작 시 최대 목숨으로 설정
        UpdateHeartUI(); // UI 초기화
    }

    /// <summary>
    /// 플레이어의 목숨을 변경합니다.
    /// amount가 양수이면 목숨 추가, 음수이면 목숨 감소.
    /// </summary>
    /// <param name="amount">목숨 변화량 (예: -1은 목숨 1개 감소)</param>
    public void ChangeLives(int amount)
    {
        currentLives += amount;
        currentLives = Mathf.Clamp(currentLives, 0, maxLives); // 목숨을 0에서 최대 목숨 사이로 제한

        Debug.Log("목숨 변화: " + amount + ", 현재 목숨: " + currentLives);

        UpdateHeartUI(); // 목숨 변화 시 UI 업데이트

        if (currentLives <= 0)
        {
            Die(); // 모든 목숨을 잃으면 사망 처리
        }
    }

    /// <summary>
    /// 현재 목숨 개수에 맞춰 UI 하트 이미지를 업데이트합니다.
    /// </summary>
    private void UpdateHeartUI()
    {
        if (heartUIImages == null || heartUIImages.Length == 0)
        {
            Debug.LogWarning("Heart UI Images가 할당되지 않았습니다. UI 업데이트를 건너뜜.");
            return;
        }

        for (int i = 0; i < maxLives; i++)
        {
            if (i < currentLives)
            {
                // 현재 목숨보다 적은 인덱스의 하트는 활성화 (보이게)
                heartUIImages[i].SetActive(true);
            }
            else
            {
                // 현재 목숨보다 많거나 같은 인덱스의 하트는 비활성화 (숨기게)
                heartUIImages[i].SetActive(false);
            }
        }
    }

    /// <summary>
    /// 플레이어가 모든 목숨을 잃었을 때 호출됩니다.
    /// </summary>
    private void Die()
    {
        Debug.Log("게임 오버! 모든 목숨을 잃었습니다.");
        // 이곳에 게임 오버 UI 표시, 게임 재시작 로직 등을 추가합니다.
        Time.timeScale = 0; // 임시로 게임을 정지 (실제 게임에서는 더 정교한 처리 필요)
    }
}