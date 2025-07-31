using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    [Header("Game Speed Control")]
    [SerializeField] private float baseGameSpeed = 5f;          // 게임 시작 시 기본 이동 속도
    [SerializeField] private float maxGameSpeed = 15f;          // 도달 가능한 최대 이동 속도 
    [SerializeField] private float speedIncreaseRate = 0.05f;   // 초당 속도 증가량 

    [Header("Obstacle Spawn Interval Control")]
    [SerializeField] private float baseSpawnInterval = 2f;      // 게임 시작 시 기본 장애물 스폰 간격
    [SerializeField] private float minSpawnInterval = 0.5f;     // 도달 가능한 최소 장애물 스폰 간격
    [SerializeField] private float intervalDecreaseRate = 0.05f;// 초당 스폰 간격 감소량

    [Header("Difficulty Trigger Time")]
    [SerializeField] private float difficultyIncreaseStartTime = 30f; // 난이도(속도/간격) 증가가 시작될 정확한 게임 시간

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