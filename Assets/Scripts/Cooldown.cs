using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Cooldown : MonoBehaviour
{
    public Text coolTimeText;

    public Image imageCooldown;

    public GameObject coolTimePanel;
    public PlayerController player;

    public float cooldown;

    public float cooldownTime;

    private float saveCooldownTime;

    public bool isCooldown;
    [SerializeField]private bool bFlag = true;

    private void Start()
    {
        saveCooldownTime = cooldownTime;

    }
    private void Update()
    {
        if (bFlag)
        {
            Initialize();

            bFlag = false;
        }
        if (isCooldown)
        {
            this.GetComponent<Button>().interactable = false;
            imageCooldown.fillAmount += 1 / cooldown * Time.deltaTime;

            if (imageCooldown.fillAmount == 1)
            {
                imageCooldown.fillAmount = 0;
                cooldownTime = saveCooldownTime;
                isCooldown = false; //isCooldown ^= false; 안됨
                bFlag = true;
                coolTimePanel.SetActive(false);
                this.enabled = false;
                this.GetComponent<Button>().interactable = true;

            }
        }

    }

    private void FixedUpdate()
    {
        if (isCooldown)
        {
            cooldownTime -= Time.deltaTime;
            int cooldownInt = (int)cooldownTime + 1;

            coolTimeText.text = cooldownInt.ToString();
        }

        //float skillMp = (float)(Math.Truncate(0.985 * 10) / 10);
        //첫째자리 빼고 다 잘라서 0.9가 됨
        //이걸 다시 string.Format("{0:0,#}",skillMp)하면 문제없이 표기
    }
    private void Initialize()
    {
        isCooldown = true;
        coolTimePanel.SetActive(true);

    }
    public bool GetFlag()
    {
        return isCooldown;
    }
}
