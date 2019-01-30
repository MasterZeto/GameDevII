using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fighter : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 moveVector;
    private float verticalVelocity;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        //handle jump 
        if(controller.isGrounded)
        {
            verticalVelocity = -1;
            if(Input.GetKeyDown(KeyCode.Space))
            {
                verticalVelocity = 10;
            }
        }
        else
        { //the player is in the air, apply gravity
            verticalVelocity -= 14 * Time.deltaTime;
        }
        moveVector = Vector3.zero;
        moveVector.x = Input.GetAxis("Horizontal")*5;
        moveVector.y = verticalVelocity;
        controller.Move(moveVector * Time.deltaTime);
    }
}
