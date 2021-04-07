using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;

public class Inventory : MonoBehaviour
{
    public JsonManager jsonManager;
    public GameManager gameManager;
    public WeaponManager weaponManager;
    public PlayerController player;
    //슬럿에 아이템 넣어주는데 다른 버튼 눌렀을때 들어간 애들 빼서 
    //자식으로 나둘 transform
    public Transform pool;

    //아이템 sprite들을 보관하는 용도
    //처음부터 다 불러 오는것이 아니라 sprite 요청할때 저장하구
    //똑같은 sprite면 기존 저장한 곳에서 반환 아니면 resource에서 불러와서 저장
    private Dictionary<int, Sprite> itemSprites;

    //인벤토리 창에 넣어줄 item prefab
    public GameObject itemPrefab;
    public Dictionary<int, ItemData> slotEquip;

    //아이템 ui 표시
    public GameObject[] dataUI;
    public GameObject[] dataButtonUI;
    public GameObject[] buttonPanel;
    public Text dataUiText;
    public Text[] countInputText;

    int invenListButtonNum;

    //인벤의 범위 설정
    public Rect baseRect;

    //아이템 사용할때 클릭한 아이템
    public Item useItem;

    //public string openSound;
    //public string cancelSound;

    //인벤토리 자식에 있는 Slot들 담는 배열
    public Slot[] slots;

    //public List<Item> existInvenItem;
    public Equipment equipment;

    //item들을 pool에 담을 용도로 사용될 리스트
    private List<Item> inventroyItemPool;

    //key : Use, Equip, Quest, ETC 에 따라 
    //itemData를 담을 리스트 호출
    //리스트로 만들었기 때문에 삭제 속도는 느림. 
    //HashSet도 괜찮은거 같음
    //넣다 뺏다 할꺼면

    public Dictionary<string, List<ItemData>> inventoryTabList; //선택한 탭에 따라 다르게 보여질 아이템 리스트
              
    public string ItemDataType { get; private set; }

