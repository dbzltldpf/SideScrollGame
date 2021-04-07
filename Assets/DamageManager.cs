using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageManager : MonoBehaviour
{
    public GameObject damageTextObj;

    public void Create(Vector3 pos, int damage)
    {
        GameObject damageText = Instantiate(damageTextObj, pos + new Vector3(0, 1, 0), Quaternion.identity, transform);
        damageText.GetComponentInChildren<DamageText>().damage = damage;
    }
}
