using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float horizontal;
    private float vertical;
    private Vector2 movement;

    // Movement Variables
    [Header("Movement")]

    // References
    [Header("References")]
    private Rigidbody2D rb;
    private RoomGenerator roomGenerator;
    private Animator animator;
    public PlayerStats playerStats;
    [SerializeField] private IWeapon currentWeapon;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        roomGenerator = GameObject.FindWithTag("GameController").GetComponent<RoomGenerator>();
    }

    void Start()
    {
        currentWeapon = GetComponent<IWeapon>();
    }

    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        if (movement != Vector2.zero)
        {
            animator.SetFloat("speed", movement.magnitude);
            animator.SetFloat("direction", Mathf.Abs(vertical));

            if (horizontal != 0)
            {
                transform.localScale = new Vector3(-Mathf.Sign(horizontal)*1.5f, 1.5f, 1);
                animator.SetBool("isWalkingSide", true);
            }
            else
            {
                animator.SetBool("isWalkingSide", false);
            }
        }
        else
        {
            animator.SetFloat("speed", 0);
        }

        // Firing logic
        if (Input.GetKey(KeyCode.RightArrow))
        {
            currentWeapon.Fire(Vector2.right);
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            currentWeapon.Fire(Vector2.left);
        }
        else if (Input.GetKey(KeyCode.UpArrow))
        {
            currentWeapon.Fire(Vector2.up);
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            currentWeapon.Fire(Vector2.down);
        }
    }

    private void FixedUpdate()
    {
        movement = new Vector2 (horizontal, vertical);
        rb.velocity = movement * playerStats.speed;
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
