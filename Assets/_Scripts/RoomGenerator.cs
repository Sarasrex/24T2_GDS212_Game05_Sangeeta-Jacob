using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Unity.VisualScripting;

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

    // Room storage
    private Dictionary<Vector2Int, RoomData> generatedRooms = new Dictionary<Vector2Int, RoomData>();
    private Vector2Int currentPosition = Vector2Int.zero;

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
        RoomData currentRoomData = new RoomData(currentPosition, currentRoom);
        generatedRooms[currentPosition] = currentRoomData;
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
        Vector2Int direction = GetDirectionFromDoorIndex(doorIndex); // Implement this based on your game's logic
        Vector2Int nextPosition = currentPosition + direction;
        entryDoorIndex = GetOppositeDoorIndex(doorIndex); // Set the opposite door index for entry in the next room

        if (!generatedRooms.TryGetValue(nextPosition, out RoomData nextRoom))
        {
            // Room doesn't exist, create and store it
            GameObject roomObject = Instantiate(roomPrefab, Vector3.zero, Quaternion.identity);
            nextRoom = new RoomData(nextPosition, roomObject);
            generatedRooms[nextPosition] = nextRoom;

            PlaceDoors(roomObject);
            roomCount++;
        }
        else
        {
            nextRoom = generatedRooms[nextPosition];
            if (currentRoom != null) currentRoom.SetActive(false);
            nextRoom.RoomObject.SetActive(true);

            currentRoom = nextRoom.RoomObject;
            currentPosition = nextPosition;

            MirrorPlayerPosition(doorIndex);
            
            return;
        }

        if (currentRoom != null) currentRoom.SetActive(false);
        nextRoom.RoomObject.SetActive(true);

        currentRoom = nextRoom.RoomObject;
        currentPosition = nextPosition;

        MirrorPlayerPosition(doorIndex);

        if (roomCount == maxRooms && !bossRoomGenerated)
        {
            GenerateBossRoom();
        }
    }

    // Sangeeta is a nerd
    void MirrorPlayerPosition(int doorIndex)
    {
        Vector3 roomCenter = currentRoom.transform.position;

        Vector3 playerPositionRelativeToCenter = playerTransform.position - roomCenter;

        Vector3 mirroredPositionRelativeToCenter = new Vector3(-playerPositionRelativeToCenter.x, -playerPositionRelativeToCenter.y, playerPositionRelativeToCenter.z);

        Vector3 offset;

        switch (doorIndex)
        {
            case 0: offset = new Vector3(0, 0.2f, 0); break;
            case 1: offset = new Vector3(0, -0.2f, 0); break;
            case 2: offset = new Vector3(-0.2f, 0, 0); break;
            case 3: offset = new Vector3(0.2f, 0, 0); break;
            default: offset = Vector3.zero; break;
        }

        playerTransform.position = roomCenter + mirroredPositionRelativeToCenter + offset;
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

    private Vector2Int GetDirectionFromDoorIndex(int doorIndex)
    {
        switch (doorIndex)
        {
            case 0: return new Vector2Int(0, 1); // Top door
            case 1: return new Vector2Int(0, -1); // Bottom door
            case 2: return new Vector2Int(-1, 0); // Left door
            case 3: return new Vector2Int(1, 0); // Right door
            default: return Vector2Int.zero;
        }
    }
}