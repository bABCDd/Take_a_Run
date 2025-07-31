using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    //�÷��̾� ����
    public Transform target;

    //ī�޶��� �ε巯�� ���� ����
    public float smoothSpeed = 0.125f;
    //������ ����
    public Vector3 offset;

    //�÷��̾� �̵��� ���Ǳ� ���� LateUpdate����
    private void LateUpdate()
    {
        if (target == null)
        {
            Debug.LogError("Not Found target");
            return;
        }
            

        //��ǥ ��ġ ���
        Vector3 desiredPosition = target.position + offset;
        //���� ��ġ�� ��ǥ ��ġ�� ������Ŵ(�ε巴�� �̵�)
        Vector3 smoothPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        //ī�޶� ��ġ�� ���� ��ġ�� ������
        transform.position = smoothPosition;
    }
}
