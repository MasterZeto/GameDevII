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
        StartCoroutine(Turn());
    }
    public override void Stop(){}
    public override void Pause(){ paused = true; }
    public override void Resume(){ paused = false; }
    private IEnumerator Turn(){
        while(Vector3.Distance(transform.forward, player.transform.forward)<1.999f){
            Debug.Log(Vector3.Distance(transform.forward, player.transform.forward));
            while(paused){
                yield return null;
            }
            Vector3 targetDir = player.transform.position - transform.position;
            Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, turnSpeed*Time.deltaTime, 0.0f);
            transform.rotation = Quaternion.LookRotation(newDir);
            yield return null;
        }
        running = false;
    }
}
