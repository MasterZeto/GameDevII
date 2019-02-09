using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CommandPatternNew
{
    public class Fighter : MonoBehaviour
    {
        private CharacterController controller;
        private Vector3 moveVector;
        private float verticalVelocity;
        public Collider[] attackHitboxes;
        Animator anim;
        Command buttonG;
        Command buttonH;
        // Start is called before the first frame update
        void Start()
        {
            controller = GetComponent<CharacterController>();
            anim = GetComponent<Animator>();
            buttonG = new ProjectileAttack();
            //right arm
            buttonH = new ArmAttack();
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
                buttonG.Execute(attackHitboxes[0],transform);

            }
            //right arm
            if (Input.GetKeyDown(KeyCode.H))
            {
                anim.SetBool("punch", true);
                buttonH.Execute(attackHitboxes[1],transform);
              
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
}