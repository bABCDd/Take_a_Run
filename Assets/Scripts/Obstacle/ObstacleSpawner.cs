using UnityEngine;
using System.Collections;

public class ObstacleSpawner : MonoBehaviour
{
    [Header("Obstacle Prefabs")]
    // 위에서 생성되는 장애물 프리팹들 (슬라이드로 피함)
    public GameObject[] topObstaclePrefabs;
    // 아래서 생성되는 장애물 프리팹들 (점프로 피함)
    public GameObject[] bottomObstaclePrefabs;

    [Header("Spawn Positions")]
    public float spawnXPosition = 12f; // 장애물이 생성될 X 좌표 (화면 우측 밖)

    // 위에서 생성되는 장애물의 Y 좌표 (보통 플레이어 점프 최대 높이 근처)
    public float[] topSpawnYPositions = { 2f }; // 예시: 플레이어가 슬라이드할 수 있는 높이
    // 아래서 생성되는 장애물의 Y 좌표 (보통 플레이어 발 아래)
    public float[] bottomSpawnYPositions = { -2f, -1.5f }; // 예시: 플레이어가 점프할 수 있는 높이

    private float _timer; // 다음 장애물 생성을 위한 타이머
    private LevelManager _levelManager; 

    void Start()
    {
        _timer = 0f; // 타이머 초기화

        _levelManager = LevelManager.Instance;
        if (_levelManager == null)
        {
            Debug.LogError("LevelManager 인스턴스를 찾을 수 없습니다! 씬에 LevelManager 오브젝트가 있는지 확인하세요.");
        }
    }

    void Update()
    {
        // LevelManager로부터 현재 계산된 스폰 간격을 가져옵니다.
        float currentSpawnInterval = (_levelManager != null) ? _levelManager.GetCurrentSpawnInterval() : 2f; // LevelManager가 없으면 기본값 사용

        _timer += Time.deltaTime;
        if (_timer >= currentSpawnInterval)
        {
            SpawnRandomObstacle(); // 장애물 생성 함수 호출
            _timer = 0f; // 타이머 리셋
        }
    }

    /// <summary>
    /// 등록된 장애물 프리팹 중 하나를 무작위로 선택하여 생성합니다.
    /// 이제 상단/하단 장애물을 구분하여 생성합니다.
    /// </summary>
    void SpawnRandomObstacle()
    {
        // 상단/하단 장애물 중 어떤 것을 생성할지 무작위로 결정
        bool spawnTopObstacle = Random.Range(0, 2) == 0; // 0이면 상단, 1이면 하단 (50% 확률)

        GameObject selectedPrefab = null;
        float randomY = 0f;

        if (spawnTopObstacle)
        {
            // 상단 장애물 생성 로직
            if (topObstaclePrefabs == null || topObstaclePrefabs.Length == 0)
            {
                Debug.LogWarning("Top Obstacle Prefabs 배열이 비어있습니다. 상단 장애물을 생성할 수 없습니다.");
                return;
            }
            selectedPrefab = topObstaclePrefabs[Random.Range(0, topObstaclePrefabs.Length)];
            randomY = topSpawnYPositions[Random.Range(0, topSpawnYPositions.Length)];
        }
        else
        {
            // 하단 장애물 생성 로직
            if (bottomObstaclePrefabs == null || bottomObstaclePrefabs.Length == 0)
            {
                Debug.LogWarning("Bottom Obstacle Prefabs 배열이 비어있습니다. 하단 장애물을 생성할 수 없습니다.");
                return;
            }
            selectedPrefab = bottomObstaclePrefabs[Random.Range(0, bottomObstaclePrefabs.Length)];//바텀옵스타클 프리팹에서 아무거나 뽑고 정의된
            randomY = bottomSpawnYPositions[Random.Range(0, bottomSpawnYPositions.Length)]; //높이에서 스폰
        }

        // 선택된 프리팹이 없다면 종료 안전장치
        if (selectedPrefab == null) return;

        // 생성 위치 결정
        Vector3 spawnPosition = new Vector3(spawnXPosition, randomY, 0);

        // 장애물 생성
        GameObject newObstacle = Instantiate(selectedPrefab, spawnPosition, Quaternion.identity);

        // 생성된 장애물의 ObstacleController 초기화
        ObstacleController obsController = newObstacle.GetComponent<ObstacleController>();
        if (obsController != null)
        {
            // LevelManager로부터 현재 게임 속도를 가져와 ObstacleController에 전달
            float currentObstacleSpeed = (_levelManager != null) ? _levelManager.GetCurrentGameSpeed() : 5f; // 레벨 매니저가 없으면 기본 값 5f사용
            obsController.Init(currentObstacleSpeed); // 장애물 속도 초기화
        }
        else
        {
            Debug.LogWarning($"생성된 장애물 '{selectedPrefab.name}'에 ObstacleController가 없습니다. 확인해주세요.");
        }
    }
}