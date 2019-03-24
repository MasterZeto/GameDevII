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
    LineRenderer rope;
    Harpooned hitCheck;
    bool paused = false;
    bool delayDone = false;
    bool playerHit = false;

    public void Start(){
        //anchor.IsActive(false);
        hitCheck = harpoon.GetComponent<Harpooned>();
        rope = harpoon.GetComponent<LineRenderer>();
        rope.positionCount = 0;
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
        if (hitbox.active) { hitbox.Pause(); }
         paused = true;
    }
    public override void Resume() 
    {
        if (hitbox.active) { hitbox.Resume(); }
        paused = false;
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
        rope.positionCount = 2;
        rope.SetPosition(0, harpoonStart.position);
        harpoon.transform.position = harpoonStart.position;
        harpoon.SetActive(true);
        Vector3 direction = transform.forward;
        direction.y = 0;
        direction.Normalize();
        hitbox.Fire(hit_duration);
        for( float t = 0f; t < hit_duration; t+= Time.deltaTime){
            while(paused){
                Debug.Log("AHHH");
                yield return null;
            }
            Debug.Log(hitbox.active);
            harpoon.transform.position += direction*speed;
            rope.SetPosition(1, harpoon.transform.position);
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
            rope.SetPosition(1, harpoon.transform.position);
            yield return null;
        }
        if(hitCheck.playerAttached){
            hitCheck.UnparentPlayer();
        }
        harpoon.SetActive(false);
        rope.positionCount = 0;
        delayDone = true;
    }
    public override bool IsDone() { return !hitbox.active&&delayDone; }
}
