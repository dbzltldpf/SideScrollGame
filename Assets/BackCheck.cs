using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackCheck : MonoBehaviour
{
    [SerializeField]private EnemyController enemy;

    private void Awake()
    {
        enemy = GetComponentInParent<EnemyController>();
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case "bullet":
                enemy.backAttack = true;
                break;
        }
    }
    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("bullet"))
        {
            enemy.backAttack = false;
        }

    }
}
