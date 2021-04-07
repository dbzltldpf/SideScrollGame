using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [SerializeField] private GameObject[] weapons;
    private void Awake()
    {
        weapons = new GameObject[transform.childCount];

        for (int i = 0; i < weapons.Length; i++)
        {
            weapons[i] = transform.GetChild(i).gameObject;
        }

    }
    public void WeaponObjActive(Item item)
    {
        for (int i = 0; i < weapons.Length; i++)
        {
            weapons[i].SetActive(false);

            if (weapons[i].name == item.itemdata.ID.ToString())
            {
                weapons[i].SetActive(true);
            }
        }
    }
    //장착 해제 했을때 끼고있는 무기 이미지 비활성화 시키기
    public void WeaponObjActiveDisable(Item item)
    {
        for (int i = 0; i < weapons.Length; i++)
        {
            if (weapons[i].name == "unarmed")
            {
                weapons[i].SetActive(true);
            }

            if (weapons[i].name == item.itemdata.ID.ToString())
            {
                weapons[i].SetActive(false);
            }
        }
    }


}
