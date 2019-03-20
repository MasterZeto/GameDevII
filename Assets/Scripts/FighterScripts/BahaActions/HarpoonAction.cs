using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarpoonAction : Action
{
    public Hitbox hitbox;
    [SerializeField] float speed = 5f;
    [SerializeField] GameObject harpoon;
    [SerializeField] Transform harpoonStart;
    [SerializeField] float hit_duration;
    [SerializeField] float hit_delay;
    [SerializeField] string anim_name;
    Harpooned hitCheck;
    bool paused = false;
    bool delayDone = false;
    bool playerHit = false;

    public void Start(){
        //anchor.IsActive(false);
        hitCheck = harpoon.GetComponent<Harpooned>();
    }
    public override void StartAction(FighterController fighter) 
    {
        this.fighter = fighter;
        fighter.SetTrigger(anim_name);
        harpoon.SetActive(false);
        StartCoroutine(HitWithDelayRoutine());
    }

    public override void Stop() 
    {

    }
    public override void Pause() 
    {
        if (hitbox.active) { hitbox.Pause(); paused = true; }
    }
    public override void Resume() 
    {
        if (hitbox.active) { hitbox.Resume(); paused = false; }
    }

    private IEnumerator HitWithDelayRoutine()
    {
        delayDone = false;
        for (float t = 0f; t < hit_delay; t += Time.deltaTime) 
        {
            while(paused){
                yield return null;
            }
            yield return null;
        }
        harpoon.transform.position = harpoonStart.position;
        harpoon.SetActive(true);
        hitbox.Fire(hit_duration);
        Vector3 direction = transform.forward;
        direction.y = 0;
        direction.Normalize();
        for( float t = 0f; t < hit_duration; t+= Time.deltaTime){
            while(paused){
                Debug.Log("AHHH");
                yield return null;
            }
            Debug.Log(paused);
            harpoon.transform.position += direction*speed;
            if(hitCheck.playerAttached) break;
            yield return null;
        }
        StartCoroutine(ReturnHarpoon());
    }
    private IEnumerator ReturnHarpoon(){
        while(Vector3.Distance(harpoon.transform.position, harpoonStart.position)>1f){
            while(paused){
                yield return null;
            }
            Vector3 direction = harpoonStart.forward;
            direction.y = 0;
            direction.Normalize();
            harpoon.transform.position -= direction*speed/2;
            yield return null;
        }
        if(hitCheck.playerAttached){
            hitCheck.UnparentPlayer();
        }
        harpoon.SetActive(false);
        delayDone = true;
    }
    public override bool IsDone() { return !hitbox.active&&delayDone; }
}
