using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Item : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerDownHandler, IPointerUpHandler
{
    //ItemName가 같은것끼리는 합쳐지게 스택표시
    //ItemCount에 갯수 표시

    public Image image;
    public Transform pool;
    public Transform myParent;
    public ItemData itemdata;
    public Inventory inven;
    public Slot slot;
    private bool isButton = false;
    private float clickCount;
    

    void Awake()
    {
        image = GetComponent<Image>();
        clickCount = 0;
    }
    void FixedUpdate()
    {

        if (isButton)
        {
            inven.useItem = this;
            clickCount++;

            if (clickCount > 30f)
            {
                inven.dataUI[0].SetActive(true);
                inven.dataUiText.text = this.itemdata.Name + ":" + "\n" + this.itemdata.Description;

                if (itemdata.Type == "Use")
                {
                    inven.dataButtonUI[1].SetActive(false);
                    inven.dataButtonUI[2].SetActive(false);

                    inven.dataButtonUI[0].SetActive(true);
                    clickCount = 0;
                }
                else if (itemdata.Type == "Equip")
                {
                    inven.dataButtonUI[0].SetActive(false);
                    inven.dataButtonUI[1].SetActive(false);

                    inven.dataButtonUI[2].SetActive(true);
                    clickCount = 0;
                }
                else
                {
                    inven.dataButtonUI[0].SetActive(false);
                    inven.dataButtonUI[2].SetActive(false);

                    inven.dataButtonUI[1].SetActive(true);
                    clickCount = 0;
                }

            }
        }
    }
    void OnEnable()
    {
        if (myParent)
        {
            transform.SetParent(myParent);
            transform.position = myParent.position;
        }
 
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        transform.SetParent(transform.parent.parent.parent.parent.parent.parent);
        image.raycastTarget = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        isButton = false;
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.SetParent(myParent);
        transform.position = myParent.position;
        image.raycastTarget = true;


    }

    public void OnPointerDown(PointerEventData eventData)
    {
        slot = myParent.GetComponent<Slot>();
        isButton = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isButton = false;
        clickCount = 0;
        slot = myParent.GetComponent<Slot>();

        if (transform.localPosition.x < inven.baseRect.xMin 
            || transform.localPosition.x > inven.baseRect.xMax
            || transform.localPosition.y < inven.baseRect.yMin
            || transform.localPosition.y > inven.baseRect.yMax)
        {
            inven.dataUI[1].SetActive(true);
        }
    }
}
