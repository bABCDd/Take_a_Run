using UnityEngine;

public class DeadzoneController : MonoBehaviour
{
    // LevelManager로부터 받아올 현재 이동 속도 (ObstacleController와 동일)
    private float _currentMoveSpeed;

    // 화면 밖으로 나가면 이 구덩이가 파괴될 X 좌표 (ObstacleController와 동일)
    [SerializeField] private float _despawnXPosition = -12f; // Inspector에서 조절 가능하게 SerializeField

    void Update()
    {
        // 구덩이 왼쪽으로 이동 (ObstacleController와 동일)
        transform.Translate(Vector2.left * _currentMoveSpeed * Time.deltaTime);

        // 구덩이가 화면 밖으로 나갔는지 확인 후 파괴 (ObstacleController와 동일)
        if (transform.position.x < _despawnXPosition)
        {
            Destroy(gameObject);
        }
    }

    // ObstacleSpawner에서 속도를 초기화할 수 있도록 Init 메서드 (ObstacleController와 동일)
    public void Init(float speed)
    {
        _currentMoveSpeed = speed;
    }
}