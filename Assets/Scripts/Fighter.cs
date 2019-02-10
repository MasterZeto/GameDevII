using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CommandPatternNew
{
    public class Fighter : MonoBehaviour
    {   //movement support
        private string moveInputAxis = "Vertical";
        private string turnInputAxis = "Horizontal";
        public float rotationRate = 360;
        public float moveSpeed = 2;

        //attack support
        public Collider[] attackHitboxes;
        Command buttonG;
        Command buttonH;

        //animation support
        Animator anim;
        int punchHash = Animator.StringToHash("punch");



        void Start()
        {
    
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
           float moveAxis = Input.GetAxis(moveInputAxis);
           float turnAxis = Input.GetAxis(turnInputAxis);
           Move(moveAxis);
           Turn(turnAxis);
           

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



    }
}