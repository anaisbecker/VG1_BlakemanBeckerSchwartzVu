using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;


public class Adventurer : MonoBehaviour
{
    public static Adventurer instance;
    
    // Outlet
    public Image imageHealthBar;
    public TMP_Text textGameOver;
    public TMP_Text textBulletsLeft;
    Rigidbody2D _rigidbody2D;
    SpriteRenderer sprite;
    Animator animator;
    public Transform aimPivot;
    public GameObject projectilePrefab;


    // State Tracking
    int jumpsLeft = 1;
    public float health = 100f;
    public float healthMax = 100f;
    public int bulletsLeft = 5;
    public bool bounceBack = false;
    public Vector2 movement;
    public bool isPaused;

    void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        bulletsLeft = 5;
        textBulletsLeft.text = bulletsLeft.ToString();

    }

    // Update is called once per frame
    void Update()
    {
        // aim toward mouse
        //Vector3 mousePosition = Input.mousePosition;
        //Vector3 mousePositionInWorld = Camera.main.ScreenToWorldPoint(mousePosition);
        //Vector3 directionFromPlayerToMouse = mousePositionInWorld - transform.position;

        //float radiansToMouse = Mathf.Atan2(directionFromPlayerToMouse.y, directionFromPlayerToMouse.x);
        //float angleToMouse = radiansToMouse * Mathf.Rad2Deg;

        //aimPivot.rotation = Quaternion.Euler(0, 0, angleToMouse);

        // On Menu Pause
        if(isPaused)
        {
            return;
        }


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

        if(Input.GetKeyDown(KeyCode.Space) && bulletsLeft > 0) {
            GameObject newProjectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            if (sprite.flipX == false)
            {
                newProjectile.transform.right = transform.right;
            }
            else
            {
                newProjectile.transform.right = -transform.right;
            }
            
            // Update Bullets and text
            bulletsLeft--;
            textBulletsLeft.text = bulletsLeft.ToString();
        }

        // Crouch
        if (Input.GetKey(KeyCode.S))
        {
            animator.ResetTrigger("Idle");
            animator.SetTrigger("Crouch");
        }

        if (Input.GetKeyUp(KeyCode.S))
        {
            animator.ResetTrigger("Crouch");
            animator.SetTrigger("Idle");
        }

        // Jump
        if (Input.GetKeyDown(KeyCode.W))
        {
            if (jumpsLeft > 0)
            {
                jumpsLeft--;
                _rigidbody2D.AddForce(Vector2.up * 15f, ForceMode2D.Impulse);
            }
        }
        animator.SetInteger("JumpsLeft", jumpsLeft);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.GetComponent<Fire>())
        {
            gameObject.GetComponent<Renderer>().enabled = false;
            textGameOver.text = "Game Over";
            StartCoroutine("LoadStartMenuTimer");
        }

        if (other.gameObject.GetComponent<Acid>())
        {
            gameObject.GetComponent<Renderer>().enabled = false;
            textGameOver.text = "Game Over";
            StartCoroutine("LoadStartMenuTimer");
        }
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
                    jumpsLeft = 2;
                }
            }
        }

        // Check if we collided with Zombie
        if (other.gameObject.GetComponent<Zombie>())
        {
            TakeDamage(10f * Time.deltaTime);
            bounceBack = true;
        }

    }

    void FixedUpdate()
    {
        animator.SetFloat("Speed", _rigidbody2D.velocity.magnitude);
        if (_rigidbody2D.velocity.magnitude > 0)
        {
            animator.speed = _rigidbody2D.velocity.magnitude / 3f;
        }
        else
        {
            animator.speed = 1f;
        }

        if (bounceBack)
        {
            movement.x = Input.GetAxis("Horizontal");
            movement.y = Input.GetAxis("Vertical");
            _rigidbody2D.MovePosition(_rigidbody2D.position + movement * -5f * Time.fixedDeltaTime);
            Invoke("StopBounce", 0.3f);
        }   
    }

    void StopBounce()
    {
        bounceBack = false;
    }

    void TakeDamage(float damageAmount)
    {
        health -= damageAmount;
        if(health <= 0)
        {
            // TODO: Revisit End Game logic
            gameObject.GetComponent<Renderer>().enabled = false;
            textGameOver.text = "Game Over";
            StartCoroutine("LoadStartMenuTimer");
        }

        imageHealthBar.fillAmount = health / healthMax;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<Door>())
        {
            textGameOver.text = "Level Complete";
            StartCoroutine("LoadStartMenuTimer");
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<Ammunition>())
        {
            if (Input.GetKeyDown(KeyCode.R) == true)
            {
                bulletsLeft += 10;
                textBulletsLeft.text = bulletsLeft.ToString();
                Destroy(other.gameObject);
            }
        }

        if (other.gameObject.GetComponent<MedKit>())
        {
            if (Input.GetKeyDown(KeyCode.R) == true)
            {
                health = Mathf.Min(healthMax, health + 10f);
                imageHealthBar.fillAmount = health / healthMax;
                Destroy(other.gameObject);
            }
        }
    }

    IEnumerator LoadStartMenuTimer()
    {
        // Wait
        yield return new WaitForSeconds(1);

        SceneManager.LoadScene(0);
    }
}
