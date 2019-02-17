using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseScript : MonoBehaviour
{
    //main script dealing w/ the pause stuff
    //replace fightercontroller here for w/e is responsible for pausing the enemy
    [SerializeField] FighterController enemy;
    private List<voidDelegate> pauseQueue;
    FighterController playerActions;
    bool pause = false;
    bool executing = false;
    // Start is called before the first frame update
    void Start()
    {
        pauseQueue = new List<voidDelegate>();
        pauseQueue.Add(() => playerActions.DashLeft());
        pauseQueue.Add(() => playerActions.LeftPunch());
        playerActions = GameObject.FindWithTag("Player").GetComponent<FighterController>();
    }

    // Update is called once per frame
    void Update()
    {
        //move this to input handler later
        if(Input.GetAxisRaw("Pause") == 1){
            if(!pause&&!executing){
            Time.timeScale = 0;
            pause = true;
            if(enemy!=null){
                //stop enemy's action, somehow.
            }   
            }
            else if(pause&&!executing){
                Time.timeScale = 1;
                executing = true;
                pause = false;
                ExecuteMoves();
            }
        }
    }
    void ExecuteMoves(){
        Debug.Log("sdkfhsdkfh");
        foreach(voidDelegate action in pauseQueue){
            action();
        }
        executing = false;
        if(enemy!=null){
            //resume enemy's action, somehow
        }
    }
    //used for the queue :/
    public delegate void voidDelegate();
}
