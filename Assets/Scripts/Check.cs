using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Check : MonoBehaviour
{
    private EnemyController enemy;

    private void Awake()
    {
        enemy = GetComponentInParent<EnemyController>();
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case "Player":
                enemy.target = collision.transform;
                Debug.Log(enemy.target.name);
                break;
        }
    }
    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            enemy.target = null;
            Debug.Log("target : null");
        }

    }
}
