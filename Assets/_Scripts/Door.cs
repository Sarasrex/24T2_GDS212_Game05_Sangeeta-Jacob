using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public int doorIndex;
    public bool isOpen = false;

    [SerializeField] private Sprite openSprite;

    private RoomGenerator roomGenerator;

    private void Awake()
    {
        roomGenerator = GameObject.FindWithTag("GameController").GetComponent<RoomGenerator>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && !roomGenerator.bossRoomGenerated)
        {
            isOpen = true;
            GetComponent<SpriteRenderer>().sprite = openSprite;
        }

        if (gameObject.CompareTag("BossDoor") && Input.GetKeyDown(KeyCode.R))
        {
            isOpen = true;
            GetComponent<SpriteRenderer>().sprite = openSprite;
        }
    }

    public void Initialize(int index)
    {
        doorIndex = index;
    }
}
