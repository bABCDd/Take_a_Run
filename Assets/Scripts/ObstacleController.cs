// ObstacleController.cs
using UnityEngine;

public class ObstacleController : MonoBehaviour
{
    [SerializeField] private float _damageAmount = 1;

    private float _currentMoveSpeed; // LevelManager에서 받아올 현재 이동 속도
    private float _despawnXPosition = -12f; // 화면 왼쪽 밖으로 나가는 X 좌표

    void Update()
    {
        // 장애물 왼쪽으로 이동
        transform.Translate(Vector2.left * _currentMoveSpeed * Time.deltaTime);

        // 화면 밖으로 나가면 비활성화 (오브젝트 풀링 사용 시) 또는 파괴
        if (transform.position.x < _despawnXPosition)
        {
            gameObject.SetActive(false); // 오브젝트 풀로 반환 (권장)
            // Destroy(gameObject); // 단순 파괴 시
        }
    }

    // ObstacleSpawner가 장애물을 생성/활성화할 때 호출하여 속도 설정
    public void Init(float speed)
    {
        _currentMoveSpeed = speed;
        gameObject.SetActive(true); // 오브젝트 풀에서 꺼낼 때 활성화
    }

    // 플레이어와의 충돌 감지 (장애물의 Collider2D에 Is Trigger가 체크되어 있어야 함)
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // 플레이어의 ResourceController 컴포넌트 가져오기
            ResourceController playerLives = collision.GetComponent<ResourceController>(); // 변수 이름 변경
            if (playerLives != null)
            {
                // ChangeLives 함수를 호출하여 목숨 감소
                playerLives.ChangeLives(-1); // 장애물에 닿으면 목숨 1개 감소
                // 혹은 playerLives.ChangeLives(-(int)_damageAmount); 로 _damageAmount 만큼 감소 가능
            }

            // 장애물은 충돌 후 사라지거나 재활용됩니다.
            gameObject.SetActive(false); // 오브젝트 풀로 반환 (권장)
            // Destroy(gameObject); // 단순 파괴 시
        }
    }
}