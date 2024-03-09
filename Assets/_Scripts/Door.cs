using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public int doorIndex;
    public bool isOpen = false;

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
        }
    }

    public void Initialize(int index)
    {
        doorIndex = index;
    }
}
