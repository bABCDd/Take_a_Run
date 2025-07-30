using UnityEngine;
using System.Collections;

public class ObstacleSpawner : MonoBehaviour
{
    [Header("Obstacle Prefabs")]
    // ������ �����Ǵ� ��ֹ� �����յ� (�����̵�� ����)
    public GameObject[] topObstaclePrefabs;
    // �Ʒ��� �����Ǵ� ��ֹ� �����յ� (������ ����)
    public GameObject[] bottomObstaclePrefabs;

    [Header("Spawn Positions")]
    public float spawnXPosition = 12f; // ��ֹ��� ������ X ��ǥ (ȭ�� ���� ��)

    // ������ �����Ǵ� ��ֹ��� Y ��ǥ (���� �÷��̾� ���� �ִ� ���� ��ó)
    public float[] topSpawnYPositions = { 2f }; // ����: �÷��̾ �����̵��� �� �ִ� ����
    // �Ʒ��� �����Ǵ� ��ֹ��� Y ��ǥ (���� �÷��̾� �� �Ʒ�)
    public float[] bottomSpawnYPositions = { -2f, -1.5f }; // ����: �÷��̾ ������ �� �ִ� ����

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
        // LevelManager�κ��� ���� ���� ���� ������ �����ɴϴ�.
        float currentSpawnInterval = (_levelManager != null) ? _levelManager.GetCurrentSpawnInterval() : 2f; // LevelManager�� ������ �⺻�� ���

        _timer += Time.deltaTime;
        if (_timer >= currentSpawnInterval)
        {
            SpawnRandomObstacle(); // ��ֹ� ���� �Լ� ȣ��
            _timer = 0f; // Ÿ�̸� ����
        }
    }

    /// <summary>
    /// ��ϵ� ��ֹ� ������ �� �ϳ��� �������� �����Ͽ� �����մϴ�.
    /// ���� ���/�ϴ� ��ֹ��� �����Ͽ� �����մϴ�.
    /// </summary>
    void SpawnRandomObstacle()
    {
        // ���/�ϴ� ��ֹ� �� � ���� �������� �������� ����
        bool spawnTopObstacle = Random.Range(0, 2) == 0; // 0�̸� ���, 1�̸� �ϴ� (50% Ȯ��)

        GameObject selectedPrefab = null;
        float randomY = 0f;

        if (spawnTopObstacle)
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
        else
        {
            // �ϴ� ��ֹ� ���� ����
            if (bottomObstaclePrefabs == null || bottomObstaclePrefabs.Length == 0)
            {
                Debug.LogWarning("Bottom Obstacle Prefabs �迭�� ����ֽ��ϴ�. �ϴ� ��ֹ��� ������ �� �����ϴ�.");
                return;
            }
            selectedPrefab = bottomObstaclePrefabs[Random.Range(0, bottomObstaclePrefabs.Length)];//���ҿɽ�ŸŬ �����տ��� �ƹ��ų� �̰� ���ǵ�
            randomY = bottomSpawnYPositions[Random.Range(0, bottomSpawnYPositions.Length)]; //���̿��� ����
        }

        // ���õ� �������� ���ٸ� ���� ������ġ
        if (selectedPrefab == null) return;

        // ���� ��ġ ����
        Vector3 spawnPosition = new Vector3(spawnXPosition, randomY, 0);

        // ��ֹ� ����
        GameObject newObstacle = Instantiate(selectedPrefab, spawnPosition, Quaternion.identity);

        // ������ ��ֹ��� ObstacleController �ʱ�ȭ
        ObstacleController obsController = newObstacle.GetComponent<ObstacleController>();
        if (obsController != null)
        {
            // LevelManager�κ��� ���� ���� �ӵ��� ������ ObstacleController�� ����
            float currentObstacleSpeed = (_levelManager != null) ? _levelManager.GetCurrentGameSpeed() : 5f; // ���� �Ŵ����� ������ �⺻ �� 5f���
            obsController.Init(currentObstacleSpeed); // ��ֹ� �ӵ� �ʱ�ȭ
        }
        else
        {
            Debug.LogWarning($"������ ��ֹ� '{selectedPrefab.name}'�� ObstacleController�� �����ϴ�. Ȯ�����ּ���.");
        }
    }
}