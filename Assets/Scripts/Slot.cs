using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Slot : MonoBehaviour, IDropHandler
{
    public Text itemCountText;

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            //새로운 아이템이고
            Item item = eventData.pointerDrag.GetComponent<Item>();
            if (item != null)
            {
                if (transform.childCount == 2)
                {
                    //현재 존재하는 아이템이고
                    Item existentItem = transform.GetChild(1).GetComponent<Item>();

                    if (item.itemdata.ID == existentItem.itemdata.ID)
                    {
                        item.inven.ListClear();
                        existentItem.itemdata.Count += item.itemdata.Count;
                        itemCountText.text = existentItem.itemdata.Count.ToString();
                        item.transform.SetParent(item.pool);
                        item.gameObject.SetActive(false);
                    }
                    else
                    {
                        //temp = item
                        string temp = item.slot.itemCountText.text;
                        //item.text = existentItem.text
                        item.slot.itemCountText.text = existentItem.slot.itemCountText.text;
                        //existentItem.text = temp
                        existentItem.slot.itemCountText.text = temp;

                        ItemData a = existentItem.itemdata;
                        ItemData b = item.itemdata;

                        //2번slot이 1번slot이 되고
                        existentItem.myParent = item.myParent;
                        //2번의 부모가 1번의 부모가되고
                        existentItem.transform.SetParent(item.myParent);
                        //2번의 위치가 1번의 위치
                        existentItem.transform.position = item.myParent.position;

                        //아이템 자리 바꿔준상태로 인벤리스트에 저장
                        item.inven.Swap(a, b);

                        itemCountText.text = existentItem.slot.itemCountText.text;
                        item.slot.itemCountText.text = existentItem.itemdata.Count.ToString();
                    }
                }
                else if (transform.childCount == 1)
                {
                    itemCountText.text = item.itemdata.Count.ToString();

                    if (item.myParent != transform)
                    {
                        item.slot.itemCountText.text = "";
                    }
                }


                item.myParent = transform;
                item.slot = this;
            }
        }
    }
}
