using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public int doorIndex;
    public bool isOpen = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            isOpen = true;
        }
    }

    public void Initialize(int index)
    {
        doorIndex = index;
    }
}
