using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WasteTimeAction : Action
{
    [SerializeField] float gloat_time = 1f;
    bool paused;


    public override void StartAction(FighterController fighter){
        this.fighter = fighter;
        running = true;
        paused = false;
        StartCoroutine(Gloat());

    }
    public override void Stop() {running = false; }
    public override void Pause(){ paused = true; }
    public override void Resume(){ paused = false; }
    private IEnumerator Gloat(){
        for(float t = 0f; t < gloat_time; t+=Time.deltaTime){
            while(paused){
                yield return null;
            }
            yield return null;
        }
        running = false;
    }
}
