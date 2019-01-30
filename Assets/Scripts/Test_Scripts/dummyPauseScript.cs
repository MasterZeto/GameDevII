using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace CommandPattern
{
    public class dummyPauseScript : MonoBehaviour
    {
        // bool pause=false;
        private Command Pause, Unpause;
        bool pause = false;
        // Start is called before the first frame update
        void Start()
        {
            Pause=new PauseGame();
            Unpause= new UnpauseGame();
        }

        // Update is called once per frame
        void Update()
        {
            if(Input.GetKeyDown(KeyCode.P)){
                if(!pause){
                    Pause.Execute(Pause);
                    pause=true;
                }
                else{
                    Unpause.Execute(Unpause);
                    pause=false;
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
    }
}
