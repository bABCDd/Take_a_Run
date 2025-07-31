using UnityEngine;

public class BgManager : MonoBehaviour
{
    private LevelManager _levelManager;

    [SerializeField] private GameObject[] backgroundObjects;

    [SerializeField] private float backgroundWidth = 13.5f;

    [SerializeField] private float resetXPosition = -16.5f; 

    private float loopOffset;

    void Awake()
    {
        _levelManager = LevelManager.Instance;

        if (_levelManager == null)
        {
            Debug.LogError("BgManager: LevelManager 인스턴스를 찾을 수 없습니다! 배경이 움직이지 않을 수 있습니다.");
            enabled = false;
            return;
        }

        if (backgroundObjects == null || backgroundObjects.Length == 0)
        {
            Debug.LogError("BgManager: backgroundObjects 배열이 비어 있습니다! 배경 오브젝트를 할당해주세요.");
            enabled = false; 
            return;
        }
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
            // 배경 오브젝트를 현재 스크롤 속도에 따라 왼쪽으로 이동시킵니다.
            background.transform.Translate(Vector2.left * currentScrollSpeed * Time.deltaTime);

            // 현재 배경 오브젝트의 왼쪽 가장자리 X 좌표를 계산합니다.
            // 오브젝트의 중앙 X 위치에서 오브젝트 너비의 절반을 뺍니다.
            float currentLeftEdgeX = background.transform.position.x - (backgroundWidth / 2f);

            // 만약 배경의 왼쪽 가장자리가 재배치 기준점(resetXPosition)보다 작아지면
            // (즉, 화면 왼쪽 밖으로 완전히 나갔다면)
            if (currentLeftEdgeX < resetXPosition)
            {
                // 현재 위치에 loopOffset을 더하여 재배치합니다.
                // 이렇게 하면 오브젝트가 리스트의 맨 뒤로 간 것처럼 화면 오른쪽 끝에 다시 나타납니다.
                background.transform.position += new Vector3(loopOffset, 0, 0);
            }
        }
    }
}