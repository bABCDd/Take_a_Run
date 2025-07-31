using UnityEngine;

public class GroundManager : MonoBehaviour
{
    private LevelManager _levelManager;

    // �ν����Ϳ� �Ҵ��� �� ������Ʈ�� (Ground_1, Ground_2)
    [SerializeField] private GameObject[] groundObjects;

    // �� �� �̹����� ���� �ʺ� (����Ƽ ���� ����) - ���⼭ 13.5f�� ����� �մϴ�.
    [SerializeField] private float groundWidth = 13.5f;

    // ���� ȭ�� ���� ������ �󸶳� ������ ���ġ���� �����ϴ� X ��ǥ
    // ī�޶� ���� ������ ��¦ �� �������� �����մϴ�.
    [SerializeField] private float resetXPosition = -15.64f;

    // ���ġ�� �� �̵��� ��ǥ X ��ǥ������ ������ (�� �� ������ �� �ʺ�)
    // groundWidth * groundObjects.Length�� ���˴ϴ�.
    private float loopOffset;

    void Awake()
    {
        _levelManager = LevelManager.Instance;
        if (_levelManager == null)
        {
            Debug.LogError("GroundManager: LevelManager �ν��Ͻ��� ã�� �� �����ϴ�! ���� �������� ���� �� �ֽ��ϴ�.");
        }

        if (groundObjects == null || groundObjects.Length == 0)
        {
            Debug.LogError("GroundManager: groundObjects �迭�� ��� �ֽ��ϴ�! �� ������Ʈ�� �Ҵ����ּ���.");
            enabled = false;
            return;
        }

        // loopOffset�� �������� ���: �� ������Ʈ ��ü�� �ʺ� ��
        loopOffset = groundWidth * groundObjects.Length;
    }

    void Update()
    {
        float currentScrollSpeed = 0f;
        if (_levelManager != null)
        {
            currentScrollSpeed = _levelManager.GetCurrentGameSpeed();
        }

        foreach (GameObject ground in groundObjects)
        {
            // �� ������Ʈ�� �������� �̵���ŵ�ϴ�.
            ground.transform.Translate(Vector2.left * currentScrollSpeed * Time.deltaTime);

            // ���� �� ������Ʈ�� ���� �����ڸ� X ��ǥ�� ����մϴ�.
            float currentLeftEdgeX = ground.transform.position.x - (groundWidth / 2f);

            // ���� ȭ�� ���� ������ ������ �������� Ȯ���մϴ�.
            if (currentLeftEdgeX < resetXPosition)
            {
                // ���� ��ġ�� loopOffset�� ���Ͽ� ���ġ (�ڱ� �ڽ��� loopOffset��ŭ �̵�)
                ground.transform.position += new Vector3(loopOffset, 0, 0);
            }
        }
    }
}