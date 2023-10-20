using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : MonoBehaviour
{
    // Outlet
    Rigidbody2D _rigidbody2D;
    SpriteRenderer sprite;
    Animator animator;
    [SerializeField] HealthBar healthBar;
    
    // State Tracking
    public float moveSpeed;
    public float health = 100f;
    public float healthMax = 100f;

    void Awake()
    {
        healthBar = GetComponentInChildren<HealthBar>();
    }

    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        moveSpeed = Random.Range(9f, 11f);
        healthBar.UpdateHealthBar(health, healthMax);
    }

    void OnCollisionEnter2D(Collision2D other) {
        // Take Damage
        if (other.gameObject.GetComponent<Projectile>())
        {
            health -= 25f;
            healthBar.UpdateHealthBar(health, healthMax);
            if (health <= 0)
            {
                Destroy(gameObject);
            }
        }

        if (other.gameObject.GetComponent<Acid>())
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        Adventurer adventurer = FindObjectOfType<Adventurer>();

        if (adventurer != null)
        {
            // Calculate the direction towards the target.
            Vector2 direction = adventurer.transform.position - transform.position;

            if(direction.x < 0)
            {
                // Move the object in the calculated direction.
                _rigidbody2D.AddForce(Vector2.left * moveSpeed * Time.deltaTime, ForceMode2D.Impulse);
                // Flip sprite if zombie is heading left
                sprite.flipX = true;
            }
            else
            {
                // Move the object in the calculated direction.
                _rigidbody2D.AddForce(Vector2.right * moveSpeed * Time.deltaTime, ForceMode2D.Impulse);
                // Zombie is heading right
                sprite.flipX = false;
            }
        }
    }

    void FixedUpdate()
    {
        animator.SetFloat("Speed", _rigidbody2D.velocity.magnitude);
        if(_rigidbody2D.velocity.magnitude > 0)
        {
            animator.speed = _rigidbody2D.velocity.magnitude / 3f;
        }
        else {
            animator.speed = 1f;
        }
    }
}
