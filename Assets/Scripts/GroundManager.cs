using UnityEngine;

public class GroundManager : MonoBehaviour
{
    private LevelManager _levelManager;

    [SerializeField] private GameObject[] groundObjects;

    [SerializeField] private float groundWidth = 13.5f;


    [SerializeField] private float resetXPosition = -15.64f;


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