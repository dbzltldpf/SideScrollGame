using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class WeaponBoard : MonoBehaviour
{
    public JsonManager jsonManager;
    public Equipment equipment;
    public GameObject[] currentUi;
    public GameObject[] skillExpBar;
    public Text[] weaponLevelText;
    public Text[] weaponMainLvText;
    public int TotalLv;
    //weaponList의 렙 더한값
    [SerializeField] private int sum;
    //이때까지 장착했던 무기 아이템 데이터 기록
    [SerializeField] private List<ItemData> weaponList;

    private void Awake()
    {
        weaponList = new List<ItemData>();
    }
    private void Start()
    {
    }
    public void OnEnable()
    {
        for (int i = 0; i < equipment.mountedItemdata.Count; i++)
        {
            if (!weaponList.Contains(equipment.mountedItemdata[i]))
            {
                weaponList.Add(equipment.mountedItemdata[i]);
            }
        }
        WeaponLevelText();
    }
    public void OnDisable()
    {
        //weaponList.Clear();
    }
    private void Update()
    {
    }
    private void WeaponLevelText()
    {
        sum = 0;

        if (weaponList.Count > 0)
        {
            for (int i = 0; i < weaponList.Count; i++)
            {
                weaponMainLvText[0].text = ($"{sum += weaponList[i].Level}/ 300");

                for (int j = 0; j < weaponLevelText.Length; j++)
                {

                    if (weaponLevelText[j].name == weaponList[i].ID.ToString())
                    {
                        weaponLevelText[j].text = ($"{weaponList[i].Level}/100");
                    }
                }
            }
        }
    }
    private void SkillExpBar()
    {
        for (int i = 0; i < skillExpBar.Length; i++)
        {
            for (int j = 0; j < equipment.mountedItemdata.Count; j++)
            {
                if (skillExpBar[i].name == equipment.mountedItemdata[j].ID.ToString())
                {

                    skillExpBar[i].GetComponent<Image>().fillAmount = (float)equipment.mountedItemdata[j].Exp / (float)equipment.mountedItemdata[j].nextLevelExp;
                    if (equipment.mountedItemdata[j].Exp >= equipment.mountedItemdata[j].nextLevelExp)
                    {
                        skillExpBar[i].GetComponent<Image>().fillAmount = 0;

                    }
                }
            }
        }
    }
    //지금은 버튼에 붙어있지만 나중에는 몹을 죽였을때 올라가게
    public void ExpUp()
    {
        if (equipment.mountedItemdata.Count > 0)
        {
            for (int i = 0; i < skillExpBar.Length; i++)
            {
                for (int j = 0; j < equipment.mountedItemdata.Count; j++)
                {
                    if (skillExpBar[i].transform.parent.parent.name == equipment.mountedItemdata[j].ID.ToString())
                    {
                        equipment.mountedItemdata[j].Exp += 1;
                        SkillExpBar();
                        LevelUp();

                    }
                }
            }

        }
        for (int i = 0; i < equipment.mountedItemdata.Count; i++)
        {
            switch (equipment.mountedItemdata[i].Level)
            {
                case 50:
                    currentUi[1].SetActive(true);
                    currentUi[0].SetActive(false);
                    break;
                case 80:
                    currentUi[2].SetActive(true);
                    currentUi[1].SetActive(false);
                    break;
                case 100:
                    currentUi[3].SetActive(true);
                    currentUi[2].SetActive(false);
                    break;
                default:
                    break;
            }
        }

    }
    public void LevelUp()
    {
        for (int i = 0; i < equipment.mountedItemdata.Count; i++)
        {
            if (equipment.mountedItemdata[i].Exp >= equipment.mountedItemdata[i].nextLevelExp)
            {
                equipment.mountedItemdata[i].Level++;
                equipment.mountedItemdata[i].Exp = 0;
                equipment.mountedItemdata[i].nextLevelExp++;
                //switch (weaponList[i].Level)
                //{
                //    case 1:
                //        //nextLevelUpExp ?
                //        //레벨이 1이 되면 렙업을 하기 위한 경험치량을 5에서 6으로 늘려줘요
                //        nextLevelUpExp = 6;
                //        break;
                //    case 2:
                //        nextLevelUpExp = 7;
                //        break;
                //    case 3:
                //        break;
                //    case 4:
                //        break;
                //    case 5:
                //        break;
                //    default:
                //        break;
                //}
            }
        }
    }

}
