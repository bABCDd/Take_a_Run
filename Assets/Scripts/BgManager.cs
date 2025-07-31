using UnityEngine;

public class BgManager : MonoBehaviour
{
    private LevelManager _levelManager;

    // �ν����Ϳ� �Ҵ��� ��� ������Ʈ�� (Background_1, Background_2)
    [SerializeField] private GameObject[] backgroundObjects;

    // �� ��� �̹����� ���� �ʺ� (����Ƽ ���� ����) - ���⼭ 13.5f�� ����� �մϴ�.
    [SerializeField] private float backgroundWidth = 13.5f;

    // ����� ȭ�� ���� ������ �󸶳� ������ ���ġ���� �����ϴ� X ��ǥ
    [SerializeField] private float resetXPosition = -15.64f;

    // ���ġ�� �� �̵��� ��ǥ X ��ǥ������ ������ (�� ��� ������ �� �ʺ�)
    private float loopOffset;

    void Awake()
    {
        _levelManager = LevelManager.Instance;
        if (_levelManager == null)
        {
            Debug.LogError("BgManager: LevelManager �ν��Ͻ��� ã�� �� �����ϴ�! ����� �������� ���� �� �ֽ��ϴ�.");
        }

        if (backgroundObjects == null || backgroundObjects.Length == 0)
        {
            Debug.LogError("BgManager: backgroundObjects �迭�� ��� �ֽ��ϴ�! ��� ������Ʈ�� �Ҵ����ּ���.");
            enabled = false;
            return;
        }

        // loopOffset�� �������� ���: ��� ������Ʈ ��ü�� �ʺ� ��
        loopOffset = backgroundWidth * backgroundObjects.Length;
    }

    void Update()
    {
        float currentScrollSpeed = 0f;
        if (_levelManager != null)
        {
            currentScrollSpeed = _levelManager.GetCurrentGameSpeed();
        }

        foreach (GameObject background in backgroundObjects)
        {
            // ��� ������Ʈ�� �������� �̵���ŵ�ϴ�.
            background.transform.Translate(Vector2.left * currentScrollSpeed * Time.deltaTime);

            // ���� ��� ������Ʈ�� ���� �����ڸ� X ��ǥ�� ����մϴ�.
            float currentLeftEdgeX = background.transform.position.x - (backgroundWidth / 2f);

            // ����� ȭ�� ���� ������ ������ �������� Ȯ���մϴ�.
            if (currentLeftEdgeX < resetXPosition)
            {
                // ���� ��ġ�� loopOffset�� ���Ͽ� ���ġ
                background.transform.position += new Vector3(loopOffset, 0, 0);
            }
        }
    }
}