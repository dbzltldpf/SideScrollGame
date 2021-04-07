using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamageText : MonoBehaviour
{
    private float moveSpeed;
    private float alphaSpeed;
    private float destroyTime;

    public int damage;

    TextMeshProUGUI meshText;
    Color alpha;


    private void Awake()
    {
        meshText = GetComponent<TextMeshProUGUI>();
        alpha = meshText.color;
    }
    private void Start()
    {
        moveSpeed = 2.0f;
        alphaSpeed = 2.0f;
        destroyTime = 2.0f;
        meshText.text = damage.ToString();

        Invoke("DestroyObject", destroyTime);
    }

    private void FixedUpdate()
    {
        transform.Translate(new Vector3(0, moveSpeed * Time.deltaTime, 0));
        alpha.a = Mathf.Lerp(alpha.a, 0, Time.deltaTime * alphaSpeed);
        meshText.color = alpha;
    }
    public void DestroyObject()
    {
        Destroy(gameObject.transform.parent.gameObject);
    }
}
