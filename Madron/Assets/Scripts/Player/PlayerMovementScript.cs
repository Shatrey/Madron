using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementScript : MonoBehaviour
{
    public Rigidbody2D rb;
    public int speed = 10;
    // Update is called once per frame
    void FixedUpdate()
    {
        float moveHor = Input.GetAxis("Horizontal");
        float moveVer = Input.GetAxis("Vertical");
        Vector2 movement = new Vector2(moveHor, moveVer);
        rb.AddForce(movement * speed);
    }
}
