using UnityEngine;

public class RoomGenerator : MonoBehaviour
{
    public GameObject roomPrefab; // Room prefab
    public GameObject[] doorPrefabs; // Array of door prefabs (0: top, 1: bottom, 2: left, 3: right)
    public GameObject[] bossDoorPrefabs; // Array of boss door prefabs (0: top, 1: bottom, 2: left, 3: right)

    private GameObject currentRoom;
    private Transform[] doorSpawnPoints;
    private int roomCount = 0;
    private int maxRooms = 5;
    private int entryDoorIndex = -1; // -1 indicates no entry door (for the first room)

    void Start()
    {
        doorSpawnPoints = new Transform[4];
        CollectDoorSpawnPoints();
        GenerateRoom(false);
    }

    void CollectDoorSpawnPoints()
    {
        for (int i = 1; i <= doorSpawnPoints.Length; i++)
        {
            doorSpawnPoints[i - 1] = roomPrefab.transform.GetChild(i);
        }
    }

    void GenerateRoom(bool isBossRoom)
    {
        if (currentRoom != null)
        {
            Destroy(currentRoom);
        }

        currentRoom = Instantiate(roomPrefab, Vector3.zero, Quaternion.identity);

        // If not the first room, instantiate a door at the entry spawn point
        if (entryDoorIndex != -1)
        {
            Door door = Instantiate(doorPrefabs[entryDoorIndex], doorSpawnPoints[entryDoorIndex].position, Quaternion.identity, currentRoom.transform).GetComponent<Door>();
            if (door != null)
            {
                door.Initialize(entryDoorIndex);
            }
        }

        if (!isBossRoom)
        {
            // Generate a random exit door that's not the entry door
            int exitDoorIndex;
            do
            {
                exitDoorIndex = Random.Range(0, doorSpawnPoints.Length);
            }
            while (exitDoorIndex == entryDoorIndex);

            Door exitDoor = Instantiate(doorPrefabs[exitDoorIndex], doorSpawnPoints[exitDoorIndex].position, Quaternion.identity, currentRoom.transform).GetComponent<Door>();
            if (exitDoor != null)
            {
                exitDoor.Initialize(exitDoorIndex);
            }
        }
        else
        {
            // Boss room generation
            Door bossDoor = Instantiate(bossDoorPrefabs[Random.Range(0, bossDoorPrefabs.Length)], doorSpawnPoints[entryDoorIndex].position, Quaternion.identity, currentRoom.transform).GetComponent<Door>();
            if (bossDoor != null)
            {
                bossDoor.Initialize(entryDoorIndex);
            }
        }

        roomCount++;
    }

    public void TransitionToNextRoom(int doorIndex)
    {
        entryDoorIndex = doorIndex; // Use the same door index as the entry for the next room
        bool isBossRoom = roomCount >= maxRooms;
        GenerateRoom(isBossRoom);
    }
}