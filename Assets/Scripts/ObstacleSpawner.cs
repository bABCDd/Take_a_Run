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

    public float[] topSpawnYPositions = { 2f }; // ��� ���� ��ġ (�����۵� ���� ����)
    public float[] bottomSpawnYPositions = { -2f, -1.5f }; // �ϴ� ���� ��ġ (�����۵� ���� ����)
    public float[] deadzoneYPositions = { -3.5f };

    [Header("Spawn Probabilities")]
    [Range(0, 1)] public float topObstacleChance = 0.3f;   // ��� ��ֹ� ���� Ȯ��
    [Range(0, 1)] public float bottomObstacleChance = 0.3f; // �ϴ� ��ֹ� ���� Ȯ��
    [Range(0, 1)] public float itemChance = 0.2f; // ������ ���� Ȯ��
    // Deadzone Chance�� 1.0 - (�� �� ���� Ȯ�� �հ�)�� �ڵ� ����

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

        // ��� ���� Ȯ���� ������ ����մϴ�. �� ������ 1.0�� ������ �� �˴ϴ�.
        // ���� ���, top:0.3, bottom:0.3, item:0.2 �̸� �հ� 0.8. Deadzone�� 0.2�� �˴ϴ�.
        float totalDefinedChance = topObstacleChance + bottomObstacleChance + itemChance;

        // ���� ������ 1.0�� �ʰ��ϸ� ��� ���� ������ 1.0���� �����մϴ�.
        if (totalDefinedChance > 1.0f)
        {
            Debug.LogWarning("��ֹ�/������ ���� Ȯ���� ���� 1.0�� �ʰ��߽��ϴ�. ������ 1.0���� �����մϴ�.");
            totalDefinedChance = 1.0f;
        }

        float deadzoneChance = 1.0f - totalDefinedChance; // ������ ���� Ȯ���� ������

        float randomValue = Random.value; // 0.0 ~ 1.0 ������ ���� ��

        // ���������� Ȯ�� ������ �˻��Ͽ� �� �ϳ��� ������Ʈ�� �����մϴ�.
        if (randomValue < topObstacleChance)
        {
            // ��� ��ֹ� ����
            if (topObstaclePrefabs == null || topObstaclePrefabs.Length == 0) { Debug.LogWarning("Top Obstacle Prefabs �迭�� ����ֽ��ϴ�."); return; }
            selectedPrefab = topObstaclePrefabs[Random.Range(0, topObstaclePrefabs.Length)];
            randomY = topSpawnYPositions[Random.Range(0, topSpawnYPositions.Length)];
        }
        else if (randomValue < topObstacleChance + bottomObstacleChance)
        {
            // �ϴ� ��ֹ� ����
            if (bottomObstaclePrefabs == null || bottomObstaclePrefabs.Length == 0) { Debug.LogWarning("Bottom Obstacle Prefabs �迭�� ����ֽ��ϴ�."); return; }
            selectedPrefab = bottomObstaclePrefabs[Random.Range(0, bottomObstaclePrefabs.Length)];
            randomY = bottomSpawnYPositions[Random.Range(0, bottomSpawnYPositions.Length)];
        }
        else if (randomValue < topObstacleChance + bottomObstacleChance + itemChance)
        {
            // ������ ����
            if (itemPrefabs == null || itemPrefabs.Length == 0) { Debug.LogWarning("Item Prefabs �迭�� ����ֽ��ϴ�."); return; }
            selectedPrefab = itemPrefabs[Random.Range(0, itemPrefabs.Length)];

            // ������ Y ��ġ�� ���/�ϴ� ��ֹ� Y ��ġ �� �������� ����
            // (��ȹ �ǵ��� ���� �������� ��ֹ� ��ο� ��������)
            if (Random.value < 0.5f && topSpawnYPositions.Length > 0)
            {
                randomY = topSpawnYPositions[Random.Range(0, topSpawnYPositions.Length)];
            }
            else if (bottomSpawnYPositions.Length > 0)
            {
                randomY = bottomSpawnYPositions[Random.Range(0, bottomSpawnYPositions.Length)];
            }
            else if (topSpawnYPositions.Length > 0) // �ϴ��� ���ٸ� ����̶� ���
            {
                randomY = topSpawnYPositions[Random.Range(0, topSpawnYPositions.Length)];
            }
            else
            {
                Debug.LogWarning("�������� ������ ��ȿ�� Y ��ġ �迭�� �����ϴ�."); return;
            }
        }
        else // ���� � Ȯ������ �ش���� ������ ������ ���� (������ Ȯ��)
        {
            // ������ ����
            if (deadzonePrefabs == null || deadzonePrefabs.Length == 0) { Debug.LogWarning("Deadzone Prefabs �迭�� ����ֽ��ϴ�."); return; }
            selectedPrefab = deadzonePrefabs[Random.Range(0, deadzonePrefabs.Length)];
            randomY = deadzoneYPositions[Random.Range(0, deadzoneYPositions.Length)];
        }

        // ���õ� �������� ���ٸ� ���� (���� ����)
        if (selectedPrefab == null) return;

        Vector3 spawnPosition = new Vector3(spawnXPosition, randomY, 0);
        GameObject newObject = Instantiate(selectedPrefab, spawnPosition, Quaternion.identity);

        // ������ ������Ʈ�� ���� Init �޼��� ȣ��
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