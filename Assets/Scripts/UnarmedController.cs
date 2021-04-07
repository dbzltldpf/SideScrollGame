using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnarmedController : MonoBehaviour
{
    public EnemyController enemy;
    public PlayerController player;

    private void Awake()
    {
        enemy = FindObjectOfType<EnemyController>();
        player = FindObjectOfType<PlayerController>();
    }
    //public void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.tag == "Enemy")
    //    {
    //        if (player.attacking)
    //        {
    //            Debug.Log("맨손");
    //            collision.GetComponent<EnemyController>().OnDamage(1);
    //        }
    //
    //    }
    //}
    //공격전 이미 trigger안에 들어와있을때
    public void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            if (player.attacking)
            {
                Debug.Log("맨손");
                collision.GetComponent<EnemyController>().OnDamage(1);
            }

        }
    }
}
