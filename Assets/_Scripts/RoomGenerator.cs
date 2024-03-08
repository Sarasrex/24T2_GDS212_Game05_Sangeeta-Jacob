using UnityEngine;
using System.Collections.Generic;

public class RoomGenerator : MonoBehaviour
{
    public GameObject roomPrefab; // Room prefab
    public GameObject[] doorPrefabs; // Array of door prefabs (top, bottom, left, right)
    public GameObject[] bossDoorPrefabs; // Array of boss door prefabs (top, bottom, left, right)

    private GameObject currentRoom;
    [SerializeField] private List<Transform> usedDoorSpawnPoints = new List<Transform>(); // List to track used door spawn points
    [SerializeField] private List<Transform> availableDoorSpawnPoints = new List<Transform>(); // List to store available door spawn points
    [SerializeField] private int roomCount = 0;
    [SerializeField] private int maxRooms = 5;
    private bool bossRoomGenerated = false;

    void Start()
    {
        // Find door spawn points in the room prefab
        FindDoorSpawnPoints();

        // Generate initial room
        GenerateInitialRoom();
    }

    void FindDoorSpawnPoints()
    {
        // Find all child objects with specific tag as door spawn points
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
        if (roomCount < maxRooms)
        {
            // Ensure that there is at least one available door spawn point and the number of used spawn points is less than the total number of spawn points
            if (availableDoorSpawnPoints.Count > 0 && usedDoorSpawnPoints.Count < availableDoorSpawnPoints.Count)
            {
                // Choose the spawn point randomly from available door spawn points
                int randomIndex = Random.Range(0, availableDoorSpawnPoints.Count);
                Transform randomSpawnPoint = availableDoorSpawnPoints[randomIndex];

                // Remove the chosen spawn point from available door spawn points
                availableDoorSpawnPoints.RemoveAt(randomIndex);

                // Add the chosen spawn point to used door spawn points
                usedDoorSpawnPoints.Add(randomSpawnPoint);

                // Determine the direction of the door based on the spawn point's position
                Vector3 doorDirection = randomSpawnPoint.position - room.transform.position;
                // Choose the appropriate door prefab based on the direction
                GameObject doorPrefab = GetDoorPrefab(doorDirection);
                // Spawn the door prefab
                Instantiate(doorPrefab, randomSpawnPoint.position, randomSpawnPoint.rotation, room.transform);
            }
        }
    }

    public void TransitionToNextRoom()
    {
        if (roomCount >= maxRooms)
        {
            // Dead-end room
            return;
        }

        Destroy(currentRoom);
        currentRoom = Instantiate(roomPrefab, Vector3.zero, Quaternion.identity);
        PlaceDoors(currentRoom);
        roomCount++;
        // Reset lists of available and used door spawn points
        usedDoorSpawnPoints.Clear();
        availableDoorSpawnPoints.Clear();
        FindDoorSpawnPoints();
        if (roomCount >= maxRooms && !bossRoomGenerated)
        {
            GenerateBossRoom();
        }
    }

    public void GenerateBossRoom()
    {
        bossRoomGenerated = true;
        // Find the last door spawn point used
        Transform lastDoorSpawnPoint = usedDoorSpawnPoints[usedDoorSpawnPoints.Count - 1];
        // Determine the direction of the last door based on the spawn point's position
        Vector3 doorDirection = lastDoorSpawnPoint.position - currentRoom.transform.position;
        // Choose the appropriate boss door prefab based on the direction
        GameObject bossDoorPrefab = GetBossDoorPrefab(doorDirection);
        // Spawn the boss door prefab
        Instantiate(bossDoorPrefab, lastDoorSpawnPoint.position, lastDoorSpawnPoint.rotation, currentRoom.transform);
    }

    // Helper function to get the door prefab based on the direction
    GameObject GetDoorPrefab(Vector3 direction)
    {
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            return direction.x > 0 ? doorPrefabs[3] : doorPrefabs[2]; // Right or Left
        }
        else
        {
            return direction.y > 0 ? doorPrefabs[0] : doorPrefabs[1]; // Top or Bottom
        }
    }

    // Helper function to get the boss door prefab based on the direction
    GameObject GetBossDoorPrefab(Vector3 direction)
    {
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            return direction.x > 0 ? bossDoorPrefabs[3] : bossDoorPrefabs[2]; // Right or Left
        }
        else
        {
            return direction.y > 0 ? bossDoorPrefabs[0] : bossDoorPrefabs[1]; // Top or Bottom
        }
    }
}
