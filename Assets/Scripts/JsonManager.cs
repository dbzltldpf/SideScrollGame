using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using UnityEngine.Networking;

public class JsonManager : MonoBehaviour
{
    [SerializeField] private string[] streamingAssetsPath;
    public PlayerState playerState;
    public ItemList itemList;
    private Dictionary<int, ItemData> itemDataList;
    //public Character player;

    private void Awake()
    {
        streamingAssetsPath[0] = System.IO.Path.Combine(Application.streamingAssetsPath, "PlayerState.json");
        streamingAssetsPath[1] = System.IO.Path.Combine(Application.streamingAssetsPath, "ItemList.json");

        itemDataList = new Dictionary<int, ItemData>();

        StartCoroutine(CallPlayerState());
        StartCoroutine(CallItemList());

    }

    IEnumerator CallPlayerState()
    {
        string jsonPlayerStateText = string.Empty;

#if UNITY_EDITOR || UNITY_IOS

        jsonPlayerStateText = File.ReadAllText(streamingAssetsPath[0]);

#elif UNITY_ANDROID

        //path경로에서 데이터를 불어옴
         WWW www = new WWW( streamingAssetsPath[0] );

        //만약에 www의 처리가 완료 되었다면

         if (www.isDone)
        {
            //디버그 로그
            Debug.Log("Downloaded");
        }


          //www가 완료 될때까지 기다려줌
        yield return www;

        //만약에 에러가 발생했다면
        if (www.error != null)
        {
            //에러상황 던져줌
            throw new Exception("www downloaded : " + www.error);
        }

        //jsonString에다가 받은 데이터를 string으로 넣어줌
        jsonPlayerStateText = www.text;
       

#endif

        playerState = JsonUtility.FromJson<PlayerState>(jsonPlayerStateText);
        yield return null;

    }

    IEnumerator CallItemList()
    {
        string jsonItemListText = string.Empty;
#if UNITY_EDITOR || UNITY_IOS

        jsonItemListText = File.ReadAllText(streamingAssetsPath[1]);


#elif UNITY_ANDROID

        //path경로에서 데이터를 불어옴
         WWW www = new WWW( streamingAssetsPath[1] );

         if (www.isDone)
        {
            //디버그 로그
            Debug.Log("Downloaded");
        }


          //www가 완료 될때까지 기다려줌
        yield return www;

        //만약에 에러가 발생했다면
        if (www.error != null)
        {
            //에러상황 던져줌
            throw new Exception("www downloaded : " + www.error);
        }

        //jsonString에다가 받은 데이터를 string으로 넣어줌
        jsonItemListText = www.text;

#endif

        itemList = JsonUtility.FromJson<ItemList>(jsonItemListText);
        foreach (var itemData in itemList.item)
        {
            itemDataList.Add(itemData.ID, itemData);
        }
        yield return null;

    }


    public ItemData GetItemData(int id)
    {
        if (itemDataList.ContainsKey(id))
        {
            return itemDataList[id];
        }

        return null;
    }
}
