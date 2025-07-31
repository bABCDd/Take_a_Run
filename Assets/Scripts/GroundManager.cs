using UnityEngine;

public class GroundManager : MonoBehaviour
{
    private LevelManager _levelManager;

    [SerializeField] private GameObject[] groundObjects;

    [SerializeField] private float groundWidth = 13.5f;


    [SerializeField] private float resetXPosition = -15.64f;


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