using UnityEngine;

public class BgLooper : MonoBehaviour
{
    private LevelManager _levelManager;

    // 인스펙터에서 설정할 배경 이미지의 실제 너비 (유니티 유닛 기준)
    // 이 스크립트가 배경 오브젝트에 붙어 있다면, 해당 배경 이미지의 폭을 입력합니다.
    [SerializeField] private float backgroundWidth;

    // 이 배경 오브젝트가 화면 왼쪽 밖으로 얼마나 나가야 다시 재배치될지 결정하는 X 좌표
    // 카메라의 왼쪽 끝보다 살짝 더 왼쪽으로 설정하여 배경이 완전히 사라진 후 재배치되도록 합니다.
    // 예: -20f (게임 스케일에 따라 조정 필요)
    [SerializeField] private float resetXPosition;

    // 이 배경이 재배치될 때 이동할 목표 X 좌표까지의 오프셋
    // 보통 배치된 배경 오브젝트들의 총 너비 (예: backgroundWidth * 2f 또는 * 3f)
    [SerializeField] private float loopOffset;

    void Awake()
    {
        _levelManager = LevelManager.Instance;
        if (_levelManager == null)
        {
            Debug.LogError("BgLooper: LevelManager 인스턴스를 찾을 수 없습니다! 배경이 움직이지 않을 수 있습니다.");
        }

        // BoxCollider2D가 배경 오브젝트에 붙어 있다면, 너비를 자동으로 가져와 backgroundWidth 초기화
        BoxCollider2D collider = GetComponent<BoxCollider2D>();
        if (collider != null)
        {
            backgroundWidth = collider.size.x;
            // 만약 2개의 배경이 루프된다면 loopOffset을 자동으로 설정 가능:
            // loopOffset = backgroundWidth * 2f; 
        }
        else
        {
            Debug.LogWarning("BgLooper: BoxCollider2D를 찾을 수 없습니다. backgroundWidth를 수동으로 설정해주세요.");
        }

        // resetXPosition과 loopOffset의 기본값 설정 (인스펙터에서 덮어쓸 수 있음)
        // 이 값들은 게임의 실제 카메라 시야와 배경 크기에 따라 달라지므로,
        // 유니티 에디터에서 직접 플레이하면서 조정하는 것이 가장 좋습니다.
        if (resetXPosition == 0) resetXPosition = -10f; // 예시 값
        if (loopOffset == 0) loopOffset = 20f; // 예시 값
    }

    void Update()
    {
        // LevelManager로부터 현재 게임 속도를 가져와 배경을 왼쪽으로 이동시킵니다.
        float currentScrollSpeed = 0f;
        if (_levelManager != null)
        {
            currentScrollSpeed = _levelManager.GetCurrentGameSpeed();
        }

        transform.Translate(Vector2.left * currentScrollSpeed * Time.deltaTime);

        // 배경이 화면 왼쪽 밖으로 완전히 나갔는지 확인
        // (현재 배경의 X 좌표 - 배경의 절반 너비)가 재배치 지점보다 작으면
        if (transform.position.x - (backgroundWidth / 2f) < resetXPosition)
        {
            // 배경을 현재 위치에서 loopOffset만큼 오른쪽으로 이동시켜 무한 루프처럼 보이게 합니다.
            transform.position += new Vector3(loopOffset, 0, 0);
        }
    }
}