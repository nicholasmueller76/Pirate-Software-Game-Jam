using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    float horizInput;
    float vertInput;
    
    [SerializeField]
    Vector3 moveVector;

    [SerializeField]
    float walkSpeed;

    CharacterController controller;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        horizInput = Input.GetAxis("Horizontal");
        vertInput = Input.GetAxis("Vertical");

        moveVector = new Vector3(horizInput, 0, vertInput).normalized;

        controller.Move(moveVector * walkSpeed * Time.deltaTime);
    }
}
