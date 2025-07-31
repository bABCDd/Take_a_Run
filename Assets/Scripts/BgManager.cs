using UnityEngine;

public class BgManager : MonoBehaviour
{
    private LevelManager _levelManager;

    // 인스펙터에 할당할 배경 오브젝트들 (Background_1, Background_2)
    [SerializeField] private GameObject[] backgroundObjects;

    // 각 배경 이미지의 실제 너비 (유니티 유닛 기준) - 여기서 13.5f로 맞춰야 합니다.
    [SerializeField] private float backgroundWidth = 13.5f;

    // 배경이 화면 왼쪽 밖으로 얼마나 나가야 재배치될지 결정하는 X 좌표
    [SerializeField] private float resetXPosition = -15.64f;

    // 재배치될 때 이동할 목표 X 좌표까지의 오프셋 (두 배경 조각의 총 너비)
    private float loopOffset;

    void Awake()
    {
        _levelManager = LevelManager.Instance;
        if (_levelManager == null)
        {
            Debug.LogError("BgManager: LevelManager 인스턴스를 찾을 수 없습니다! 배경이 움직이지 않을 수 있습니다.");
        }

        if (backgroundObjects == null || backgroundObjects.Length == 0)
        {
            Debug.LogError("BgManager: backgroundObjects 배열이 비어 있습니다! 배경 오브젝트를 할당해주세요.");
            enabled = false;
            return;
        }

        // loopOffset을 동적으로 계산: 배경 오브젝트 전체의 너비 합
        loopOffset = backgroundWidth * backgroundObjects.Length;
    }

    void Update()
    {
        float currentScrollSpeed = 0f;
        if (_levelManager != null)
        {
            currentScrollSpeed = _levelManager.GetCurrentGameSpeed();
        }

        foreach (GameObject background in backgroundObjects)
        {
            // 배경 오브젝트를 왼쪽으로 이동시킵니다.
            background.transform.Translate(Vector2.left * currentScrollSpeed * Time.deltaTime);

            // 현재 배경 오브젝트의 왼쪽 가장자리 X 좌표를 계산합니다.
            float currentLeftEdgeX = background.transform.position.x - (backgroundWidth / 2f);

            // 배경이 화면 왼쪽 밖으로 완전히 나갔는지 확인합니다.
            if (currentLeftEdgeX < resetXPosition)
            {
                // 현재 위치에 loopOffset을 더하여 재배치
                background.transform.position += new Vector3(loopOffset, 0, 0);
            }
        }
    }
}