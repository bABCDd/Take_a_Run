// LevelManager.cs
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; } // �̱��� ����

    [Header("Game Speed Control")]
    [SerializeField] private float baseGameSpeed = 5f;          // ���� �⺻ �ӵ�
    [SerializeField] private float maxGameSpeed = 15f;          // ���� ������ �ִ� �ӵ�
    [SerializeField] private float speedIncreaseRate = 0.1f;    // �ʴ� �ӵ� ������
    [SerializeField] private float speedIncreaseStartTime = 0f; // �ӵ� ���� ���� �ð� (��: 0�ʺ���)

    [Header("Obstacle Spawn Interval Control")]
    [SerializeField] private float baseSpawnInterval = 2f;      // ���� �⺻ ���� ����
    [SerializeField] private float minSpawnInterval = 0.5f;     // ���� ������ �ּ� ���� ����
    [SerializeField] private float intervalDecreaseRate = 0.05f;// �ʴ� ���� ���ҷ�
    [SerializeField] private float intervalDecreaseStartTime = 0f; // ���� ���� ���� �ð� (��: 0�ʺ���)

    [Header("Difficulty Start Delay")]
    [SerializeField] private float difficultyStartDelay = 30f; // ���̵� ���� ���۱����� ������ (��: 30��)

    private float _gameplayTime;
    private float _currentCalculatedGameSpeed;
    private float _currentCalculatedSpawnInterval;

    void Awake()
    {
        if (Instance != null && Instance != this) Destroy(gameObject);
        else Instance = this;
        // DontDestroyOnLoad(gameObject); // �� ��ȯ �� �ı����� �ʰ� �Ϸ��� �ּ� ����
    }

    void Start()
    {
        _gameplayTime = 0f;
        _currentCalculatedGameSpeed = baseGameSpeed;
        _currentCalculatedSpawnInterval = baseSpawnInterval;
    }

    void Update()
    {
        _gameplayTime += Time.deltaTime;

        // �ӵ��� ���� ��꿡 ����� "���̵� ���� �ð�"
        float timeAfterDelay = Mathf.Max(0, _gameplayTime - difficultyStartDelay);

        // ���� ���� �ӵ� ���
        _currentCalculatedGameSpeed = Mathf.Min(maxGameSpeed, baseGameSpeed + (timeAfterDelay * speedIncreaseRate));

        // ���� ��ֹ� ���� ���� ���
        _currentCalculatedSpawnInterval = Mathf.Max(minSpawnInterval, baseSpawnInterval - (timeAfterDelay * intervalDecreaseRate));
    }

    public float GetCurrentGameSpeed() { return _currentCalculatedGameSpeed; }
    public float GetCurrentSpawnInterval() { return _currentCalculatedSpawnInterval; }
    public float GetGameplayTime() { return _gameplayTime; }
}