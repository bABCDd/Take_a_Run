using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BgLooper : MonoBehaviour
{
    public int numBgCoont = 5;

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
    }
}
