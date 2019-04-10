using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarpoonFishAction : Action
{
    public HitboxForHarpoon hitbox;
    [SerializeField] float speed = 5f;
    [SerializeField] GameObject harpoon;
    [SerializeField] Transform harpoonStart;
    [SerializeField] Transform playerHoistPoint;
    [SerializeField] Transform playerDropPoint;
    [SerializeField] float hit_duration;
    [SerializeField] float hit_delay;
    [SerializeField] string anim_name;
    Rigidbody rb;
    LineRenderer rope;
    Harpooned hitCheck;
    bool paused = false;
    bool delayDone = false;
    public bool playerHit = false;
    float timeTohit;
    GameObject opponent;
    Transform opponentLoc;
    Vector3 direction;
    float t;
    bool returning = false;

    public void Start(){
        //anchor.IsActive(false);
        rb = harpoon.GetComponent<Rigidbody>();
        hitCheck = harpoon.GetComponent<Harpooned>();
        rope = harpoon.GetComponent<LineRenderer>();
        rope.positionCount = 0;
    }
    public override void StartAction(FighterController fighter) 
    {
        this.fighter = fighter;
        opponent = fighter.GetOpponent();
        opponentLoc = opponent.transform;
        direction = opponentLoc.position - harpoonStart.transform.position;
        direction.y/=3;
        direction.Normalize();
        playerHit = false;
        hitCheck.playerAttached = false;
        returning = false;
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
        harpoon.transform.position = harpoonStart.position;
        harpoon.SetActive(true);
        for (t = 0f; t < hit_delay; t += Time.deltaTime) 
        {
            while(paused){
                yield return null;
            }
            opponentLoc = opponent.transform;
            direction = opponentLoc.position - harpoonStart.transform.position;
            direction.y/=3;
            direction.Normalize();
            yield return null;
        }
        rope.positionCount = 2;
        rope.SetPosition(0, harpoonStart.position);
        //direction.y = 0;
        //direction.Normalize();
        hitbox.Fire(hit_duration);
        for( t = 0f; t < hit_duration; t+= Time.deltaTime){
            while(paused){
                rb.velocity = Vector3.zero;
                Debug.Log("AHHH");
                yield return null;
            }
            rb.velocity=direction*speed;
            Debug.Log(hitbox.active);
            rope.SetPosition(1, harpoon.transform.position);
            if(hitCheck.playerAttached){
                playerHit = true;
                break;
            } 
            timeTohit = t;
            if(hitCheck.isStunned) break;
            yield return null;
        }
        rope.SetPosition(1, harpoon.transform.position);
        returning = true;
        StartCoroutine(ReturnHarpoon());
    }
    private IEnumerator ReturnHarpoon(){
        t = 0;
        if(hitCheck.playerAttached){
            direction = playerHoistPoint.position - harpoon.transform.position;
            direction.Normalize();
            while(harpoon.transform.position.y< playerHoistPoint.position.y){
                Debug.Log("is it stuck going to hoist point?");
                Debug.Log(direction);
                rb.velocity = direction*speed/2;
                t+=Time.deltaTime;
                opponent.transform.position = harpoon.transform.position;
                rope.SetPosition(1, harpoon.transform.position);
                yield return null;
            }
            direction = playerDropPoint.position - harpoon.transform.position;
            direction.Normalize();
            while(Vector3.Distance(harpoon.transform.position, playerDropPoint.position)>2f){
                Debug.Log("is it stuck going to drop point?");
                rb.velocity = direction*speed/2;
                t+=Time.deltaTime;
                opponent.transform.position = harpoon.transform.position;
                rope.SetPosition(1, harpoon.transform.position);
                yield return null;
            }
        }
        direction = harpoonStart.transform.position - harpoon.transform.position;
        direction.Normalize();
        while(Vector3.Distance(harpoon.transform.position, harpoonStart.position)>2f){
            while(paused){
                rb.velocity = Vector3.zero;
                yield return null;
            }
            rb.velocity = direction*speed/2;
            t+=Time.deltaTime;
            rope.SetPosition(1, harpoon.transform.position);
            if(Vector3.Distance(opponentLoc.position,transform.position)<5f&&hitCheck.playerAttached){
                break;
            }
            yield return null;
        }
        rb.velocity = Vector3.zero;
        if(hitCheck.playerAttached){
            hitCheck.UnparentPlayer();
        }
        if(hitCheck.isStunned){
            hitCheck.isStunned = false;
        }
        harpoon.SetActive(false);
        rope.positionCount = 0;
        delayDone = true;
    }
    public override bool IsDone() { return delayDone; }
    public float GetRemainingTime(){
        if(!returning){
            return t;
        }
        return hit_duration;
    }
    public float GetSpeed(){
        return speed;
    }
    public float GetHitDuration(){
        return hit_duration;
    }
    public Vector3 GetDirection(){
        return direction;
    }
}
