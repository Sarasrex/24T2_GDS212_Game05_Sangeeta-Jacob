using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public int doorIndex;
    public bool isBossDoor;
    public bool isOpen = false;

    [SerializeField] private Sprite openSprite;

    private void Update()
    {
        if (GameObject.FindGameObjectsWithTag("Enemy").Length == 0)
        {
            isOpen = true;
            GetComponent<SpriteRenderer>().sprite = openSprite;
        }
    }
}
