using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
namespace CommandPattern
{
    public class DummyPauseScript : MonoBehaviour
    {
        // bool pause=false;
        //possible coms should store the possible list of commands that can currently be executed
        private List<Command> possibleComs;
        public int numOfPossComs = 3;
        private Command Pause, Unpause, camMoveBack, camMoveForward, camMoveLeft, camMoveRight;
        bool pause = false;
        public List<Command> pauseQueue;
        public GameObject pauseManager;
        public PauseUIManager UIManager;
        private float deltaTime, lastFrozenTime;
        // Start is called before the first frame update
        void Start()
        {
            UIManager=pauseManager.GetComponent<PauseUIManager>();
            possibleComs= new List<Command>();
            Pause = new PauseGame();
            Unpause = new UnpauseGame();
            pauseQueue = new List<Command>();
            possibleComs.Add(new dummyCom());
            possibleComs.Add(new dummyCom2());
            possibleComs.Add(new dummyCom3());
            camMoveForward=new MoveForwardUnscaled();
            camMoveBack=new MoveBackUnscaled();
            camMoveRight=new MoveRightUnscaled();
            camMoveLeft=new MoveLeftUnscaled();
            lastFrozenTime=Time.unscaledTime;
        }

        // Update is called once per frame
        void Update()
        {
            if(Input.GetKeyDown(KeyCode.P)){
                //To Do: try to add all the UI and other stuff to the command rather than being a part of this script
                //Also change the input thing and move it to the InputHandler 
                if(!pause){
                    Pause.Execute(Pause);
                    UIManager.SetUp();
                    pause = true;
                    pauseQueue.Clear();
                }
                else{
                    Unpause.Execute(Unpause);
                    UIManager.Hide();
                    foreach (Command com in pauseQueue)
                    {
                        com.Execute(com);
                    }
                    pause = false;
                }
                
            }
            deltaTime=Time.unscaledTime-lastFrozenTime;
            lastFrozenTime=Time.unscaledTime;
            if(pause){
                //have to use axis raw because freezing time stops acceleration, meaning the acceleration of axis too
                if(Input.GetAxisRaw("Vertical")==1){
                    camMoveForward.Move(camMoveForward, deltaTime, Camera.main.transform);
                }
                if(Input.GetAxisRaw("Vertical")==-1){
                camMoveBack.Move(camMoveBack, deltaTime, Camera.main.transform);
                }
                if(Input.GetAxisRaw("Horizontal")==1){
                    camMoveRight.Move(camMoveRight, deltaTime, Camera.main.transform);
                }
                if(Input.GetAxisRaw("Horizontal")==-1){
                camMoveLeft.Move(camMoveLeft, deltaTime, Camera.main.transform);
                }
            }
        }
        public void addToQueue(int i){
            pauseQueue.Add(possibleComs[i]);
        }
    }
}