    void Awake()
    { 
        //초기화
        inventroyItemPool = new List<Item>();
        slotEquip = new Dictionary<int, ItemData>();
        inventoryTabList = new Dictionary<string, List<ItemData>>();
        itemSprites = new Dictionary<int, Sprite>();
        baseRect = transform.parent.parent.parent.GetComponent<RectTransform>().rect;
        //existInvenItem = new List<Item>();

        //inventoryTabList에 키는 Total이고 itemData를 담을 리스트 초기회
        inventoryTabList.Add("Total", new List<ItemData>());
        //inventoryTabList에 키는 Equip고 itemData를 담을 리스트 초기회
        inventoryTabList.Add("Equip", new List<ItemData>());
        //inventoryTabList에 키는 Use고 itemData를 담을 리스트 초기회
        inventoryTabList.Add("Use", new List<ItemData>());
        //inventoryTabList에 키는 Quest고 itemData를 담을 리스트 초기회
        inventoryTabList.Add("Quest", new List<ItemData>());
        //inventoryTabList에 키는 ETC고 itemData를 담을 리스트 초기회
        inventoryTabList.Add("ETC", new List<ItemData>());

        //slots의 크기는 자식의 갯수 만큼
        slots = new Slot[transform.childCount];

        //slots[i] 에다가 0번째 자식부터 끝 자식에 붙어 있는 Slot을 캐싱
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i] = transform.GetChild(i).GetComponent<Slot>();
            slots[i].itemCountText.text = "";
        }

    }

    void Start()
    {
        CreateItemToInventory(20001);
        CreateItemToInventory(20002);
        CreateItemToInventory(10003);
        CreateItemToInventory(10003);
        CreateItemToInventory(21001);
        CreateItemToInventory(21001);

        AllItemButton();

    }

    public void Swap(ItemData aItem, ItemData bItem)
    {
        int aIndex = inventoryTabList[ItemDataType].IndexOf(aItem);
        ItemData temp = inventoryTabList[ItemDataType][aIndex];
        int bIndex = inventoryTabList[ItemDataType].IndexOf(bItem);
        inventoryTabList[ItemDataType][aIndex] = inventoryTabList[ItemDataType][bIndex];
        inventoryTabList[ItemDataType][bIndex] = temp;
    }
    //전체시트 클릭
    public void AllItemButton()
    {
        PoolClear();

        ItemDataType = "Total";
        InvenListButton();

        for (int i = 0; i < inventoryTabList["Total"].Count; i++)
        {
            ItemData itemData = inventoryTabList["Total"][i];
            Item item = GetItem();
            item.image.sprite = GetItemSprite(itemData.ID);
            item.itemdata = itemData;
            slots[i].itemCountText.text = itemData.Count.ToString();
            item.myParent = slots[i].transform;
            item.slot = slots[i];
            item.gameObject.SetActive(true);
        }

    }
    //타입별시트 클릭 
    public void ClickItemButton(string type)
    {
        PoolClear();

        ItemDataType = type;

        //inventoryTabList type에 따라
        if (inventoryTabList.ContainsKey(type))
        {
            //inventoryTabList[type]에 들어가있는 갯수
            for (int i = 0; i < inventoryTabList[type].Count; i++)
            {
                // itemData = inventoryTabList type의 0 ~ count-1 까지 정보 
                ItemData itemData = inventoryTabList[type][i];
                Item item = GetItem();
                item.image.sprite = GetItemSprite(itemData.ID);
                item.itemdata = itemData;
                slots[i].itemCountText.text = itemData.Count.ToString();
                item.myParent = slots[i].transform;
                item.slot = slots[i];
                item.gameObject.SetActive(true);
            }
        }
    }
    //인벤리스트 버튼 그전에 뭘 클릭했는지 저장해서 그 버튼 시트 그대로 보여주기
    public void SaveInvenListButton()
    {
        switch (invenListButtonNum)
        {
            case 0: //Total
                AllItemButton();
                break;
            case 1:
                ClickItemButton("Equip");
                break;
            case 2:
                ClickItemButton("Use");
                break;
            case 3:
                ClickItemButton("Quest");
                break;
            case 4:
                ClickItemButton("ETC");
                break;
        }
    }
    //인벤 메뉴 리스트 버튼 활성화/비활성화
    public void InvenListButton()
    {
        for (int i = 0; i < buttonPanel.Length; i++)
        {
            buttonPanel[i].SetActive(false);
        }
        switch (ItemDataType)
        {
            case "Total":
                invenListButtonNum = 0;
                break;
            case "Equip":
                invenListButtonNum = 1;
                break;
            case "Use":
                invenListButtonNum = 2;
                break;
            case "Quest":
                invenListButtonNum = 3;
                break;
            case "ETC":
                invenListButtonNum = 4;
                break;
            default:
                break;
        }
       
        buttonPanel[invenListButtonNum].SetActive(true);

    }
    //id에 맞는 Resource 들고옴
    private Sprite GetItemSprite(int id)
    {
        if (itemSprites.ContainsKey(id))
        {
            return itemSprites[id];
        }
        else
        {
            var itemSprite = Resources.Load<Sprite>($"ItemIcon/{id}");
            itemSprites.Add(id, itemSprite);
            return itemSprite;
        }
    }

    public int AllCount()
    {
        int useCount = inventoryTabList["Use"].Count;
        int equipCount = inventoryTabList["Equip"].Count;
        int questCount = inventoryTabList["Quest"].Count;
        int etcCount = inventoryTabList["ETC"].Count;

        return useCount + equipCount + questCount + etcCount +1;
    }

    //itemID받아서 TabList만들어주기
    public bool CreateItemToInventory(int id)
    {
        //먹어서 아이템이 사라지면 true 
        //못 먹으면 false
        ItemData itemData = jsonManager.GetItemData(id);

        if (itemData != null)
        {
            //아이템 주웠을때 같으면 합쳐지게
            if (inventoryTabList[itemData.Type.ToString()].Contains(itemData))
            {
                int index = inventoryTabList[itemData.Type.ToString()].IndexOf(itemData);
                inventoryTabList[itemData.Type.ToString()][index].Count++;
                return true;
            }
            else
            {

                if (AllCount() > slots.Length)
                {
                    return false;
                }
                else
                {
                    inventoryTabList[itemData.Type.ToString()].Add(itemData);
                    inventoryTabList["Total"].Add(itemData);
                    return true;
                }

            }

        }

        return false;
    }
    //itemPool생성
    public Item GetItem()
    {
        //inventroyItemPool 리스트안에 있는 0번째 부터 리스트의 길이만큼 pooledItem에 넣어줌
        foreach (var pooledItem in inventroyItemPool)
        {
            //pooledItem이 비활성화면
            if (!(pooledItem.gameObject.activeSelf))
            {
                //pooledItem 반환
                return pooledItem;
            }
        }
        //obj = 생성한 itemPrefab
        GameObject obj = Instantiate(itemPrefab);
        //item은 obj.Item
        Item item = obj.GetComponent<Item>();
        //item의 pool은 인벤의 pool
        item.pool = pool;
        item.inven = this;
        inventroyItemPool.Add(item);
        obj.SetActive(false);
        return item;
    }

    //아이템 사용
    public void Use()
    {
        string useNotice = $"{useItem.itemdata.Name}사용";

        gameManager.Notify(useNotice);
        switch (useItem.itemdata.ID)
        {
            case 10001:
                player.Hp += 5;

                if (player.Hp > player.maxHp)
                {
                    player.Hp = (int)player.maxHp;
                }
                player.PlayerStateSet();
                break;

            case 10002:
                player.Mp += 5;

                if (player.Mp > player.maxMp)
                {
                    player.Mp = (int)player.maxMp;
                }
                player.PlayerStateSet();
                break;

            case 10003:
                double percentHpHeal = Math.Round(player.maxHp * 0.3f);
                player.Hp += (int)percentHpHeal;

                if (player.Hp > player.maxHp)
                {
                    player.Hp = (int)player.maxHp;
                }
                player.PlayerStateSet();
                break;

            case 10004:
                double percentMpHeal = Math.Round(player.maxMp * 0.3f);
                player.Mp += (int)percentMpHeal;

                if (player.Mp > player.maxMp)
                {
                    player.Mp = (int)player.maxMp;
                }
                player.PlayerStateSet();
                break;

            default:
                break;
        }
        if (useItem.itemdata.Count > 1)
        {
            useItem.itemdata.Count--;
            useItem.slot.itemCountText.text = useItem.itemdata.Count.ToString();
        }
        else
        {
            ListClear();
        }
    }
  
    //버리기
    public void Drop()
    {
        if (useItem.itemdata.Count > 1)
        {
            dataUI[2].SetActive(true);
        }
        else
        {
            //버린아이템 인벤에서 없애주기
            ListClear();


        }
    }

    //분할
    public void Split()
    {
        string splitNotice = "갯수가 1개이기에 분할이 불가능합니다.";

        if (useItem.itemdata.Count > 1)
        {
            dataUI[3].SetActive(true);
        }
        else
        {
            gameManager.Notify(splitNotice);
        }
  
    }
    //장비장착
    public void Equip()
    {
        string equipNotice =  $"{useItem.itemdata.Name} 장착";
        string equipItemNotice = "이미 착용중인 장비 입니다.";


        //새로 장착할 아이템이 기존장착슬롯에 없을 때
        if (!slotEquip.ContainsValue(useItem.itemdata))
        {
            //캐릭의 무기 이미지 켜줌
            weaponManager.WeaponObjActive(useItem);
            switch (useItem.itemdata.ID)
            {
                case 20001:
                    if (slotEquip.ContainsKey(4))
                    {
                        equipment.AutoReleaseEquip(4);
                    }
                    slotEquip.Add(4, useItem.itemdata);
                    equipment.longDistanceWeapon = false;
                    gameManager.Notify(equipNotice);
                    equipment.SetEquipSlot(4);
                    break;

                case 20002:
                    if (slotEquip.ContainsKey(4))
                    {
                        equipment.AutoReleaseEquip(4);
                    }
                    slotEquip.Add(4, useItem.itemdata);
                    equipment.longDistanceWeapon = true;
                    gameManager.Notify(equipNotice);
                    equipment.SetEquipSlot(4);
                    break;

                case 21001:
                    slotEquip.Add(3, useItem.itemdata);
                    gameManager.Notify(equipNotice);
                    equipment.SetEquipSlot(3);
                    break;
            }


            if (useItem.itemdata.Count > 1)
            {
                useItem.itemdata.Count--;
                useItem.slot.itemCountText.text = useItem.itemdata.Count.ToString();
            
            }
            else
            {
                ListClear();
            }
        }
        else
        {
            gameManager.Notify(equipItemNotice);
        }
        SaveInvenListButton();
    }
    public void DropOK()
    {
        int num;

        if (countInputText[0].text != "")
        {
            if (CheckSplitCount(countInputText[0].text))
            {
                num = int.Parse(countInputText[0].text);
                useItem.itemdata.Count -= num;
                if (useItem.itemdata.Count == 0)
                {
                    ListClear();
                }
                else
                {
                    useItem.slot.itemCountText.text = useItem.itemdata.Count.ToString();

                }
            }
        }
    }
    public void SplitOK()
    {
        int num;
        string splitNotice = "분할 갯수와 아이템 갯수가 같습니다.";

        if (countInputText[1].text != "")
        {
            if (CheckSplitCount(countInputText[1].text))
            {
                num = int.Parse(countInputText[1].text);

                if (num < useItem.itemdata.Count)
                {
                    useItem.itemdata.Count -= num;
                    useItem.slot.itemCountText.text = useItem.itemdata.Count.ToString();

                    //분할 아이템 정보 넣어줌
                    Item splitItem = GetItem();
                    splitItem.image.sprite = useItem.image.sprite;
                    splitItem.itemdata.ID = useItem.itemdata.ID;
                    splitItem.itemdata.Name = useItem.itemdata.Name;
                    splitItem.itemdata.Description = useItem.itemdata.Description;
                    splitItem.itemdata.Type = useItem.itemdata.Type;
                    splitItem.itemdata.Count = num;

                    //리스트에 분할아이템 추가
                    inventoryTabList[splitItem.itemdata.Type].Add(splitItem.itemdata);
                    inventoryTabList["Total"].Add(splitItem.itemdata);

                    //분할 했을때 슬롯에 아이템 만들어주는곳
                    for (int i = 0; i < inventoryTabList[splitItem.itemdata.Type].Count; i++)
                    {
                        if (slots[i].transform.childCount == 1)
                        {
                            slots[i].itemCountText.text = splitItem.itemdata.Count.ToString();
                            splitItem.myParent = slots[i].transform;
                            splitItem.slot = slots[i];
                            splitItem.gameObject.SetActive(true);
                            break;
                        }

                    }
                }
                else
                {
                    gameManager.Notify(splitNotice);
                    //Debug.Log("분할 불가");
                }
               
            }
        }

    }

    //분할 할때 갯수 //버릴때 갯수
    private bool CheckSplitCount(string inputCount)
    {
        string countNotice = "갯수 오류입니다.";
        int getCount = int.Parse(inputCount);
        bool getNumber = true;

        if (getCount > useItem.itemdata.Count)
        {
            gameManager.Notify(countNotice);
            //Debug.Log("갯수 오류");
            getNumber = false;
        }
        return getNumber;
    }
    //아이템 없어진애들 리스트 지워주기 
    public void ListClear()
    {
        int index = inventoryTabList[useItem.itemdata.Type].IndexOf(useItem.itemdata);
        int number = inventoryTabList["Total"].IndexOf(useItem.itemdata);

        inventoryTabList[useItem.itemdata.Type].RemoveAt(index);
        inventoryTabList["Total"].RemoveAt(number);
        useItem.gameObject.SetActive(false);
        useItem.transform.SetParent(pool);
        useItem.slot.itemCountText.text = "";
    }
    //아이템 풀 초기화
    public void PoolClear()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            //슬롯에 자식이 있으면
            if (slots[i].transform.childCount >= 2)
            {
                //슬롯에 자식 false, pool로 옮김
                slots[i].transform.GetChild(1).gameObject.SetActive(false);
                slots[i].transform.GetChild(1).SetParent(pool);
                slots[i].itemCountText.text = "";
            }

        }

    }


}
