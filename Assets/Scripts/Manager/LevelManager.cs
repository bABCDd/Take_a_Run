using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    [Header("Game Speed Control")]
    [SerializeField] private float baseGameSpeed = 5f;          // ���� ���� �� �⺻ �̵� �ӵ�
    [SerializeField] private float maxGameSpeed = 15f;          // ���� ������ �ִ� �̵� �ӵ� 
    [SerializeField] private float speedIncreaseRate = 0.05f;   // �ʴ� �ӵ� ������ 

    [Header("Obstacle Spawn Interval Control")]
    [SerializeField] private float baseSpawnInterval = 2f;      // ���� ���� �� �⺻ ��ֹ� ���� ����
    [SerializeField] private float minSpawnInterval = 0.5f;     // ���� ������ �ּ� ��ֹ� ���� ����
    [SerializeField] private float intervalDecreaseRate = 0.05f;// �ʴ� ���� ���� ���ҷ�

    [Header("Difficulty Trigger Time")]
    [SerializeField] private float difficultyIncreaseStartTime = 30f; // ���̵�(�ӵ�/����) ������ ���۵� ��Ȯ�� ���� �ð�

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
        _currentCalculatedGameSpeed = baseGameSpeed;
        _currentCalculatedSpawnInterval = baseSpawnInterval;
    }

    void Update()
    {
        _gameplayTime += Time.deltaTime;
      
        float timeForDifficultyCalc = Mathf.Max(0, _gameplayTime - difficultyIncreaseStartTime);
       
        _currentCalculatedGameSpeed = Mathf.Min(maxGameSpeed, baseGameSpeed + (timeForDifficultyCalc * speedIncreaseRate));
        _currentCalculatedSpawnInterval = Mathf.Max(minSpawnInterval, baseSpawnInterval - (timeForDifficultyCalc * intervalDecreaseRate));
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