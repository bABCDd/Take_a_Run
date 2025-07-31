using UnityEngine;
using System.Collections; // IEnumerator 사용을 위해 필요할 수 있지만, 현재 코드에서는 직접 사용하지 않음

public class ObstacleSpawner : MonoBehaviour
{
    [Header("Obstacle Prefabs")]

    public GameObject[] topObstaclePrefabs;

    public GameObject[] bottomObstaclePrefabs;

    public GameObject[] deadzonePrefabs;

    [Header("Spawn Positions")]
    public float spawnXPosition = 12f; 

    
    public float[] topSpawnYPositions = { 2f };
   
    public float[] bottomSpawnYPositions = { -2f, -1.5f };
    
    public float[] deadzoneYPositions = {-3.5f };

    [Header("Spawn Probabilities")]
    [Range(0, 1)] public float topObstacleChance = 0.4f;    // 상단 장애물 스폰 확률 (40%)
    [Range(0, 1)] public float bottomObstacleChance = 0.4f; 
   

    private float _timer; 
    private LevelManager _levelManager;

    void Start()
    {
        _timer = 0f; 

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
            SpawnRandomObstacle(); 
            _timer = 0f;
        }
    }


    void SpawnRandomObstacle()
    {
        GameObject selectedPrefab = null;
        float randomY = 0f;

        
        float totalChance = topObstacleChance + bottomObstacleChance;
        float deadzoneChance = 1.0f - totalChance; // 구덩이 스폰 확률 (나머지)

        float randomValue = Random.value; // 0.0 ~ 1.0 사이의 랜덤 값

        if (randomValue < topObstacleChance)
        {
            // 상단 장애물 스폰 로직
            if (topObstaclePrefabs == null || topObstaclePrefabs.Length == 0)
            {
                Debug.LogWarning("Top Obstacle Prefabs 배열이 비어있습니다. 상단 장애물을 생성할 수 없습니다.");
                return;
            }
            selectedPrefab = topObstaclePrefabs[Random.Range(0, topObstaclePrefabs.Length)];
            randomY = topSpawnYPositions[Random.Range(0, topSpawnYPositions.Length)];
        }
        else if (randomValue < topObstacleChance + bottomObstacleChance)
        {
            // 하단 장애물 스폰 로직
            if (bottomObstaclePrefabs == null || bottomObstaclePrefabs.Length == 0)
            {
                Debug.LogWarning("Bottom Obstacle Prefabs 배열이 비어있습니다. 하단 장애물을 생성할 수 없습니다.");
                return;
            }
            selectedPrefab = bottomObstaclePrefabs[Random.Range(0, bottomObstaclePrefabs.Length)];
            randomY = bottomSpawnYPositions[Random.Range(0, bottomSpawnYPositions.Length)];
        }
        else 
        {
            // 구덩이 스폰 로직
            if (deadzonePrefabs == null || deadzonePrefabs.Length == 0)
            {
                Debug.LogWarning("Deadzone Prefabs 배열이 비어있습니다. 구덩이를 생성할 수 없습니다.");
                return;
            }
            selectedPrefab = deadzonePrefabs[Random.Range(0, deadzonePrefabs.Length)];
            randomY = deadzoneYPositions[Random.Range(0, deadzoneYPositions.Length)];
        }

        // 선택된 프리팹이 없다면 종료 안전장치
        if (selectedPrefab == null) return;

       
        Vector3 spawnPosition = new Vector3(spawnXPosition, randomY, 0);

        
        GameObject newObject = Instantiate(selectedPrefab, spawnPosition, Quaternion.identity);

        
        ObstacleController obsController = newObject.GetComponent<ObstacleController>();
        if (obsController != null)
        {
            
            float currentObstacleSpeed = (_levelManager != null) ? _levelManager.GetCurrentGameSpeed() : 5f;
            obsController.Init(currentObstacleSpeed); // 장애물 속도 초기화
        }
        else
        {
            DeadzoneController deadzoneController = newObject.GetComponent<DeadzoneController>();
            if (deadzoneController != null)
            {
                float currentDeadzoneSpeed = (_levelManager != null) ? _levelManager.GetCurrentGameSpeed() : 5f;
                deadzoneController.Init(currentDeadzoneSpeed);
            }
        }
    }
}