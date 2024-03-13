using System.Collections.Generic;
using UnityEngine;

public class ColliderSwitcher : MonoBehaviour
{
    private PolygonCollider2D mainCollider;

    private Dictionary<string, PolygonCollider2D> collidersMap;

    private void Awake()
    {
        mainCollider = GetComponent<PolygonCollider2D>();
        InitializeCollidersMap();
    }

    private void InitializeCollidersMap()
    {
        collidersMap = new Dictionary<string, PolygonCollider2D>();

        foreach (var collider in GetComponentsInChildren<PolygonCollider2D>(true))
        {
            collidersMap[collider.gameObject.name] = collider;
        }
    }

    public void SwitchCollider(string colliderName)
    {
        if (collidersMap.TryGetValue(colliderName, out PolygonCollider2D selectedCollider))
        {
            // Clear existing paths
            mainCollider.pathCount = selectedCollider.pathCount;

            // Copy all paths from the selected collider to the main collider
            for (int i = 0; i < selectedCollider.pathCount; i++)
            {
                Vector2[] path = selectedCollider.GetPath(i);
                mainCollider.SetPath(i, path);
            }
        }
    }
}
