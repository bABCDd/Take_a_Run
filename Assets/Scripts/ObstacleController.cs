using UnityEngine;

public class ObstacleController : MonoBehaviour
{
    [Header("Obstacle Properties")]
    [SerializeField] private int _livesToDecrease = 1;

    // ȭ�� ������ ������ �� ��ֹ��� �ı��� X ��ǥ (���� ī�޶� ���� ��)
    [SerializeField] private float _despawnXPosition = -12f;

    private float _currentMoveSpeed; // LevelManager�κ��� �޾ƿ� ���� �̵� �ӵ�

    [Header("Effects")]
    [SerializeField] private GameObject _impactEffectPrefab;

    void Update()
    {
        // ��ֹ� �������� �̵�
        transform.Translate(Vector2.left * _currentMoveSpeed * Time.deltaTime);

        // ��ֹ��� ȭ�� ������ �������� Ȯ�� �� �ı�
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
        if (collision.CompareTag("Player"))
        {
            Player player = collision.GetComponent<Player>();
            if (player != null)
            {
                player.TakeDamage(_livesToDecrease);

                if (_impactEffectPrefab != null)
                {
                    GameObject effect = Instantiate(_impactEffectPrefab, transform.position, Quaternion.identity);
                    Destroy(effect, 1.5f);
                }
                Destroy(gameObject);
            }
        }
    }
}