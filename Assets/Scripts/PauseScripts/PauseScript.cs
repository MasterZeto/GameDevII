using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseScript : MonoBehaviour
{
    //main script dealing w/ the pause stuff
    FighterController playerActions;
    bool pause = false;
    // Start is called before the first frame update
    void Start()
    {
        playerActions = GameObject.FindWithTag("Player").GetComponent<FighterController>();
    }

    // Update is called once per frame
    void Update()
    {
        //move this to input handler later
        if(Input.GetAxisRaw("Pause")==1&&!pause){
            Time.timeScale = 0;
            pause = true;   
        }
        else if(Input.GetAxisRaw("Pause")==1&&pause){
            Time.timeScale = 1;
            pause = false;
        }
    }
}
