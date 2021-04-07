using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Equipment : MonoBehaviour
{
    public Inventory inven;
    public GameManager gameManager;

    public Item[] selectEquipItem;
    public List<ItemData> mountedItemdata;
    public Transform[] currentEquipSlot;
    public GameObject dataUI;
    public Text dataUiText;
    public bool[] mounted; //장착여부
    public bool longDistanceWeapon;
    public int currentItemNum;
    public string weaponType;


    //selectEquipSlot[] = 0(머리),1(몸통),2(신발),3(반지),4(무기),5(보조무기),6(가방)
    private void Awake()
    {
        selectEquipItem = new Item[transform.childCount];
        mounted = new bool[transform.childCount];
        currentEquipSlot = new Transform[transform.childCount];
        mountedItemdata = new List<ItemData>();

        for (int i = 0; i < selectEquipItem.Length; i++)
        {
            selectEquipItem[i] = transform.GetChild(i).GetChild(0).GetComponent<Item>();
            currentEquipSlot[i] = transform.GetChild(i).GetChild(1).GetComponent<Transform>();
            mounted[i] = false;
        }

    }
    //장착한 장비 슬롯에 넣어주기
    public void SetEquipSlot(int num)
    {
        selectEquipItem[num].gameObject.SetActive(true);
        currentEquipSlot[num].gameObject.SetActive(false);
        selectEquipItem[num].itemdata = inven.useItem.itemdata;
        selectEquipItem[num].image.sprite = inven.useItem.image.sprite;

        mounted[num] = true;
        //장착한 아이템의 아이디 리스트에 담아주기
        mountedItemdata.Add(selectEquipItem[num].itemdata);

        switch (selectEquipItem[num].itemdata.ID)
        {
            case 20001:
                weaponType = "sword";
                break;
            case 20002:
                weaponType = "bow";
                break;
        }
    }
    public void ItemDataUi(int num)
    {
        string emptyEquip = "장착중인 장비가 없습니다.";
        if (mounted[num])
        {
            currentItemNum = num;
            dataUI.transform.position = new Vector3(selectEquipItem[num].transform.position.x+300,selectEquipItem[num].transform.position.y - 250,selectEquipItem[num].transform.position.z);

            dataUI.SetActive(true);
            dataUiText.text = selectEquipItem[num].itemdata.Name + ":" + "\n" + selectEquipItem[num].itemdata.Description;
        }
        else
        {
            gameManager.Notify(emptyEquip);
        }
    }
    //장착 해제
    public void ReleaseEquip()
    {
        int num = currentItemNum;

        ReleaseItemCreate(selectEquipItem[num]);
        //장착중인 아이템 이미지 비활성화 시켜주기
        inven.weaponManager.WeaponObjActiveDisable(selectEquipItem[num]);
        //장착해제한 아이템의 아이디를 리스트에서 삭제
        mountedItemdata.Remove(selectEquipItem[num].itemdata);

        weaponType = "unarmed";
        mounted[num] = false;
        selectEquipItem[num].itemdata = null;
        selectEquipItem[num].image.sprite = null;
        selectEquipItem[num].gameObject.SetActive(false);
        currentEquipSlot[num].gameObject.SetActive(true);
        longDistanceWeapon = false;

        inven.slotEquip.Remove(num);

        //inven.AllItemButton();
        inven.SaveInvenListButton();
    }
    //다른 장비를 끼웠을때 자동으로 해제되서 인벤으로 들어감
    public void AutoReleaseEquip(int num)
    {
        ReleaseItemCreate(selectEquipItem[num]);
        inven.weaponManager.WeaponObjActiveDisable(selectEquipItem[num]);
        mountedItemdata.Remove(selectEquipItem[num].itemdata);

        mounted[num] = false;
        selectEquipItem[num].itemdata = null;
        selectEquipItem[num].image.sprite = null;
        selectEquipItem[num].gameObject.SetActive(false);
        currentEquipSlot[num].gameObject.SetActive(true);
        longDistanceWeapon = false;

        inven.slotEquip.Remove(num);
    }
    //장착해제한 아이템 인벤에 생성
    public bool ReleaseItemCreate(Item item)
    {
        if (item.itemdata != null)
        {
            if (inven.inventoryTabList[item.itemdata.Type.ToString()].Contains(item.itemdata))
            {
                int index = inven.inventoryTabList[item.itemdata.Type.ToString()].IndexOf(item.itemdata);
                inven.inventoryTabList[item.itemdata.Type.ToString()][index].Count += item.itemdata.Count;
                return true;
            }
            else
            {
                if (inven.AllCount() > inven.slots.Length)
                {
                    return false;
                }
                else
                {
                    inven.inventoryTabList[item.itemdata.Type.ToString()].Add(item.itemdata);
                    inven.inventoryTabList["Total"].Add(item.itemdata);
                    return true;
                }
            }
        }

        return false;
    }

}
