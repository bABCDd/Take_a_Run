using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BgLooper : MonoBehaviour
{
    public int numBgCoont = 5;
    public int obstacleCount = 0;
    public Vector3 obstacleLastPosition = Vector3.zero;

    void Start()
    {
        //Obstacle �̶��  ������Ʈ�� ���� ��� ������Ʈ�� ã�Ƽ� �迭�� �����մϴ�.
        Obstacle[] obstacle = GameObject.FindObjectsOfType<Obstacle>();
        // ù��° ��ֹ��� ��ġ�� �����մϴ�. ���� �ڵ� �ּ�ó�� �ص� �����۵��ϳ�?
        // ������ �ɽ�ŸŬ Ŭ������ ��� ������ �߻���
        obstacleLastPosition = obstacle[0].transform.position;
        // ��ֹ��� ������ �����մϴ�.
        obstacleCount = obstacle.Length;

        //��ֹ� ��ġ�� �����ϰ� ����
        for (int i = 0; i < obstacleCount; i ++)
        {
            obstacleLastPosition = obstacle[i].SetRandomPlace(obstacleLastPosition, obstacleCount);
        }
    }


    //
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //�浹�� ������Ʈ�� ����̶��
        if(collision.CompareTag("background"))
        {
            float widthOfBgObject = ((BoxCollider2D)(collision)).size.x;
            Vector3 pos = collision.transform.position;

            pos.x = widthOfBgObject* numBgCoont;
            collision.transform.position = pos;
        }

        Obstacle obstacle = collision.GetComponent<Obstacle>();
        if(obstacle)
        {
            obstacleLastPosition = obstacle.SetRandomPlace(obstacleLastPosition, obstacleCount);
        }
    }
}
