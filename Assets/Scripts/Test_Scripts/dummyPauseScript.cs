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
        private Command Pause, Unpause;
        bool pause = false;
        public List<Command> pauseQueue;
        public GameObject pauseManager;
        public PauseUIManager UIManager;
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
        }
        public void addToQueue(int i){
            pauseQueue.Add(possibleComs[i]);
        }
    }
}
