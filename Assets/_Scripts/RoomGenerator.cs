using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class RoomGenerator : MonoBehaviour
{
    public GameObject roomPrefab;
    public GameObject[] doorPrefabs; // Array of door prefabs (top, bottom, left, right)
    public GameObject[] bossDoorPrefabs; // Array of boss door prefabs (top, bottom, left, right)
    public Transform playerTransform;

    private GameObject currentRoom;
    [SerializeField] private List<Transform> availableDoorSpawnPoints = new List<Transform>();
    private int roomCount = 0;
    private int maxRooms = 5;
    public bool bossRoomGenerated = false;
    private int entryDoorIndex = -1; // -1 represents no entry
    [SerializeField] private int oppositeDoorIndex;

    void Start()
    {
        FindDoorSpawnPoints();
        GenerateInitialRoom();
    }

    void FindDoorSpawnPoints()
    {
        Transform[] allChildren = roomPrefab.GetComponentsInChildren<Transform>();
        foreach (Transform child in allChildren)
        {
            if (child != roomPrefab.transform && !child.CompareTag("Background"))
            {
                availableDoorSpawnPoints.Add(child);
            }
        }
    }

    void GenerateInitialRoom()
    {
        currentRoom = Instantiate(roomPrefab, Vector3.zero, Quaternion.identity);
        PlaceDoors(currentRoom);
        roomCount++;
    }

    void PlaceDoors(GameObject room)
    {
        if (entryDoorIndex >= 0)
        {
            Transform doorSpawnPoint = availableDoorSpawnPoints[entryDoorIndex];
            Instantiate(doorPrefabs[entryDoorIndex], doorSpawnPoint.position, doorSpawnPoint.rotation, room.transform); // Door player just entered from
        }

        if (roomCount != maxRooms - 1)
        {
            int randomDoorIndex;
            do
            {
                randomDoorIndex = Random.Range(0, availableDoorSpawnPoints.Count);
            }
            while (entryDoorIndex >= 0 && randomDoorIndex == entryDoorIndex);

            Instantiate(doorPrefabs[randomDoorIndex], availableDoorSpawnPoints[randomDoorIndex].position, availableDoorSpawnPoints[randomDoorIndex].rotation, room.transform);
        }
    }

    int GetOppositeDoorIndex(int entryIndex)
    {
        switch (entryIndex)
        {
            case 0:
                return 1;
            case 1:
                return 0;
            case 2:
                return 3;
            case 3:
                return 2;
        }
        return -1;
    }

    public void TransitionToNextRoom(int doorIndex)
    {
        entryDoorIndex = GetOppositeDoorIndex(doorIndex); // Set the opposite door index for entry in the next room
        Destroy(currentRoom);
        currentRoom = Instantiate(roomPrefab, Vector3.zero, Quaternion.identity);
        PlaceDoors(currentRoom);
        MirrorPlayerPosition();
        roomCount++;

        if (roomCount == maxRooms && !bossRoomGenerated)
        {
            GenerateBossRoom();
        }
    }

    void MirrorPlayerPosition()
    {
        Vector3 roomCenter = currentRoom.transform.position;

        Vector3 playerPositionRelativeToCenter = playerTransform.position - roomCenter;

        Vector3 mirroredPositionRelativeToCenter = new Vector3(-playerPositionRelativeToCenter.x, -playerPositionRelativeToCenter.y, playerPositionRelativeToCenter.z);

        playerTransform.position = roomCenter + mirroredPositionRelativeToCenter;
    }


    public void GenerateBossRoom()
    {
        bossRoomGenerated = true;
        Transform doorSpawnPoint = availableDoorSpawnPoints[entryDoorIndex];
        int randomDoorIndex;
        do
        {
            randomDoorIndex = Random.Range(0, availableDoorSpawnPoints.Count);
        }
        while (entryDoorIndex >= 0 && randomDoorIndex == entryDoorIndex);
        Instantiate(bossDoorPrefabs[randomDoorIndex], availableDoorSpawnPoints[randomDoorIndex].position, availableDoorSpawnPoints[randomDoorIndex].rotation, currentRoom.transform);
    }
}