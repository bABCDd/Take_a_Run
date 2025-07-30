using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BgLooper : MonoBehaviour
{
    public int numBgCoont = 5;

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
    }
}
