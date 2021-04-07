using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    public Slider progressbar; //�����̴� ����
    public float maxvalue; //�ִ����
    private void Update()
    {
        progressbar.maxValue = maxvalue;
        progressbar.value += Time.deltaTime * 5;
    }
}
