using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    [Header("Game Speed Control")]
    [SerializeField] private float baseGameSpeed = 5f;          // 게임 시작 시 기본 이동 속도
    [SerializeField] private float increasedGameSpeed = 8f;     // 난이도 상승 후 적용될 속도 (예: 30초 이후)


    [Header("Obstacle Spawn Interval Control")]
    [SerializeField] private float baseSpawnInterval = 2f;      // 게임 시작 시 기본 장애물 스폰 간격
    [SerializeField] private float minSpawnInterval = 0.5f;     // 도달 가능한 최소 장애물 스폰 간격
    [SerializeField] private float intervalDecreaseRate = 0.05f;// 초당 스폰 간격 감소량 

    [Header("Difficulty Trigger Time")] // 난이도 조절 '시작'이 아니라 '트리거' 시점으로 변경
    [SerializeField] private float difficultyIncreaseTime = 30f; // 난이도(속도/간격)가 증가할 정확한 게임 시간 (예: 30초)

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
        _currentCalculatedGameSpeed = baseGameSpeed; // 시작은 기본 속도로
        _currentCalculatedSpawnInterval = baseSpawnInterval;
    }

    void Update()
    {
        _gameplayTime += Time.deltaTime;

        // 게임 속도 조절: difficultyIncreaseTime을 기준으로 속도를 변경
        if (_gameplayTime >= difficultyIncreaseTime)
        {
            _currentCalculatedGameSpeed = increasedGameSpeed; // 특정 시간 이후 증가된 속도 적용
        }
        else
        {
            _currentCalculatedGameSpeed = baseGameSpeed; // 그 전까지는 기본 속도 유지
        }

        // 장애물 스폰 간격 조절: difficultyIncreaseTime을 기준으로 간격 감소율 적용
        // 간격은 시간에 따라 점진적으로 감소하도록 유지 (기존 방식 유지)
        float timeForIntervalCalc = Mathf.Max(0, _gameplayTime - difficultyIncreaseTime); // 딜레이 후 시간 계산
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