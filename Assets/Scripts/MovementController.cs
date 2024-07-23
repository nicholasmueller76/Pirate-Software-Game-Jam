using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    [SerializeField]
    Vector3 moveVector;

    [SerializeField]
    float acceleration;

    [SerializeField]
    float walkSpeed;

    [SerializeField]
    float sprintSpeed;

    [SerializeField]
    bool isSprinting;

    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        moveVector = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).normalized;

        isSprinting = (Input.GetKey(KeyCode.LeftShift) && moveVector.magnitude != 0);
    }

    private void FixedUpdate()
    {
        if (!isSprinting)
        { 
            rb.velocity = Vector3.ClampMagnitude(rb.velocity + moveVector * acceleration, walkSpeed);
        }
        else
        {
            rb.velocity = Vector3.ClampMagnitude(rb.velocity + moveVector * acceleration * 2, sprintSpeed);
        }
    }
}
