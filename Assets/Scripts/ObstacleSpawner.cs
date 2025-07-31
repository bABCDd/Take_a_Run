using UnityEngine;
using System.Collections;

public class ObstacleSpawner : MonoBehaviour
{
    [Header("Obstacle Prefabs")]
    public GameObject[] topObstaclePrefabs;
    public GameObject[] bottomObstaclePrefabs;

    [Header("Deadzone Prefabs")]
    public GameObject[] deadzonePrefabs;

    [Header("Item Prefabs")]
    public GameObject[] itemPrefabs;

    [Header("Spawn Positions")]
    public float spawnXPosition = 12f;

    public float[] topSpawnYPositions = { 2f }; // 상단 스폰 위치 (아이템도 공유 가능)
    public float[] bottomSpawnYPositions = { -2f, -1.5f }; // 하단 스폰 위치 (아이템도 공유 가능)
    public float[] deadzoneYPositions = { -3.5f };

    [Header("Spawn Probabilities")]
    [Range(0, 1)] public float topObstacleChance = 0.3f;   // 상단 장애물 스폰 확률
    [Range(0, 1)] public float bottomObstacleChance = 0.3f; // 하단 장애물 스폰 확률
    [Range(0, 1)] public float itemChance = 0.2f; // 아이템 스폰 확률
    // Deadzone Chance는 1.0 - (위 세 가지 확률 합계)로 자동 계산됨

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
        if (GameManager.instance == null || GameManager.instance.gameState != GameState.Playing) return;

        float currentSpawnInterval = (_levelManager != null) ? _levelManager.GetCurrentSpawnInterval() : 2f;

        _timer += Time.deltaTime;
        if (_timer >= currentSpawnInterval)
        {
            SpawnRandomObject();
            _timer = 0f;
        }
    }

    void SpawnRandomObject()
    {
        GameObject selectedPrefab = null;
        float randomY = 0f;

        // 모든 스폰 확률의 총합을 계산합니다. 이 총합이 1.0을 넘으면 안 됩니다.
        // 예를 들어, top:0.3, bottom:0.3, item:0.2 이면 합계 0.8. Deadzone은 0.2가 됩니다.
        float totalDefinedChance = topObstacleChance + bottomObstacleChance + itemChance;

        // 만약 총합이 1.0을 초과하면 경고를 띄우고 총합을 1.0으로 강제합니다.
        if (totalDefinedChance > 1.0f)
        {
            Debug.LogWarning("장애물/아이템 스폰 확률의 합이 1.0을 초과했습니다. 총합을 1.0으로 조정합니다.");
            totalDefinedChance = 1.0f;
        }

        float deadzoneChance = 1.0f - totalDefinedChance; // 구덩이 스폰 확률은 나머지

        float randomValue = Random.value; // 0.0 ~ 1.0 사이의 랜덤 값

        // 순차적으로 확률 구간을 검사하여 단 하나의 오브젝트만 스폰합니다.
        if (randomValue < topObstacleChance)
        {
            // 상단 장애물 스폰
            if (topObstaclePrefabs == null || topObstaclePrefabs.Length == 0) { Debug.LogWarning("Top Obstacle Prefabs 배열이 비어있습니다."); return; }
            selectedPrefab = topObstaclePrefabs[Random.Range(0, topObstaclePrefabs.Length)];
            randomY = topSpawnYPositions[Random.Range(0, topSpawnYPositions.Length)];
        }
        else if (randomValue < topObstacleChance + bottomObstacleChance)
        {
            // 하단 장애물 스폰
            if (bottomObstaclePrefabs == null || bottomObstaclePrefabs.Length == 0) { Debug.LogWarning("Bottom Obstacle Prefabs 배열이 비어있습니다."); return; }
            selectedPrefab = bottomObstaclePrefabs[Random.Range(0, bottomObstaclePrefabs.Length)];
            randomY = bottomSpawnYPositions[Random.Range(0, bottomSpawnYPositions.Length)];
        }
        else if (randomValue < topObstacleChance + bottomObstacleChance + itemChance)
        {
            // 아이템 스폰
            if (itemPrefabs == null || itemPrefabs.Length == 0) { Debug.LogWarning("Item Prefabs 배열이 비어있습니다."); return; }
            selectedPrefab = itemPrefabs[Random.Range(0, itemPrefabs.Length)];

            // 아이템 Y 위치는 상단/하단 장애물 Y 위치 중 랜덤으로 선택
            // (기획 의도에 따라 아이템이 장애물 경로에 나오도록)
            if (Random.value < 0.5f && topSpawnYPositions.Length > 0)
            {
                randomY = topSpawnYPositions[Random.Range(0, topSpawnYPositions.Length)];
            }
            else if (bottomSpawnYPositions.Length > 0)
            {
                randomY = bottomSpawnYPositions[Random.Range(0, bottomSpawnYPositions.Length)];
            }
            else if (topSpawnYPositions.Length > 0) // 하단이 없다면 상단이라도 사용
            {
                randomY = topSpawnYPositions[Random.Range(0, topSpawnYPositions.Length)];
            }
            else
            {
                Debug.LogWarning("아이템을 스폰할 유효한 Y 위치 배열이 없습니다."); return;
            }
        }
        else // 위의 어떤 확률에도 해당되지 않으면 구덩이 스폰 (나머지 확률)
        {
            // 구덩이 스폰
            if (deadzonePrefabs == null || deadzonePrefabs.Length == 0) { Debug.LogWarning("Deadzone Prefabs 배열이 비어있습니다."); return; }
            selectedPrefab = deadzonePrefabs[Random.Range(0, deadzonePrefabs.Length)];
            randomY = deadzoneYPositions[Random.Range(0, deadzoneYPositions.Length)];
        }

        // 선택된 프리팹이 없다면 종료 (에러 방지)
        if (selectedPrefab == null) return;

        Vector3 spawnPosition = new Vector3(spawnXPosition, randomY, 0);
        GameObject newObject = Instantiate(selectedPrefab, spawnPosition, Quaternion.identity);

        // 생성된 오브젝트에 따라 Init 메서드 호출
        float currentSpeed = (_levelManager != null) ? _levelManager.GetCurrentGameSpeed() : 5f;

        ObstacleController obsController = newObject.GetComponent<ObstacleController>();
        if (obsController != null)
        {
            obsController.Init(currentSpeed);
        }
        else
        {
            DeadzoneController deadzoneController = newObject.GetComponent<DeadzoneController>();
            if (deadzoneController != null)
            {
                deadzoneController.Init(currentSpeed);
            }
            else
            {
                ItemController itemController = newObject.GetComponent<ItemController>();
                if (itemController != null)
                {
                    itemController.Init(currentSpeed);
                }
            }
        }
    }
}