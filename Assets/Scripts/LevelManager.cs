// LevelManager.cs
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; } // 싱글톤 패턴

    [Header("Game Speed Control")]
    [SerializeField] private float baseGameSpeed = 5f;          // 시작 기본 속도
    [SerializeField] private float maxGameSpeed = 15f;          // 도달 가능한 최대 속도
    [SerializeField] private float speedIncreaseRate = 0.1f;    // 초당 속도 증가량
    [SerializeField] private float speedIncreaseStartTime = 0f; // 속도 증가 시작 시간 (예: 0초부터)

    [Header("Obstacle Spawn Interval Control")]
    [SerializeField] private float baseSpawnInterval = 2f;      // 시작 기본 스폰 간격
    [SerializeField] private float minSpawnInterval = 0.5f;     // 도달 가능한 최소 스폰 간격
    [SerializeField] private float intervalDecreaseRate = 0.05f;// 초당 간격 감소량
    [SerializeField] private float intervalDecreaseStartTime = 0f; // 간격 감소 시작 시간 (예: 0초부터)

    [Header("Difficulty Start Delay")]
    [SerializeField] private float difficultyStartDelay = 30f; // 난이도 조절 시작까지의 딜레이 (예: 30초)

    private float _gameplayTime;
    private float _currentCalculatedGameSpeed;
    private float _currentCalculatedSpawnInterval;

    void Awake()
    {
        if (Instance != null && Instance != this) Destroy(gameObject);
        else Instance = this;
        // DontDestroyOnLoad(gameObject); // 씬 전환 시 파괴되지 않게 하려면 주석 해제
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

        // 속도와 간격 계산에 사용할 "난이도 적용 시간"
        float timeAfterDelay = Mathf.Max(0, _gameplayTime - difficultyStartDelay);

        // 현재 게임 속도 계산
        _currentCalculatedGameSpeed = Mathf.Min(maxGameSpeed, baseGameSpeed + (timeAfterDelay * speedIncreaseRate));

        // 현재 장애물 스폰 간격 계산
        _currentCalculatedSpawnInterval = Mathf.Max(minSpawnInterval, baseSpawnInterval - (timeAfterDelay * intervalDecreaseRate));
    }

    public float GetCurrentGameSpeed() { return _currentCalculatedGameSpeed; }
    public float GetCurrentSpawnInterval() { return _currentCalculatedSpawnInterval; }
    public float GetGameplayTime() { return _gameplayTime; }
}