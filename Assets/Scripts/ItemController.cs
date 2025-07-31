using UnityEngine;

public class ItemController : MonoBehaviour
{
    // 아이템의 종류를 정의하는 enum
    public enum ItemType
    {
        Heal,   // 체력 회복 아이템
        Damage  // 플레이어에게 데미지를 주는 아이템
    }

    [Header("Item Type and Effects")]
    [SerializeField] private ItemType _itemType; // 이 아이템의 종류를 인스펙터에서 설정
    [SerializeField] private int _effectAmount = 1; // 체력 회복량 또는 데미지량

    [Header("Movement")]
    [SerializeField] private float _despawnXPosition = -12f; // 화면 왼쪽 밖으로 나가면 파괴될 X 좌표
    private float _currentMoveSpeed; // LevelManager로부터 받아올 현재 게임 속도

    [Header("Effects")]
    [SerializeField] private GameObject _collectEffectPrefab; // 아이템 획득 시 재생될 이펙트 프리팹

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
                // 아이템 종류에 따라 다른 동작 수행
                if (_itemType == ItemType.Heal)
                {
                    player.Heal(_effectAmount); // 플레이어의 체력을 회복시키는 메서드 호출
                    Debug.Log("Player Healed by: " + _effectAmount); // 디버그 로그
                }
                else if (_itemType == ItemType.Damage)
                {
                    player.TakeDamage(_effectAmount); // 플레이어에게 데미지를 주는 메서드 호출
                    Debug.Log("Player Damaged by Item: " + _effectAmount); // 디버그 로그
                }
            }

            // 획득 이펙트 프리팹이 할당되어 있다면, 아이템 위치에 이펙트를 생성합니다.
            if (_collectEffectPrefab != null)
            {
                GameObject effect = Instantiate(_collectEffectPrefab, transform.position, Quaternion.identity);
                Destroy(effect, 1.5f);
            }

            // 아이템은 효과를 준 후 사라져야 하므로 자신을 파괴합니다.
            Destroy(gameObject);
        }
    }
}