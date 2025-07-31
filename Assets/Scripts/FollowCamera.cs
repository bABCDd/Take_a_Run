using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    //플레이어 지정
    public Transform target;

    //카메라의 부드러움 정도 조정
    public float smoothSpeed = 0.125f;
    //오프셋 지정
    public Vector3 offset;

    //플레이어 이동후 계산되기 위해 LateUpdate지정
    private void LateUpdate()
    {
        if (target == null)
        {
            Debug.LogError("Not Found target");
            return;
        }
            

        //목표 위치 계산
        Vector3 desiredPosition = target.position + offset;
        //현재 위치와 목표 위치를 보간시킴(부드럽게 이동)
        Vector3 smoothPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        //카메라 위치를 계산된 위치로 설정함
        transform.position = smoothPosition;
    }
}
