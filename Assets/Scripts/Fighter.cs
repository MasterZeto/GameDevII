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
        public float jumpSpeed = 10;
        Collider boxCollider;
        float distToGround;
        Rigidbody rb;
        
        [SerializeField] Transform enemy;

        //attack support
        public List<Collider> attackHitboxes;
        public Hitbox punchHitbox;
        //sorry for making this public, is used in Dummy Pause script
        public Command buttonG;
        public Command buttonH;

        //animation support
        Animator anim;
        //int punchHash = Animator.StringToHash("punch");

        //queue stuff
        public bool waiting = false;
        public List<Command> comQueue;
        private bool pause = false;
        private DummyPauseScript pauseScript;
        void Awake()
        {
             buttonG = new ProjectileAttack();
            //right arm
            buttonH = new VoidDelegateCommand(Punch);
            //buttonH = new ArmAttack();
            
        }
        void Start()
        {
            anim = GetComponent<Animator>();
            comQueue = new List<Command>();
           
            boxCollider = GetComponent<BoxCollider>();
            distToGround = boxCollider.bounds.extents.y;
            rb = GetComponent<Rigidbody>();
            pauseScript=GameObject.Find("PauseManager").GetComponent<DummyPauseScript>();
        }

        // Update is called once per frame
        void Update()
        {
            HandleInput();
            HandleMovement();
            DoAction();
        }

        public void HandleInput()
        {
            if(waiting||pause||comQueue.Count!=0){
                return;
            }
            //projectile
            if (Input.GetKeyDown(KeyCode.G))
            {
                comQueue.Add(buttonG);
               // buttonG.Execute(buttonG,transform,attackHitboxes[0]);
                //comQueue.Remove(buttonG);

            }
            //right arm
            if (Input.GetKeyDown(KeyCode.H))
            {  
                //waiting=true;
                //anim.SetTrigger("punch");
                // Debug.Log("AAAAA");
                //buttonH.Execute(buttonH,transform,null);
                comQueue.Add(buttonH);
                Debug.Log(comQueue.Count);
            }

        }
        public void HandleMovement()
        {
           float moveAxis = Input.GetAxis(moveInputAxis);
           float turnAxis = Input.GetAxis(turnInputAxis);
           Move(moveAxis);
           Turn(turnAxis);
           bool isGrounded = Physics.Raycast(transform.position, -Vector3.up, distToGround + 1.2f);
            //Debug.Log(isGrounded);
           Jump(isGrounded);
           if (moveAxis > 0.0001f || moveAxis < -0.0001f)
            {
                anim.SetBool("walk", true);
            }
            else
            {
                anim.SetBool("walk", false);
            }

            transform.forward = Vector3.ProjectOnPlane(enemy.position - transform.position, Vector3.up).normalized;
        }
        public void DoAction(){
            if(waiting){
                return;
            }
            if(comQueue.Count==0&&pause&&!pauseScript.pause){
                Unpause();
                pauseScript.waiting = false;
            }
            if(comQueue.Count!=0){
                if(comQueue[0]==buttonG){
                    waiting=true;
                    buttonG.Execute(buttonG,transform,null);
                    StartCoroutine(FakeTiming());
                }
                else if(comQueue[0]==buttonH&&!waiting){
                    Debug.Log("slkdfjsldjf");
                    waiting=true;
                    Punch();
                    StartCoroutine(FakeTiming());
                    buttonH.Execute(buttonH,transform,null);
                    
                }
                else if(!waiting){
                    waiting=true;
                    comQueue[0].Execute(comQueue[0], transform, null);
                    StartCoroutine(FakeTiming());
                    Debug.Log("this one");
                }
            }
        }
        public void UpdateUI(){
            if(pause){
                pauseScript.UpdateUI();
            }
        }
        void Move(float input)
        {
            transform.Translate(Vector3.forward* input* moveSpeed* Time.deltaTime);
            transform.position = Vector3.ClampMagnitude(transform.position, 32f);
        }
        void Turn (float input)
        {
            //rotate around y axis
            transform.Translate(Vector3.right * input * moveSpeed * Time.deltaTime);
            transform.position = Vector3.ClampMagnitude(transform.position, 32f);
        }
        void Jump(bool isGrounded)
        { 
            if(Input.GetKeyDown(KeyCode.Space) && isGrounded)
            {
                Debug.Log("on the ground");
                //  Vector3 jump = new Vector3(0, 10, 0);
                // rb.AddForce(jump * jumpSpeed, ForceMode.Impulse);
                rb.velocity = Vector3.up * jumpSpeed;
            }
        }

        public void Punch()
        {
            punchHitbox.Fire(.5f);
            anim.SetTrigger("punch");
        }

        public void Pause(){
            pause = true;
        }
        public void Unpause(){
            pause = false;
        }
        private IEnumerator FakeTiming(){
            yield return new WaitForSeconds(.5f);
            comQueue.RemoveAt(0);
            UpdateUI();
            waiting=false;
        }

    }
}