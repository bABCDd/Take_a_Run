// ObstacleController.cs
using UnityEngine;

public class ObstacleController : MonoBehaviour
{
    [SerializeField] private float _damageAmount = 1;

    private float _currentMoveSpeed; // LevelManager���� �޾ƿ� ���� �̵� �ӵ�
    private float _despawnXPosition = -12f; // ȭ�� ���� ������ ������ X ��ǥ

    void Update()
    {
        // ��ֹ� �������� �̵�
        transform.Translate(Vector2.left * _currentMoveSpeed * Time.deltaTime);

        // ȭ�� ������ ������ ��Ȱ��ȭ (������Ʈ Ǯ�� ��� ��) �Ǵ� �ı�
        if (transform.position.x < _despawnXPosition)
        {
            gameObject.SetActive(false); // ������Ʈ Ǯ�� ��ȯ (����)
            // Destroy(gameObject); // �ܼ� �ı� ��
        }
    }

    // ObstacleSpawner�� ��ֹ��� ����/Ȱ��ȭ�� �� ȣ���Ͽ� �ӵ� ����
    public void Init(float speed)
    {
        _currentMoveSpeed = speed;
        gameObject.SetActive(true); // ������Ʈ Ǯ���� ���� �� Ȱ��ȭ
    }

    // �÷��̾���� �浹 ���� (��ֹ��� Collider2D�� Is Trigger�� üũ�Ǿ� �־�� ��)
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // �÷��̾��� ResourceController ������Ʈ ��������
            ResourceController playerLives = collision.GetComponent<ResourceController>(); // ���� �̸� ����
            if (playerLives != null)
            {
                // ChangeLives �Լ��� ȣ���Ͽ� ��� ����
                playerLives.ChangeLives(-1); // ��ֹ��� ������ ��� 1�� ����
                // Ȥ�� playerLives.ChangeLives(-(int)_damageAmount); �� _damageAmount ��ŭ ���� ����
            }

            // ��ֹ��� �浹 �� ������ų� ��Ȱ��˴ϴ�.
            gameObject.SetActive(false); // ������Ʈ Ǯ�� ��ȯ (����)
            // Destroy(gameObject); // �ܼ� �ı� ��
        }
    }
}