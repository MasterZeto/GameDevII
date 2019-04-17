using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GloatAction : Action
{
    [SerializeField] float gloat_time = 1f;
    [SerializeField] string anim_name;
    bool paused;


    public override void StartAction(FighterController fighter){
        this.fighter = fighter;
        running = true;
        paused = false;
        if(anim_name!="") fighter.SetTrigger(anim_name);
        StartCoroutine(Gloat());

    }
    public override void Stop() {running = false; }
    public override void Pause(){ paused = true; }
    public override void Resume(){ paused = false; }
    private IEnumerator Gloat(){
        gameObject.GetComponent<SoundBox>().SpecialSFX();
        for(float t = 0f; t < gloat_time; t+=Time.deltaTime){
            while(paused){
                yield return null;
            }
            yield return null;
        }
        running = false;
    }
}
