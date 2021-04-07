using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcCheck : MonoBehaviour
{
    public GameObject chatBox;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            chatBox.SetActive(true);
        }
    }
    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            chatBox.SetActive(false);
        }
    }
}
