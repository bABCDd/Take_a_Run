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
        //Obstacle 이라는  컴포넌트를 가진 모든 오브젝트를 찾아서 배열로 저장합니다.
        Obstacle[] obstacle = GameObject.FindObjectsOfType<Obstacle>();
        // 첫번째 장애물의 위치를 저장합니다. 위에 코드 주석처리 해도 정상작동하네?
        // 지금은 옵스타클 클래스가 없어서 오류가 발생함
        obstacleLastPosition = obstacle[0].transform.position;
        // 장애물의 개수를 저장합니다.
        obstacleCount = obstacle.Length;

        //장애물 위치를 랜덤하게 설정
        for (int i = 0; i < obstacleCount; i ++)
        {
            obstacleLastPosition = obstacle[i].SetRandomPlace(obstacleLastPosition, obstacleCount);
        }
    }


    //
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //충돌한 오브젝트가 배경이라면
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
