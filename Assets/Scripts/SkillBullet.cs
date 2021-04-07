using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillBullet : Bullet
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            Debug.Log("arrow");
            enemy.OnDamage(4);
            transform.parent.gameObject.SetActive(false);
        }


    }
    private void OnDisable()
    {
        gameObject.SetActive(false);
    }
}
