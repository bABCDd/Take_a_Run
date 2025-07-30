using UnityEngine;

public class BgLooper : MonoBehaviour
{
    private LevelManager _levelManager;

    // �ν����Ϳ��� ������ ��� �̹����� ���� �ʺ� (����Ƽ ���� ����)
    // �� ��ũ��Ʈ�� ��� ������Ʈ�� �پ� �ִٸ�, �ش� ��� �̹����� ���� �Է��մϴ�.
    [SerializeField] private float backgroundWidth;

    // �� ��� ������Ʈ�� ȭ�� ���� ������ �󸶳� ������ �ٽ� ���ġ���� �����ϴ� X ��ǥ
    // ī�޶��� ���� ������ ��¦ �� �������� �����Ͽ� ����� ������ ����� �� ���ġ�ǵ��� �մϴ�.
    // ��: -20f (���� �����Ͽ� ���� ���� �ʿ�)
    [SerializeField] private float resetXPosition;

    // �� ����� ���ġ�� �� �̵��� ��ǥ X ��ǥ������ ������
    // ���� ��ġ�� ��� ������Ʈ���� �� �ʺ� (��: backgroundWidth * 2f �Ǵ� * 3f)
    [SerializeField] private float loopOffset;

    void Awake()
    {
        _levelManager = LevelManager.Instance;
        if (_levelManager == null)
        {
            Debug.LogError("BgLooper: LevelManager �ν��Ͻ��� ã�� �� �����ϴ�! ����� �������� ���� �� �ֽ��ϴ�.");
        }

        // BoxCollider2D�� ��� ������Ʈ�� �پ� �ִٸ�, �ʺ� �ڵ����� ������ backgroundWidth �ʱ�ȭ
        BoxCollider2D collider = GetComponent<BoxCollider2D>();
        if (collider != null)
        {
            backgroundWidth = collider.size.x;
            // ���� 2���� ����� �����ȴٸ� loopOffset�� �ڵ����� ���� ����:
            // loopOffset = backgroundWidth * 2f; 
        }
        else
        {
            Debug.LogWarning("BgLooper: BoxCollider2D�� ã�� �� �����ϴ�. backgroundWidth�� �������� �������ּ���.");
        }

        // resetXPosition�� loopOffset�� �⺻�� ���� (�ν����Ϳ��� ��� �� ����)
        // �� ������ ������ ���� ī�޶� �þ߿� ��� ũ�⿡ ���� �޶����Ƿ�,
        // ����Ƽ �����Ϳ��� ���� �÷����ϸ鼭 �����ϴ� ���� ���� �����ϴ�.
        if (resetXPosition == 0) resetXPosition = -10f; // ���� ��
        if (loopOffset == 0) loopOffset = 20f; // ���� ��
    }

    void Update()
    {
        // LevelManager�κ��� ���� ���� �ӵ��� ������ ����� �������� �̵���ŵ�ϴ�.
        float currentScrollSpeed = 0f;
        if (_levelManager != null)
        {
            currentScrollSpeed = _levelManager.GetCurrentGameSpeed();
        }

        transform.Translate(Vector2.left * currentScrollSpeed * Time.deltaTime);

        // ����� ȭ�� ���� ������ ������ �������� Ȯ��
        // (���� ����� X ��ǥ - ����� ���� �ʺ�)�� ���ġ �������� ������
        if (transform.position.x - (backgroundWidth / 2f) < resetXPosition)
        {
            // ����� ���� ��ġ���� loopOffset��ŭ ���������� �̵����� ���� ����ó�� ���̰� �մϴ�.
            transform.position += new Vector3(loopOffset, 0, 0);
        }
    }
}