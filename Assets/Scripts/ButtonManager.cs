using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{
    public PlayerController player;
    public Sprite NextSprite;
    public Sprite CurrentSprite;
    private Image currentImage;
    public bool search;

    void Start()
    {
        search = false;
        currentImage = GetComponent<Image>();
    }
    private void FixedUpdate()
    {
        if (!search)
        {
            if (player.state == State.Check)
            {
                search = true;
                currentImage.sprite = NextSprite;
            }
           
        }
        if (player.state != State.Check)
        {
            search = false;
            currentImage.sprite = CurrentSprite;
        }

    }

}
