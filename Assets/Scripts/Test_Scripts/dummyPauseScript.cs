using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
namespace CommandPattern
{
    public class DummyPauseScript : MonoBehaviour
    {
        // bool pause=false;
        private Command Pause, Unpause;
        bool pause = false;
        private List<Command> pauseQueue;
        public GameObject pauseManager;
        public PauseUIManager UIManager;
        // Start is called before the first frame update
        void Start()
        {
            UIManager=pauseManager.GetComponent<PauseUIManager>();
            Pause = new PauseGame();
            Unpause = new UnpauseGame();
            pauseQueue = new List<Command>();
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
            /* if(Input.GetKeyDown(KeyCode.P)&&!pause){
            Time.timeScale=0;
            //UI shit here... change this to command pattern too
            }
            else if(Input.GetKeyDown(KeyCode.P)&&pause){
                Time.timeScale=1;
            }*/
        }
        public void addToQueue(){
            pauseQueue.Add(new dummyCom());
        }
    }
}
