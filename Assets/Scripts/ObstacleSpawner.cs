using UnityEngine;
using System.Collections; // IEnumerator ����� ���� �ʿ��� �� ������, ���� �ڵ忡���� ���� ������� ����

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
    [Range(0, 1)] public float topObstacleChance = 0.4f;    // ��� ��ֹ� ���� Ȯ�� (40%)
    [Range(0, 1)] public float bottomObstacleChance = 0.4f; 
   

    private float _timer; 
    private LevelManager _levelManager;

    void Start()
    {
        _timer = 0f; 

        _levelManager = LevelManager.Instance;
        if (_levelManager == null)
        {
            Debug.LogError("LevelManager �ν��Ͻ��� ã�� �� �����ϴ�! ���� LevelManager ������Ʈ�� �ִ��� Ȯ���ϼ���.");
        }
    }

    void Update()
    {
        
        float currentSpawnInterval = (_levelManager != null) ? _levelManager.GetCurrentSpawnInterval() : 2f; // LevelManager�� ������ �⺻�� ���

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
        float deadzoneChance = 1.0f - totalChance; // ������ ���� Ȯ�� (������)

        float randomValue = Random.value; // 0.0 ~ 1.0 ������ ���� ��

        if (randomValue < topObstacleChance)
        {
            // ��� ��ֹ� ���� ����
            if (topObstaclePrefabs == null || topObstaclePrefabs.Length == 0)
            {
                Debug.LogWarning("Top Obstacle Prefabs �迭�� ����ֽ��ϴ�. ��� ��ֹ��� ������ �� �����ϴ�.");
                return;
            }
            selectedPrefab = topObstaclePrefabs[Random.Range(0, topObstaclePrefabs.Length)];
            randomY = topSpawnYPositions[Random.Range(0, topSpawnYPositions.Length)];
        }
        else if (randomValue < topObstacleChance + bottomObstacleChance)
        {
            // �ϴ� ��ֹ� ���� ����
            if (bottomObstaclePrefabs == null || bottomObstaclePrefabs.Length == 0)
            {
                Debug.LogWarning("Bottom Obstacle Prefabs �迭�� ����ֽ��ϴ�. �ϴ� ��ֹ��� ������ �� �����ϴ�.");
                return;
            }
            selectedPrefab = bottomObstaclePrefabs[Random.Range(0, bottomObstaclePrefabs.Length)];
            randomY = bottomSpawnYPositions[Random.Range(0, bottomSpawnYPositions.Length)];
        }
        else 
        {
            // ������ ���� ����
            if (deadzonePrefabs == null || deadzonePrefabs.Length == 0)
            {
                Debug.LogWarning("Deadzone Prefabs �迭�� ����ֽ��ϴ�. �����̸� ������ �� �����ϴ�.");
                return;
            }
            selectedPrefab = deadzonePrefabs[Random.Range(0, deadzonePrefabs.Length)];
            randomY = deadzoneYPositions[Random.Range(0, deadzoneYPositions.Length)];
        }

        // ���õ� �������� ���ٸ� ���� ������ġ
        if (selectedPrefab == null) return;

       
        Vector3 spawnPosition = new Vector3(spawnXPosition, randomY, 0);

        
        GameObject newObject = Instantiate(selectedPrefab, spawnPosition, Quaternion.identity);

        
        ObstacleController obsController = newObject.GetComponent<ObstacleController>();
        if (obsController != null)
        {
            
            float currentObstacleSpeed = (_levelManager != null) ? _levelManager.GetCurrentGameSpeed() : 5f;
            obsController.Init(currentObstacleSpeed); // ��ֹ� �ӵ� �ʱ�ȭ
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