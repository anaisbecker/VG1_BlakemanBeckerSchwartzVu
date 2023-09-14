using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : MonoBehaviour
{
    // Outlet
    Rigidbody2D _rigidbody2D;

    // State Tracking
    public float moveSpeed;

    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Adventurer target = FindObjectOfType<Adventurer>();

        if (target != null)
        {
            // Calculate the direction towards the target.
            Vector3 direction = (target.transform.position - transform.position).normalized;

            // Move the object in the calculated direction.
            transform.Translate(direction * moveSpeed * Time.deltaTime);
        }
    }
}
