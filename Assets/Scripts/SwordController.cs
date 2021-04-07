using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordController : MonoBehaviour
{
    public PlayerController player;
    private Collider2D mCollider;
    bool bHit;
    private void Awake()
    {
        player = FindObjectOfType<PlayerController>();
        mCollider = GetComponent<Collider2D>();
        bHit = false;
    }
    private void LateUpdate()
    {
        if (bHit == true)
        {
            Invoke("BhitController", 0.5f);
        }
    }

    public void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            if (player.attacking && !bHit)
            {
                Debug.Log("검");
                collision.GetComponent<EnemyController>().OnDamage(2);
                bHit = true;

            }
        }
    }
    void BhitController()
    {
        bHit = false;
    }
}
