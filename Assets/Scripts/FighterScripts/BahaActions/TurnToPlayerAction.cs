using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnToPlayerAction : Action
{
    GameObject player;
    [SerializeField] float turnSpeed;
    bool paused = false;
    public void Start(){
        player = GameObject.FindGameObjectWithTag("Player");
    }
    public override void StartAction(FighterController fighter){
        running = true;
        Debug.Log("turning");
        StartCoroutine(Turn());
    }
    public override void Stop(){}
    public override void Pause(){ paused = true; }
    public override void Resume(){ paused = false; }
    private IEnumerator Turn(){
        float turnUntil = 0f;
        if(Vector3.Distance(player.transform.position, transform.position) > 20f){
            turnUntil = 1.999f;
        }
        else{
            turnUntil = 1.99f;
        }
        while(Vector3.Distance(transform.forward, player.transform.forward)<turnUntil){
            while(paused){
                yield return null;
            }
            Debug.Log("Is it stuck here?");
            Vector3 targetDir = player.transform.position - transform.position;
            Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, turnSpeed*Time.deltaTime, 0.0f);
            transform.rotation = Quaternion.LookRotation(newDir);
            transform.eulerAngles = new Vector3(0,transform.eulerAngles.y,transform.eulerAngles.z);
            yield return null;
        }
        running = false;
    }
}
