using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    public Slider progressbar; //슬라이더 선언
    public float maxvalue; //최대범위
    private void Update()
    {
        progressbar.maxValue = maxvalue;
        progressbar.value += Time.deltaTime * 5;
    }
}
