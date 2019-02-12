using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
namespace CommandPattern
{
    public class DummyPauseScript : MonoBehaviour
    {
        //possible coms should store the possible list of commands that can currently be executed
        private List<Command> possibleComs;
        public int numOfPossComs = 3;
        private Command Pause, Unpause, camMoveBack, camMoveForward, camMoveLeft, camMoveRight;
        public bool pause = false;
        public List<Command> pauseQueue;
        public GameObject pauseManager;
        public PauseUIManager UIManager;
        //will change this once timing is added to commands or w/e
        public bool waiting = false;
        private Vector3 originalCamPos, originalCamRot;
        private Vector3 velocity=Vector3.zero;
        private Animator anim;
        private float punchTime = 1;
        private Fighter fighter;
        private int comsBeforePause = 0;
        // Start is called before the first frame update
        void Start()
        {
            anim=GameObject.Find("MurryMech 1").GetComponent<Animator>();
            fighter=GameObject.Find("MurryMech 1").GetComponent<Fighter>();
            UIManager = pauseManager.GetComponent<PauseUIManager>();
            possibleComs = new List<Command>();
            Pause = new PauseGame();
            Unpause = new UnpauseGame();
            pauseQueue = new List<Command>();
            possibleComs.Add(fighter.buttonH);
            possibleComs.Add(fighter.buttonG);
            //possibleComs.Add(new dummyCom3());
            camMoveForward = new MoveForwardUnscaled();
            camMoveBack = new MoveBackUnscaled();
            camMoveRight = new MoveRightUnscaled();
            camMoveLeft = new MoveLeftUnscaled();
            originalCamPos=Camera.main.transform.position;
            RuntimeAnimatorController ac = anim.runtimeAnimatorController;
            for(int i = 0; i<ac.animationClips.Length; i++)
            {
                //Debug.Log(ac.animationClips[i].name);
                if(ac.animationClips[i].name == "Punching")        
                    {
                        punchTime = ac.animationClips[i].length;
                    }
            }
            Debug.Log(punchTime);    
        }

        // Update is called once per frame
        void Update()
        {
            if(Input.GetKeyDown(KeyCode.P)){
                //Also change the input thing and move it to the InputHandler 
                Debug.Log("waiting is set to: "+ waiting);
                if(!pause && !waiting){
                    Debug.Log("this runs");
                    Pause.Execute(Pause,transform, null);
                    UIManager.SetUp();
                    pause = true;
                    originalCamPos=Camera.main.transform.position;
                    originalCamRot=Camera.main.transform.eulerAngles;
                    pauseQueue.Clear();
                    fighter.Pause();
                    comsBeforePause=fighter.comQueue.Count;
                }
                else if(!waiting){
                    //change the startCoroutine so that it depends on the timing of the moves rather than just hard coding the time
                    //StartCoroutine(ExecuteComs());
                    foreach(Command com in pauseQueue){
                        fighter.comQueue.Add(com);
                    }
                    Unpause.Execute(Unpause, transform, null);
                    UIManager.Hide();
                    pause = false;
                    waiting = true;
                }
                
            }
            if(!pause){
                //Camera.main.transform.position = Vector3.SmoothDamp(Camera.main.transform.position, originalCamPos, ref velocity, .5f, Mathf.Infinity,Time.unscaledDeltaTime);
            }
            if(pause){
                //have to use axis raw because freezing time stops acceleration, meaning the acceleration of axis too
                if(Input.GetAxisRaw("Vertical") == 1){
                    camMoveForward.Move(camMoveForward, Camera.main.transform);
                }
                if(Input.GetAxisRaw("Vertical") == -1){
                camMoveBack.Move(camMoveBack, Camera.main.transform);
                }
                if(Input.GetAxisRaw("Horizontal") == 1){
                    camMoveRight.Move(camMoveRight, Camera.main.transform);
                }
                if(Input.GetAxisRaw("Horizontal") == -1){
                camMoveLeft.Move(camMoveLeft, Camera.main.transform);
                }
            }
        }
        public void addToQueue(int i){
            pauseQueue.Add(possibleComs[i]);
        }
        public void UpdateUI(){
            if(comsBeforePause!=0){
                comsBeforePause--;
                return;
            }
            UIManager.updateQueueButtons();
        }
        private IEnumerator ExecuteComs(){
            waiting=true;
            UIManager.Hide();
            while(Vector3.Distance(Camera.main.transform.eulerAngles, originalCamRot)>.1f){
                Camera.main.transform.eulerAngles = Vector3.SmoothDamp(Camera.main.transform.eulerAngles, originalCamRot, ref velocity, .5f, Mathf.Infinity,Time.unscaledDeltaTime);
                yield return null;
            }
            while(Vector3.Distance(Camera.main.transform.position, originalCamPos)>.1f){
                Camera.main.transform.position = Vector3.SmoothDamp(Camera.main.transform.position, originalCamPos, ref velocity, .5f, Mathf.Infinity,Time.unscaledDeltaTime);
                yield return null;
            }
            Camera.main.transform.eulerAngles=originalCamRot;
            Camera.main.transform.position=originalCamPos;
            if(fighter.waiting){
                while(fighter.waiting){
                    //wait for punch to finish
                }
            }
            Unpause.Execute(Unpause, transform, null);
            foreach(Command com in pauseQueue){
                if(possibleComs.IndexOf(com)==0){
                    com.Execute(com, transform, fighter.attackHitboxes[1]);
                }
                else if(possibleComs.IndexOf(com)==1){
                    com.Execute(com, transform, fighter.attackHitboxes[0]);
                }
                else{
                    com.Execute(com, transform, null);
                }
                
                UIManager.updateQueueButtons();
                yield return new WaitForSeconds(punchTime);
            }
            fighter.Unpause();
            waiting=false;
        }
    }
}
