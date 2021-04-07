using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class transferMap : MonoBehaviour
{
    public string transferMapName;

    public Transform target;
    private PlayerController thePlayer;
    private CameraController theCamera;

    void Start()
    {
        thePlayer = FindObjectOfType<PlayerController>();
        theCamera = FindObjectOfType<CameraController>();
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            thePlayer.currentMapName = transferMapName;
            

            switch (thePlayer.currentMapName)
            {
                case "village":
                    theCamera.transform.position = new Vector3(0, -16, -1);
                    break;
                case "road":
                    theCamera.transform.position = new Vector3(0, 0, -1);
                    break;
                case "dungeon":
                    theCamera.transform.position = new Vector3(0, 16, -1);
                    break;
                case "dungeonRoad":
                    theCamera.transform.position = new Vector3(0, 0, -1);
                    break;
                default:
                    break;
            }
            if (thePlayer.currentMapName == transferMapName)
            {
                thePlayer.transform.position = target.transform.position;
            } 
        }
    }
}
