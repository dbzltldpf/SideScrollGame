using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    Transform trans;

    private void OnEnable()
    {
        trans = transform.parent.GetChild(1).GetComponent<Transform>();
    }
    private void Update()
    {
        transform.position = trans.position;
    }
}
