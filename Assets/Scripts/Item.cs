using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public GameObject item;


    public ObstacleController obstacleController;
    public Player player;


    public void OnTriggerEnter2D(Collider2D collision)
    {
        // �÷��̾ �����۰� �浹���� ���� ó��
        // catFood;water;tunaCan;egg;�� �÷��̾ �Ծ��� ��� �������� ȸ��
        // ratPoison;antifreeze;chocolate;bone;�� �÷��̾ �Ծ��� ��� �������� ����
        //HealItem
        //TakeDamageItem
        if (collision.gameObject.CompareTag("Player"))
        {
            if (gameObject.CompareTag("HealItem"))
            {
                player.Heal(1);
                Destroy(gameObject);
            }
            else if (gameObject.CompareTag("TakeDamgeItem"))
            {
                player.TakeDamage(1);
                Destroy(gameObject);
            }
        }
    }
}
