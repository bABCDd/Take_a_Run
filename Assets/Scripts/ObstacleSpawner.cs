using UnityEngine;
using System.Collections; 

public class ObstacleSpawner : MonoBehaviour
{
    [Header("Obstacle Prefabs")]
    public GameObject[] obstaclePrefabs; 

    [Header("Spawn Positions")]
    public float spawnXPosition = 12f; // 장애물이 생성될 X 좌표 (화면 우측 밖)
    public float[] spawnYPositions = { -2f, 0f, 2f }; // 장애물이 나타날 수 있는 Y 좌표 목록 (바닥, 점프)

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
        float currentSpawnInterval = (_levelManager != null) ? _levelManager.GetCurrentSpawnInterval() : 2f; // LevelManager가 없으면 기본값 사용

        _timer += Time.deltaTime;
        if (_timer >= currentSpawnInterval)
        {
            SpawnRandomObstacle(); // 장애물 생성 함수 호출
            _timer = 0f; // 타이머 리셋
        }
    }

    void SpawnRandomObstacle()
    {
        if (obstaclePrefabs == null || obstaclePrefabs.Length == 0)
        {
            Debug.LogWarning("Obstacle Prefabs 배열이 할당되지 않았거나 비어있습니다. 장애물을 생성할 수 없습니다.");
            return;
        }

        // 1. 랜덤 장애물 프리팹 선택
        GameObject selectedPrefab = obstaclePrefabs[Random.Range(0, obstaclePrefabs.Length)];

        // 2. 랜덤 Y 위치 선택
        float randomY = spawnYPositions[Random.Range(0, spawnYPositions.Length)];

        // 3. 생성 위치 결정
        Vector3 spawnPosition = new Vector3(spawnXPosition, randomY, 0);

        // 4. 장애물 생성 (오브젝트 풀링을 사용하면 Instantiate 대신 풀에서 가져옴)
        GameObject newObstacle = Instantiate(selectedPrefab, spawnPosition, Quaternion.identity);

        // 5. 생성된 장애물의 ObstacleController 초기화
        ObstacleController obsController = newObstacle.GetComponent<ObstacleController>();
        if (obsController != null)
        {
            
            float currentObstacleSpeed = (_levelManager != null) ? _levelManager.GetCurrentGameSpeed() : 5f; // LevelManager가 없으면 기본값 사용
            obsController.Init(currentObstacleSpeed); // 장애물 속도를 초기화하는 Init 함수 호출
        }
        else
        {
            Debug.LogWarning($"생성된 장애물 '{selectedPrefab.name}'에 ObstacleController가 없습니다. 확인해주세요.");
        }
    }
}