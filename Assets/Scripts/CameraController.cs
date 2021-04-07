using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public PlayerController player;

    private void Awake()
    {
        player = FindObjectOfType<PlayerController>();
    }
    private void Start()
    {
        switch (player.currentMapName)
        {
            case "road":
                transform.position = new Vector3(0, 0, -1);
                break;
            case "village":
                transform.position = new Vector3(0, -16, -1);
                break;
            case "dungeon":
                transform.position = new Vector3(0, 16, -1);
                break;
        }
        //road
        //transform.position = new Vector3(0, 0, -1);
        //village
        //transform.position = new Vector3(0, -16, -1);
        //dungeon
        //transform.position = new Vector3(0, 16, -1);
    }
    //loadTest로 update잠시 만듬
    private void Update()
    {
        switch (player.currentMapName)
        {
            case "road":
                transform.position = new Vector3(0, 0, -1);
                break;
            case "village":
                transform.position = new Vector3(0, -16, -1);
                break;
            case "dungeon":
                transform.position = new Vector3(0, 16, -1);
                break;
        }
        //road
        //transform.position = new Vector3(0, 0, -1);
        //village
        //transform.position = new Vector3(0, -16, -1);
        //dungeon
        //transform.position = new Vector3(0, 16, -1);
    }

}
