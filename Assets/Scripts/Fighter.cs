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
        int punchHash = Animator.StringToHash("punch");
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
                anim.SetTrigger(punchHash);
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
            if(moveVector.x>0.0001f||moveVector.x<-0.0001f)
            {
                anim.SetBool("walk", true);
            
            }
            else {
                anim.SetBool("walk", false);
          
            }
            moveVector.z = Input.GetAxis("Vertical") * 5;
            if (moveVector.z > 0.0001f || moveVector.z < -0.0001f)
            {
                anim.SetBool("walk", true);

            }
            else
            {
                anim.SetBool("walk", false);
           
            }
            moveVector.y = verticalVelocity;
            controller.Move(moveVector * Time.deltaTime);

        }



    }
}