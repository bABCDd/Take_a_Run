using UnityEngine;

public class ObstacleController : MonoBehaviour
{
    [Header("Obstacle Properties")]
    [SerializeField] private int _livesToDecrease = 1;

    // 화면 밖으로 나가면 이 장애물이 파괴될 X 좌표 (보통 카메라 왼쪽 밖)
    [SerializeField] private float _despawnXPosition = -12f;

    private float _currentMoveSpeed; // LevelManager로부터 받아올 현재 이동 속도

    [Header("Effects")]
    [SerializeField] private GameObject _impactEffectPrefab;

    void Update()
    {
        // 장애물 왼쪽으로 이동
        transform.Translate(Vector2.left * _currentMoveSpeed * Time.deltaTime);

        // 장애물이 화면 밖으로 나갔는지 확인 후 파괴
        if (transform.position.x < _despawnXPosition)
        {
            Destroy(gameObject);
        }
    }

    public void Init(float speed)
    {
        _currentMoveSpeed = speed;
    }


    void OnTriggerEnter2D(Collider2D collision)
    {
        // 충돌한 오브젝트가 "Player" 태그를 가졌는지 확인
        if (collision.CompareTag("Player"))
        {
            ResourceController playerLives = collision.GetComponent<ResourceController>();
            if (playerLives != null)
            {
                // 플레이어의 목숨 감소
                playerLives.ChangeLives(-_livesToDecrease);
            }

            if (_impactEffectPrefab != null)
            {
                GameObject effect = Instantiate(_impactEffectPrefab, transform.position, Quaternion.identity);

                Destroy(effect, 1.5f);
            }

            Destroy(gameObject);
        }
    }
}