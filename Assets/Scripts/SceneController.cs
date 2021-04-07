using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class SceneController : MonoBehaviour
{
    public GameObject player;
    public PlayerController playerData;
    public JsonManager jsonManager;
    private bool saved;

    public void Awake()
    {

        playerData = FindObjectOfType<PlayerController>();
        player = playerData.gameObject;
        jsonManager = FindObjectOfType<JsonManager>();
    }
    private void Start()
    {
        saved = false;
    }
    public void NewGame()
    {
        SceneManager.LoadScene("GameScene");
    }
    public void LoadGame()
    {
        SceneManager.LoadScene("GameScene");
    }
    public void GameSave()
    {
        saved = true;
        Debug.Log("정보를 저장합니다.");
        jsonManager.playerState.character[0].X = player.transform.position.x;
        jsonManager.playerState.character[0].Y = player.transform.position.y;
        jsonManager.playerState.character[0].Z = player.transform.position.z;
        jsonManager.playerState.character[0].Hp = playerData.Hp;
        jsonManager.playerState.character[0].Mp = playerData.Mp;
        jsonManager.playerState.character[0].O2 = playerData.O2;
        jsonManager.playerState.character[0].CurrentMapName = playerData.currentMapName;

        StartCoroutine("SaveData");

    }
    public void GameLoad()
    {
        if (saved)
        {
            Debug.Log("정보를 불러옵니다.");
            player.transform.position = new Vector3(jsonManager.playerState.character[0].X, jsonManager.playerState.character[0].Y, jsonManager.playerState.character[0].Z);
            playerData.Hp = jsonManager.playerState.character[0].Hp;
            playerData.Mp = jsonManager.playerState.character[0].Mp;
            playerData.O2 = jsonManager.playerState.character[0].O2;
            playerData.currentMapName = jsonManager.playerState.character[0].CurrentMapName;
            playerData.PlayerStateSet();
            StartCoroutine("LoadData");
        }
        else
        {
            Debug.Log("불러올 정보가 없습니다.");
        }
    }
    public void GameExit()
    {
        SceneManager.LoadScene("MainScene");
        //Application.Quit();
    }

    IEnumerator SaveData()
    {
        Inventory inventory = playerData.inventory;

        int useCount = inventory.inventoryTabList["Use"].Count;
        int equipCount = inventory.inventoryTabList["Equip"].Count;
        int questCount = inventory.inventoryTabList["Quest"].Count;
        int etcCount = inventory.inventoryTabList["ETC"].Count;

        string path = string.Empty;
        string jsonfile = string.Empty;

        if (useCount > 0)
        {
            path = System.IO.Path.Combine(Application.streamingAssetsPath, "Use.json");
            ItemList itemList = new ItemList();
            itemList.item = inventory.inventoryTabList["Use"];
            jsonfile = JsonUtility.ToJson(itemList);
            Debug.Log(jsonfile);
            File.WriteAllText(path, jsonfile);
        }
        if (equipCount > 0)
        {
            path = System.IO.Path.Combine(Application.streamingAssetsPath, "Equip.json");
            ItemList itemList = new ItemList();
            itemList.item = inventory.inventoryTabList["Equip"];
            jsonfile = JsonUtility.ToJson(itemList);
            Debug.Log(jsonfile);
            File.WriteAllText(path, jsonfile);
        }
        if (questCount > 0)
        {
            path = System.IO.Path.Combine(Application.streamingAssetsPath, "Quest.json");
            ItemList itemList = new ItemList();
            itemList.item = inventory.inventoryTabList["Quest"];
            jsonfile = JsonUtility.ToJson(itemList);
            Debug.Log(jsonfile);
            File.WriteAllText(path, jsonfile);
        }
        if (etcCount > 0)
        {
            path = System.IO.Path.Combine(Application.streamingAssetsPath, "ETC.json");
            ItemList itemList = new ItemList();
            itemList.item = inventory.inventoryTabList["ETC"];
            jsonfile = JsonUtility.ToJson(itemList);
            Debug.Log(jsonfile);
            File.WriteAllText(path, jsonfile);
        }


        yield return null;
    }
    IEnumerator LoadData()
    {
        //현재 인벤저장,불러오기 완료
        //장비창 저장,불러오기 시스템 구현해야함
        //같이 연동되어 있어야함
        ItemList itemList = new ItemList();
        Inventory inventory = playerData.inventory;


        string usepath = System.IO.Path.Combine(Application.streamingAssetsPath, "Use.json");
        string path1 = System.IO.Path.Combine(Application.streamingAssetsPath, "Equip.json");
        string path2 = System.IO.Path.Combine(Application.streamingAssetsPath, "Quest.json");
        string path3 = System.IO.Path.Combine(Application.streamingAssetsPath, "ETC.json");

        FileInfo fileInfo = new FileInfo(usepath);
        FileInfo fileInfo1 = new FileInfo(path1);
        FileInfo fileInfo2 = new FileInfo(path2);
        FileInfo fileInfo3 = new FileInfo(path3);



        inventory.inventoryTabList["Use"].Clear();
        inventory.inventoryTabList["Equip"].Clear();
        inventory.inventoryTabList["Quest"].Clear();
        inventory.inventoryTabList["ETC"].Clear();
        inventory.inventoryTabList["Total"].Clear();


        if (fileInfo.Exists)
        {
            string JsonText = File.ReadAllText(usepath);
            itemList = JsonUtility.FromJson<ItemList>(JsonText);
            foreach (var itemData in itemList.item)
            {
                inventory.inventoryTabList[itemData.Type].Add(itemData);
                inventory.inventoryTabList["Total"].Add(itemData);
            }
        }
        if (fileInfo1.Exists)
        {
            string JsonText = File.ReadAllText(path1);
            itemList = JsonUtility.FromJson<ItemList>(JsonText);
            foreach (var itemData in itemList.item)
            {
                inventory.inventoryTabList[itemData.Type].Add(itemData);
                inventory.inventoryTabList["Total"].Add(itemData);
            }

        }
        if (fileInfo2.Exists)
        {
            string JsonText = File.ReadAllText(path2);
            itemList = JsonUtility.FromJson<ItemList>(JsonText);
            foreach (var itemData in itemList.item)
            {
                inventory.inventoryTabList[itemData.Type].Add(itemData);
                inventory.inventoryTabList["Total"].Add(itemData);
            }

        }
        if (fileInfo3.Exists)
        {
            string JsonText = File.ReadAllText(path3);
            itemList = JsonUtility.FromJson<ItemList>(JsonText);
            foreach (var itemData in itemList.item)
            {
                inventory.inventoryTabList[itemData.Type].Add(itemData);
                inventory.inventoryTabList["Total"].Add(itemData);
            }

        }

        yield return null;
    }

}
