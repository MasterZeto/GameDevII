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
        bool pause = false;
        public List<Command> pauseQueue;
        public GameObject pauseManager;
        public PauseUIManager UIManager;
        //will change this once timing is added to commands or w/e
        private bool waiting = false;
        // Start is called before the first frame update
        void Start()
        {
            UIManager = pauseManager.GetComponent<PauseUIManager>();
            possibleComs = new List<Command>();
            Pause = new PauseGame();
            Unpause = new UnpauseGame();
            pauseQueue = new List<Command>();
            possibleComs.Add(new dummyCom());
            possibleComs.Add(new dummyCom2());
            possibleComs.Add(new dummyCom3());
            camMoveForward = new MoveForwardUnscaled();
            camMoveBack = new MoveBackUnscaled();
            camMoveRight = new MoveRightUnscaled();
            camMoveLeft = new MoveLeftUnscaled();
        }

        // Update is called once per frame
        void Update()
        {
            if(Input.GetKeyDown(KeyCode.P)){
                //Also change the input thing and move it to the InputHandler 
                if(!pause && !waiting){
                    Pause.Execute(Pause);
                    UIManager.SetUp();
                    pause = true;
                    pauseQueue.Clear();
                }
                else if(!waiting){
                    Unpause.Execute(Unpause);
                    UIManager.Hide();
                    //change the startCoroutine so that it depends on the timing of the moves rather than just hard coding the time
                    StartCoroutine(ExecuteComs());
                    pause = false;
                }
                
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
        private IEnumerator ExecuteComs(){
            waiting=true;
            foreach(Command com in pauseQueue){
                com.Execute(com);
                yield return new WaitForSeconds(2);
            }
            waiting=false;
        }
    }
}
