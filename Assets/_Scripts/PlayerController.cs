using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float horizontal;
    private float vertical;

    // Movement Variables
    [Header("Movement")]
    [SerializeField] private float moveSpeed;

    // References
    [Header("References")]
    private Rigidbody2D rb;
    private RoomGenerator roomGenerator;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        roomGenerator = GameObject.FindWithTag("GameController").GetComponent<RoomGenerator>();
    }

    void Start()
    {
        
    }

    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
    }

    private void FixedUpdate()
    {
        Vector2 movement = new Vector2 (horizontal, vertical);
        rb.velocity = movement * moveSpeed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Door"))
        {
            Door door = collision.GetComponent<Door>();
            if (door != null && door.isOpen)
            {
                roomGenerator.TransitionToNextRoom(door.doorIndex);
            }
        }
    }

}
