using UnityEngine;
using System.Collections; 

public class ObstacleSpawner : MonoBehaviour
{
    [Header("Obstacle Prefabs")]
    public GameObject[] obstaclePrefabs; 

    [Header("Spawn Positions")]
    public float spawnXPosition = 12f; // ��ֹ��� ������ X ��ǥ (ȭ�� ���� ��)
    public float[] spawnYPositions = { -2f, 0f, 2f }; // ��ֹ��� ��Ÿ�� �� �ִ� Y ��ǥ ��� (�ٴ�, ����)

    private float _timer; // ���� ��ֹ� ������ ���� Ÿ�̸�
    private LevelManager _levelManager; 

    void Start()
    {
        _timer = 0f; // Ÿ�̸� �ʱ�ȭ
 
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
            SpawnRandomObstacle(); // ��ֹ� ���� �Լ� ȣ��
            _timer = 0f; // Ÿ�̸� ����
        }
    }

    void SpawnRandomObstacle()
    {
        if (obstaclePrefabs == null || obstaclePrefabs.Length == 0)
        {
            Debug.LogWarning("Obstacle Prefabs �迭�� �Ҵ���� �ʾҰų� ����ֽ��ϴ�. ��ֹ��� ������ �� �����ϴ�.");
            return;
        }

        // 1. ���� ��ֹ� ������ ����
        GameObject selectedPrefab = obstaclePrefabs[Random.Range(0, obstaclePrefabs.Length)];

        // 2. ���� Y ��ġ ����
        float randomY = spawnYPositions[Random.Range(0, spawnYPositions.Length)];

        // 3. ���� ��ġ ����
        Vector3 spawnPosition = new Vector3(spawnXPosition, randomY, 0);

        // 4. ��ֹ� ���� (������Ʈ Ǯ���� ����ϸ� Instantiate ��� Ǯ���� ������)
        GameObject newObstacle = Instantiate(selectedPrefab, spawnPosition, Quaternion.identity);

        // 5. ������ ��ֹ��� ObstacleController �ʱ�ȭ
        ObstacleController obsController = newObstacle.GetComponent<ObstacleController>();
        if (obsController != null)
        {
            
            float currentObstacleSpeed = (_levelManager != null) ? _levelManager.GetCurrentGameSpeed() : 5f; // LevelManager�� ������ �⺻�� ���
            obsController.Init(currentObstacleSpeed); // ��ֹ� �ӵ��� �ʱ�ȭ�ϴ� Init �Լ� ȣ��
        }
        else
        {
            Debug.LogWarning($"������ ��ֹ� '{selectedPrefab.name}'�� ObstacleController�� �����ϴ�. Ȯ�����ּ���.");
        }
    }
}