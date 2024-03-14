using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossSlimeController : MonoBehaviour
{
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float chargeSpeed = 10f;
    private Animator animator;
    private Rigidbody2D rb;
    [SerializeField] private PolygonCollider2D triggerCollider;
    [SerializeField] private PolygonCollider2D physicalCollider;
    private Vector2 jumpDirection;
    private Transform playerTransform;

    [SerializeField] private float offsetDistance;

    private enum State { Idle, Jumping, Charging }
    private State currentState = State.Idle;

    private void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        StartCoroutine(BehaviorLoop());
    }

    private void Update()
    {
        physicalCollider.pathCount = triggerCollider.pathCount;
        for (int i = 0; i < triggerCollider.pathCount; i++)
        {
            Vector2[] path = triggerCollider.GetPath(i);
            physicalCollider.SetPath(i, path);
        }
    }

    private IEnumerator BehaviorLoop()
    {
        while (true)
        {
            switch (currentState)
            {
                case State.Idle:
                    animator.SetTrigger("Idle");
                    rb.velocity = Vector3.zero;
                    yield return new WaitForSeconds(2f); // Wait for a bit before next action
                    DecideNextAction();
                    break;
                case State.Jumping:
                    if (transform.localScale == new Vector3(1.5f, 1.5f, 1))
                    {
                        transform.position = new Vector3(transform.position.x - offsetDistance, transform.position.y, transform.position.z);
                    }
                    else if (transform.localScale == new Vector3(-1.5f, 1.5f, 1))
                    {
                        transform.position = new Vector3(transform.position.x + offsetDistance, transform.position.y, transform.position.z);
                    }
                    animator.SetTrigger("Jump");
                    jumpDirection = (playerTransform.position - transform.position).normalized;
                    rb.AddForce(jumpDirection * jumpForce, ForceMode2D.Impulse);
                    yield return new WaitForSeconds((1/13)*5f); // Adjust timing as needed
                    triggerCollider.enabled = false; // Disable collider partway through the jump
                    physicalCollider.enabled = false;
                    yield return new WaitForSeconds((1 / 13) * 8f); // Adjust timing for how long collider is disabled
                    triggerCollider.enabled = true; // Re-enable collider
                    physicalCollider.enabled = true;
                    currentState = State.Idle;
                    break;
                case State.Charging:
                    StartCoroutine(Charge());
                    break;
            }
            yield return null;
        }
    }

    private void DecideNextAction()
    {
        // Decide randomly whether to jump or charge
        if (Random.Range(0, 5) < 4) // 80% chance to jump, adjust as needed
        {
            currentState = State.Jumping;
        }
        else
        {
            currentState = State.Jumping;
        }
    }

    private IEnumerator Charge()
    {
        animator.SetTrigger("Charge");

        Vector2 directionToPlayer = (playerTransform.position - transform.position).normalized;
        Vector2 chargeDirection = Vector2.zero;

        // Determine charge direction based on player's position
        if (Mathf.Abs(directionToPlayer.x) > Mathf.Abs(directionToPlayer.y))
        {
            chargeDirection = directionToPlayer.x > 0 ? Vector2.right : Vector2.left;
        }
        else
        {
            chargeDirection = directionToPlayer.y > 0 ? Vector2.up : Vector2.down;
        }

        rb.velocity = chargeDirection * chargeSpeed;

        // Wait until a wall is hit
        yield return new WaitUntil(() => rb.IsTouchingLayers(LayerMask.GetMask("Wall")));
        rb.velocity = Vector2.zero; // Stop moving
        currentState = State.Idle;
    }
}
