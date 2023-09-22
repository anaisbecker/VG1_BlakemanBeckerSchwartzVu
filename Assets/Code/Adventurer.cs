using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Adventurer : MonoBehaviour
{
    // Outlet
    public Image imageHealthBar;
    public TMP_Text textGameOver;
    Rigidbody2D _rigidbody2D;
    SpriteRenderer sprite;
    Animator animator;
    public Transform aimPivot;
    public GameObject projectilePrefab;
    public GameObject adventurer;
    CapsuleCollider2D collider;


    // State Tracking
    public int jumpsLeft;
    public float health = 100f;
    public float healthMax = 100f;

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        collider = GetComponent<CapsuleCollider2D>();
        //_rigidbody2D.velocity = transform.right * 10f;
    }

    // Update is called once per frame
    void Update()
    {
        // aim toward mouse
        Vector3 mousePosition = Input.mousePosition;
        Vector3 mousePositionInWorld = Camera.main.ScreenToWorldPoint(mousePosition);
        Vector3 directionFromPlayerToMouse = mousePositionInWorld - transform.position;

        float radiansToMouse = Mathf.Atan2(directionFromPlayerToMouse.y, directionFromPlayerToMouse.x);
        float angleToMouse = radiansToMouse * Mathf.Rad2Deg;

        aimPivot.rotation = Quaternion.Euler(0, 0, angleToMouse);

        // walk backward
        if (Input.GetKey(KeyCode.A))
        {
            _rigidbody2D.AddForce(Vector2.left * 18f * Time.deltaTime, ForceMode2D.Impulse);
            sprite.flipX = true;
        }

        // walk forward
        if (Input.GetKey(KeyCode.D))
        {
            _rigidbody2D.AddForce(Vector2.right * 18f * Time.deltaTime, ForceMode2D.Impulse);
            sprite.flipX = false;
        }

        float crouchHeight = 3.5f;
        float standHeight = 5f;
        float crouchOffset = -0.75f;

        // duck
        if (Input.GetKey(KeyCode.S))
        {
            GetComponent<Collider>().size = new Vector2(GetComponent<Collider>().size.x, crouchHeight);
            GetComponent<Collider>().center = new Vector2(0, crouchOffset);
            animator.SetTrigger("Crouch");

        }

        // jump
        if(Input.GetKeyDown(KeyCode.W))
        {
            if(jumpsLeft > 0)
            {
                jumpsLeft--;
                _rigidbody2D.AddForce(Vector2.up * 15f, ForceMode2D.Impulse);
            }
        }
        animator.SetInteger("JumpsLeft", jumpsLeft);
        collider.size = new Vector2(collider.size.x, standHeight);
        collider.center = new Vector2(0, -crouchOffset);
    }

    void OnCollisionStay2D(Collision2D other)
    {
        // Check that we collided with Ground or is on top of Zombie
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground") || other.gameObject.GetComponent<Zombie>())
        {
            // Check what is directly below our character's feet
            RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, Vector2.down, 1.2f);
            //Debug.DrawRay(transform.position, Vector2.down * 1.2f); // Visualize Raycast

            // We might have multiple things below our character's feet
            for (int i = 0; i < hits.Length; i++)
            {
                RaycastHit2D hit = hits[i];

                // Check that we collided with ground below our feet or Zombie
                if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Ground") || other.gameObject.GetComponent<Zombie>())
                {
                    // Reset jump count
                    jumpsLeft = 1;
                }
            }
        }

        // Check if we collided with Zombie
        if (other.gameObject.GetComponent<Zombie>())
        {
            TakeDamage(5f * Time.deltaTime);
        }

    }

    void FixedUpdate()
    {
        animator.SetFloat("Speed", _rigidbody2D.velocity.magnitude);
    }

    void TakeDamage(float damageAmount)
    {
        health -= damageAmount;
        if(health <= 0)
        {
            // TODO: Revisit End Game logic
            Destroy(gameObject);
            textGameOver.text = "Game Over";
        }

        imageHealthBar.fillAmount = health / healthMax;
    }
}
