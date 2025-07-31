using UnityEngine;

public class BgManager : MonoBehaviour
{
    private LevelManager _levelManager;

    [SerializeField] private GameObject[] backgroundObjects;

    [SerializeField] private float backgroundWidth = 13.5f;

    [SerializeField] private float resetXPosition = -16.5f; 

    private float loopOffset;

    void Awake()
    {
        _levelManager = LevelManager.Instance;

        if (_levelManager == null)
        {
            Debug.LogError("BgManager: LevelManager �ν��Ͻ��� ã�� �� �����ϴ�! ����� �������� ���� �� �ֽ��ϴ�.");
            enabled = false;
            return;
        }

        if (backgroundObjects == null || backgroundObjects.Length == 0)
        {
            Debug.LogError("BgManager: backgroundObjects �迭�� ��� �ֽ��ϴ�! ��� ������Ʈ�� �Ҵ����ּ���.");
            enabled = false; 
            return;
        }
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
            // ��� ������Ʈ�� ���� ��ũ�� �ӵ��� ���� �������� �̵���ŵ�ϴ�.
            background.transform.Translate(Vector2.left * currentScrollSpeed * Time.deltaTime);

            // ���� ��� ������Ʈ�� ���� �����ڸ� X ��ǥ�� ����մϴ�.
            // ������Ʈ�� �߾� X ��ġ���� ������Ʈ �ʺ��� ������ ���ϴ�.
            float currentLeftEdgeX = background.transform.position.x - (backgroundWidth / 2f);

            // ���� ����� ���� �����ڸ��� ���ġ ������(resetXPosition)���� �۾�����
            // (��, ȭ�� ���� ������ ������ �����ٸ�)
            if (currentLeftEdgeX < resetXPosition)
            {
                // ���� ��ġ�� loopOffset�� ���Ͽ� ���ġ�մϴ�.
                // �̷��� �ϸ� ������Ʈ�� ����Ʈ�� �� �ڷ� �� ��ó�� ȭ�� ������ ���� �ٽ� ��Ÿ���ϴ�.
                background.transform.position += new Vector3(loopOffset, 0, 0);
            }
        }
    }
}