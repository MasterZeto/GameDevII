using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CommandPattern
{
    public class Fighter : MonoBehaviour
    {   //movement support
        private string moveInputAxis = "Vertical";
        private string turnInputAxis = "Horizontal";
        public float rotationRate = 360;
        public float moveSpeed = 2;
        public float jumpSpeed = 1;
        Collider meshcollider;
        float distToGround;
        Rigidbody rb;
 

        //attack support
        public Collider[] attackHitboxes;
        Command buttonG;
        Command buttonH;

        //animation support
        Animator anim;
        int punchHash = Animator.StringToHash("punch");

        //queue stuff
        bool waiting = false;
        List<Command> comQueue;



        void Start()
        {
    
            anim = GetComponent<Animator>();
            comQueue = new List<Command>();
            buttonG = new ProjectileAttack();
            //right arm
            buttonH = new ArmAttack();
            meshcollider = GetComponent<MeshCollider>();
            distToGround =meshcollider.bounds.extents.y;
            rb = GetComponent<Rigidbody>();
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
            if(waiting){
                return;
            }
            if (Input.GetKeyDown(KeyCode.G))
            {
                buttonG.Execute(buttonG,transform,attackHitboxes[0]);
                comQueue.Add(buttonG);

            }
            //right arm
            if (Input.GetKeyDown(KeyCode.H))
            {  
                anim.SetTrigger(punchHash);
                buttonH.Execute(buttonH,transform,attackHitboxes[1]);
                comQueue.Add(buttonH);
              
            }

        }
        public void HandleMovement()
        {
           float moveAxis = Input.GetAxis(moveInputAxis);
           float turnAxis = Input.GetAxis(turnInputAxis);
           Move(moveAxis);
           Turn(turnAxis);
           bool isGrounded = Physics.Raycast(transform.position, -Vector3.up, distToGround + 1f);
            Debug.Log(isGrounded);
           Jump(isGrounded);
           if (moveAxis > 0.0001f || moveAxis < -0.0001f)
            {
                anim.SetBool("walk", true);
            }
            else
            {
                anim.SetBool("walk", false);
            }
        }

        void Move(float input)
        {
            transform.Translate(Vector3.forward* input* moveSpeed* Time.deltaTime);
        }
        void Turn (float input)
        {
           //rotate around y axis
           transform.Rotate(0,input*rotationRate*Time.deltaTime,0);
        }
        void Jump(bool isGrounded)
        { 
         if(Input.GetKeyDown(KeyCode.Space)&&isGrounded)
            {
                Debug.Log("on the ground");
                Vector3 jump = new Vector3(0, 10, 0);
                rb.AddForce(jump * jumpSpeed, ForceMode.Impulse);
            }
        }



    }
}