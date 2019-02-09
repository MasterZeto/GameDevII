using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fighter : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 moveVector;
    private float verticalVelocity;
    public Collider[] attackHitboxes;
    Animator anim;
    Command buttonG;
    Command buttonH;
    Command buttonJ;
    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        buttonG = new ProjectileAttack();
        buttonH = new ArmAttack();
        buttonJ = new ArmAttack();

    }

    // Update is called once per frame
    void Update()
    {
        HandleInput();
        HandleMovement();
    }
    public void HandleInput()
    {

        //projectile
        if (Input.GetKeyDown(KeyCode.G))
        {
            buttonG.Execute(attackHitboxes[0]);
     
        }
        //arm1
        if (Input.GetKeyDown(KeyCode.H))
        {
            buttonH.Execute(attackHitboxes[1]);
            anim.SetBool("punch", true);
        }
        //arm2
        if (Input.GetKeyDown(KeyCode.J))
        {
            buttonJ.Execute(attackHitboxes[2]);
            anim.SetBool("punch", true);
        }

    }
    public void HandleMovement()
    {

        //handle jump 
        if (controller.isGrounded)
        {
            Debug.Log("on the ground");
            verticalVelocity = 0;
            if (Input.GetKeyDown(KeyCode.Space))
            {
                verticalVelocity = 10;
            }
        }
        else
        { //the player is in the air, apply gravity
            verticalVelocity -= 14 * Time.deltaTime;
        }
        moveVector = Vector3.zero;
        moveVector.x = Input.GetAxis("Horizontal") * 5;
        moveVector.y = verticalVelocity;
        controller.Move(moveVector * Time.deltaTime);

    }



}
