using UnityEngine;

public class ItemController : MonoBehaviour
{
    // 아이템의 종류를 정의하는 enum
    public enum ItemType
    {
        Heal,
        Damage 
    }

    [Header("Item Type and Effects")]
    [SerializeField] private ItemType _itemType; 
    [SerializeField] private int _effectAmount = 1;

    [Header("Movement")]
    [SerializeField] private float _despawnXPosition = -12f;
    private float _currentMoveSpeed;


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
                if (_itemType == ItemType.Heal)
                {
                    player.Heal(_effectAmount); 
                    Debug.Log("Player Healed by: " + _effectAmount);
                }
                else if (_itemType == ItemType.Damage)
                {
                    player.TakeDamage(_effectAmount);
                    Debug.Log("Player Damaged by Item: " + _effectAmount);
                }
            }
            Destroy(gameObject);
        }
    }
}