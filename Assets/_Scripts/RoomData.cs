using UnityEngine;

public class RoomData
{
    public Vector2Int Position { get; set; }
    public GameObject RoomObject { get; set; }
    public bool IsVisited { get; set; }

    public RoomData(Vector2Int position, GameObject roomObject)
    {
        Position = position;
        RoomObject = roomObject;
        IsVisited = false;
    }
}
