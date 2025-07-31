using UnityEngine;

public class ItemController : MonoBehaviour
{
    // �������� ������ �����ϴ� enum
    public enum ItemType
    {
        Heal,   // ü�� ȸ�� ������
        Damage  // �÷��̾�� �������� �ִ� ������
    }

    [Header("Item Type and Effects")]
    [SerializeField] private ItemType _itemType; // �� �������� ������ �ν����Ϳ��� ����
    [SerializeField] private int _effectAmount = 1; // ü�� ȸ���� �Ǵ� ��������

    [Header("Movement")]
    [SerializeField] private float _despawnXPosition = -12f; // ȭ�� ���� ������ ������ �ı��� X ��ǥ
    private float _currentMoveSpeed; // LevelManager�κ��� �޾ƿ� ���� ���� �ӵ�

    [Header("Effects")]
    [SerializeField] private GameObject _collectEffectPrefab; // ������ ȹ�� �� ����� ����Ʈ ������

    void Update()
    {
        transform.Translate(Vector2.left * _currentMoveSpeed * Time.deltaTime);

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
                // ������ ������ ���� �ٸ� ���� ����
                if (_itemType == ItemType.Heal)
                {
                    player.Heal(_effectAmount); // �÷��̾��� ü���� ȸ����Ű�� �޼��� ȣ��
                    Debug.Log("Player Healed by: " + _effectAmount); // ����� �α�
                }
                else if (_itemType == ItemType.Damage)
                {
                    player.TakeDamage(_effectAmount); // �÷��̾�� �������� �ִ� �޼��� ȣ��
                    Debug.Log("Player Damaged by Item: " + _effectAmount); // ����� �α�
                }
            }

            // ȹ�� ����Ʈ �������� �Ҵ�Ǿ� �ִٸ�, ������ ��ġ�� ����Ʈ�� �����մϴ�.
            if (_collectEffectPrefab != null)
            {
                GameObject effect = Instantiate(_collectEffectPrefab, transform.position, Quaternion.identity);
                Destroy(effect, 1.5f);
            }

            // �������� ȿ���� �� �� ������� �ϹǷ� �ڽ��� �ı��մϴ�.
            Destroy(gameObject);
        }
    }
}