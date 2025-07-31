using UnityEngine;

public class GroundManager : MonoBehaviour
{
    private LevelManager _levelManager;

    // 인스펙터에 할당할 땅 오브젝트들 (Ground_1, Ground_2)
    [SerializeField] private GameObject[] groundObjects;

    // 각 땅 이미지의 실제 너비 (유니티 유닛 기준) - 여기서 13.5f로 맞춰야 합니다.
    [SerializeField] private float groundWidth = 13.5f;

    // 땅이 화면 왼쪽 밖으로 얼마나 나가야 재배치될지 결정하는 X 좌표
    // 카메라 왼쪽 끝보다 살짝 더 왼쪽으로 설정합니다.
    [SerializeField] private float resetXPosition = -15.64f;

    // 재배치될 때 이동할 목표 X 좌표까지의 오프셋 (두 땅 조각의 총 너비)
    // groundWidth * groundObjects.Length로 계산됩니다.
    private float loopOffset;

    void Awake()
    {
        _levelManager = LevelManager.Instance;
        if (_levelManager == null)
        {
            Debug.LogError("GroundManager: LevelManager 인스턴스를 찾을 수 없습니다! 땅이 움직이지 않을 수 있습니다.");
        }

        if (groundObjects == null || groundObjects.Length == 0)
        {
            Debug.LogError("GroundManager: groundObjects 배열이 비어 있습니다! 땅 오브젝트를 할당해주세요.");
            enabled = false;
            return;
        }

        // loopOffset을 동적으로 계산: 땅 오브젝트 전체의 너비 합
        loopOffset = groundWidth * groundObjects.Length;
    }

    void Update()
    {
        float currentScrollSpeed = 0f;
        if (_levelManager != null)
        {
            currentScrollSpeed = _levelManager.GetCurrentGameSpeed();
        }

        foreach (GameObject ground in groundObjects)
        {
            // 땅 오브젝트를 왼쪽으로 이동시킵니다.
            ground.transform.Translate(Vector2.left * currentScrollSpeed * Time.deltaTime);

            // 현재 땅 오브젝트의 왼쪽 가장자리 X 좌표를 계산합니다.
            float currentLeftEdgeX = ground.transform.position.x - (groundWidth / 2f);

            // 땅이 화면 왼쪽 밖으로 완전히 나갔는지 확인합니다.
            if (currentLeftEdgeX < resetXPosition)
            {
                // 현재 위치에 loopOffset을 더하여 재배치 (자기 자신을 loopOffset만큼 이동)
                ground.transform.position += new Vector3(loopOffset, 0, 0);
            }
        }
    }
}