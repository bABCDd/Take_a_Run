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
        // 플레이어가 아이템과 충돌했을 때의 처리
        // catFood;water;tunaCan;egg;는 플레이어가 먹었을 경우 라이프가 회복
        // ratPoison;antifreeze;chocolate;bone;는 플레이어가 먹었을 경우 라이프가 감소
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
