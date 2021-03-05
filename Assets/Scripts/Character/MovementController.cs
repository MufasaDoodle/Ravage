using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    public float speed = 10f;
    public float sprintMultiplier = 1.5f;
    public float jumpSpeed = 20f;
    public float rbPushPower = 2f;

    private float vertSpeed = 0f;
    protected CharacterController characterController;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        HandleMovement();        
    }

    private void HandleMovement()
    {
        //get the speeds from the input axis
        float horizontalMovement = Input.GetAxis("Horizontal");
        float verticalMovement = Input.GetAxis("Vertical");
        bool isGrounded = characterController.isGrounded;

        //if character is grounded, make sure vertspeed is 0
        if (isGrounded && vertSpeed < 0)
        {
            vertSpeed = 0f;
        }

        //apply hor+vert movement
        Vector3 movement = transform.forward * verticalMovement + transform.right * horizontalMovement;

        if (Input.GetKey(KeyCode.LeftShift) && (horizontalMovement > 0 || verticalMovement > 0))
        {
            //check if this moveable character even has the sprint component
            if(GetComponent<SprintController>() != null)
            {
                SprintController sprintController = GetComponent<SprintController>();

                

                //need 20 sprint charge to be allowed to sprint
                if((sprintController.SprintCharge >= 20 || sprintController.IsSprinting) && characterController.isGrounded)
                {
                    sprintController.IsSprinting = true;
                    characterController.Move(speed * sprintMultiplier * Time.deltaTime * movement);

                    if(sprintController.SprintCharge <= 0)
                    {
                        sprintController.IsSprinting = false;
                    }
                }
                else //if we don't have enough charge to sprint, we just move normally
                {
                    sprintController.IsSprinting = false;
                    characterController.Move(speed * Time.deltaTime * movement);
                }
            }
        }
        else //we move normally
        {
            GetComponent<SprintController>().IsSprinting = false;
            characterController.Move(speed * Time.deltaTime * movement);
        }

        //check if player is trying to jump
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            //apply the jump into the vertspeed calc
            vertSpeed += Mathf.Sqrt(-jumpSpeed * Physics.gravity.y);
        }

        //handle gravity
        vertSpeed += Physics.gravity.y * Time.deltaTime;
        Vector3 movementFromGravity = new Vector3(0, vertSpeed, 0);

        //apply vertical change from gravity
        characterController.Move(movementFromGravity * Time.deltaTime);
    }


    //gets called when the controller collides with anything with a collider
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody body = hit.collider.attachedRigidbody;

        //check if the collider has a rigidbody, or is kinematic
        if (body == null || body.isKinematic)
        {
            return;
        }

        //we don't need to push things below us, -0.3 is hard coded, and from the example script reference in the docs
        if(hit.moveDirection.y < -0.3)
        {
            return;
        }

        //create the pushing direction
        Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);

        //apply that force on the rigidbody
        body.velocity = pushDir * rbPushPower;
    }
}
