using UnityEngine;

public class DeadzoneController : MonoBehaviour
{
    // LevelManager�κ��� �޾ƿ� ���� �̵� �ӵ� (ObstacleController�� ����)
    private float _currentMoveSpeed;

    // ȭ�� ������ ������ �� �����̰� �ı��� X ��ǥ (ObstacleController�� ����)
    [SerializeField] private float _despawnXPosition = -12f; // Inspector���� ���� �����ϰ� SerializeField

    void Update()
    {
        // ������ �������� �̵� (ObstacleController�� ����)
        transform.Translate(Vector2.left * _currentMoveSpeed * Time.deltaTime);

        // �����̰� ȭ�� ������ �������� Ȯ�� �� �ı� (ObstacleController�� ����)
        if (transform.position.x < _despawnXPosition)
        {
            Destroy(gameObject);
        }
    }

    // ObstacleSpawner���� �ӵ��� �ʱ�ȭ�� �� �ֵ��� Init �޼��� (ObstacleController�� ����)
    public void Init(float speed)
    {
        _currentMoveSpeed = speed;
    }
}