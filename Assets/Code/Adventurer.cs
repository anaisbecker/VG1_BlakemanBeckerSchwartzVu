using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Adventurer : MonoBehaviour
{
    // Outlet

    Rigidbody2D _rigidbody2D;

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _rigidbody2D.velocity = transform.right * 10f;
    }

    // Update is called once per frame
    void Update()
    {
        // walk backward
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.position += new Vector3(-0.2f, 0, 0);
        }

        // walk forward
        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.position += new Vector3(0.2f, 0, 0);
        }

        // jump
        if(Input.GetKeyDown(KeyCode.Space))
        {
            _rigidbody2D.AddForce(Vector2.up * 15f, ForceMode2D.Impulse);
        }
    }
}
