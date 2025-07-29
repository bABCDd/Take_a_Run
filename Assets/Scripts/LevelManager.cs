using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    [Header("Game Speed Control")]
    [SerializeField] private float baseGameSpeed = 5f;          // ���� ���� �� �⺻ �̵� �ӵ�
    [SerializeField] private float increasedGameSpeed = 8f;     // ���̵� ��� �� ����� �ӵ� (��: 30�� ����)


    [Header("Obstacle Spawn Interval Control")]
    [SerializeField] private float baseSpawnInterval = 2f;      // ���� ���� �� �⺻ ��ֹ� ���� ����
    [SerializeField] private float minSpawnInterval = 0.5f;     // ���� ������ �ּ� ��ֹ� ���� ����
    [SerializeField] private float intervalDecreaseRate = 0.05f;// �ʴ� ���� ���� ���ҷ� 

    [Header("Difficulty Trigger Time")] // ���̵� ���� '����'�� �ƴ϶� 'Ʈ����' �������� ����
    [SerializeField] private float difficultyIncreaseTime = 30f; // ���̵�(�ӵ�/����)�� ������ ��Ȯ�� ���� �ð� (��: 30��)

    private float _gameplayTime;
    private float _currentCalculatedGameSpeed;
    private float _currentCalculatedSpawnInterval;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    void Start()
    {
        _gameplayTime = 0f;
        _currentCalculatedGameSpeed = baseGameSpeed; // ������ �⺻ �ӵ���
        _currentCalculatedSpawnInterval = baseSpawnInterval;
    }

    void Update()
    {
        _gameplayTime += Time.deltaTime;

        // ���� �ӵ� ����: difficultyIncreaseTime�� �������� �ӵ��� ����
        if (_gameplayTime >= difficultyIncreaseTime)
        {
            _currentCalculatedGameSpeed = increasedGameSpeed; // Ư�� �ð� ���� ������ �ӵ� ����
        }
        else
        {
            _currentCalculatedGameSpeed = baseGameSpeed; // �� �������� �⺻ �ӵ� ����
        }

        // ��ֹ� ���� ���� ����: difficultyIncreaseTime�� �������� ���� ������ ����
        // ������ �ð��� ���� ���������� �����ϵ��� ���� (���� ��� ����)
        float timeForIntervalCalc = Mathf.Max(0, _gameplayTime - difficultyIncreaseTime); // ������ �� �ð� ���
        _currentCalculatedSpawnInterval = Mathf.Max(minSpawnInterval, baseSpawnInterval - (timeForIntervalCalc * intervalDecreaseRate));
    }

    public float GetCurrentGameSpeed()
    {
        return _currentCalculatedGameSpeed;
    }

    public float GetCurrentSpawnInterval()
    {
        return _currentCalculatedSpawnInterval;
    }

    public float GetGameplayTime()
    {
        return _gameplayTime;
    }
}