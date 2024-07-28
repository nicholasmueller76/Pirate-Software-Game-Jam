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

    [SerializeField]
    Animator anim;

    [SerializeField]
    bool inAction;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        PlayerActionController.OnActionStart += delegate () { inAction = true; };
        PlayerActionController.OnActionEnd += delegate () { inAction = false; };
    }

    private void OnDisable()
    {
        PlayerActionController.OnActionStart -= delegate () { inAction = true; };
        PlayerActionController.OnActionEnd -= delegate () { inAction = false; };
    }

    // Update is called once per frame
    void Update()
    {
        if(!inAction)
        {
            moveVector = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

            isSprinting = (Input.GetKey(KeyCode.LeftShift) && moveVector.magnitude != 0);

            if (moveVector.magnitude != 0)
            {
                anim.SetInteger("Horiz MoveDir", (int)moveVector.x);
                anim.SetInteger("Vert MoveDir", (int)moveVector.z);

                if(moveVector.x != 0)
                {
                    anim.SetBool("Facing Right", moveVector.x > 0);
                }
            }
        }
        else
        {
            moveVector = Vector3.zero;
            isSprinting = false;
        }
    }

    private void FixedUpdate()
    {
        if (!isSprinting)
        { 
            rb.velocity = Vector3.ClampMagnitude(rb.velocity + moveVector.normalized * acceleration, walkSpeed);
        }
        else
        {
            rb.velocity = Vector3.ClampMagnitude(rb.velocity + moveVector.normalized * acceleration * 2, sprintSpeed);
        }
    }
}
