using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
namespace CommandPattern
{
    public class dummyPauseScript : MonoBehaviour
    {
        // bool pause=false;
        private Command Pause, Unpause;
        bool pause = false;
        public Button bttn;
        private List<Command> pauseQueue;
        // Start is called before the first frame update
        void Start()
        {
            Pause = new PauseGame();
            Unpause = new UnpauseGame();
            bttn.gameObject.SetActive(false);
            bttn.onClick.AddListener(addToQueue);
            pauseQueue = new List<Command>();
        }

        // Update is called once per frame
        void Update()
        {
            if(Input.GetKeyDown(KeyCode.P)){
                if(!pause){
                    Pause.Execute(Pause);
                    bttn.gameObject.SetActive(true);
                    pause = true;
                    pauseQueue.Clear();
                }
                else{
                    Unpause.Execute(Unpause);
                    bttn.gameObject.SetActive(false);
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
            void addToQueue(){
            pauseQueue.Add(new dummyCom());
        }
    }
}
