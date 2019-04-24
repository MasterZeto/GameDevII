using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarpoonAction : Action
{
    public HitboxForHarpoon hitbox;
    [SerializeField] float speed = 5f;
    [SerializeField] GameObject harpoon;
    [SerializeField] Transform harpoonStart;
    [SerializeField] float hit_duration;
    [SerializeField] float hit_delay;
    [SerializeField] string anim_name;
    [SerializeField] Transform playerDropPoint = null;
    [SerializeField] float minDist = 7f;
    [SerializeField] float medDist = 8.5f;
    float medminDist = 7.75f;
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
    float oriSpeed;

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
        oriSpeed = speed;
        opponent = fighter.GetOpponent();
        opponentLoc = opponent.transform;
        direction = opponentLoc.position - harpoonStart.transform.position;
        direction.y/=3;
        direction.Normalize();
        if(Vector3.Distance(opponentLoc.position, transform.position)<minDist){
            delayDone = true;
            return;
        }
        if(Vector3.Distance(opponentLoc.position, transform.position)<medDist){
            speed = speed/3;
        }
        if(Vector3.Distance(opponentLoc.position, transform.position)<medminDist){
            speed = speed/4;
        }
        playerHit = false;
        hitCheck.playerAttached = false;
        returning = false;
        fighter.SetTrigger(anim_name);
        harpoon.SetActive(false);
        gameObject.GetComponent<SoundBox>().OnlyBahaNeedsThisManySFX();
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
            if(Vector3.Distance(transform.forward, opponent.transform.forward)<2f){
                Vector3 targetDir = opponent.transform.position - transform.position;
                Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, 10*Time.deltaTime, 0.0f);
                transform.rotation = Quaternion.LookRotation(newDir);
                transform.eulerAngles = new Vector3(0,transform.eulerAngles.y,transform.eulerAngles.z);
            }
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
        gameObject.GetComponent<SoundBox>().ExtraSFX();
        for( t = 0f; t < hit_duration; t+= Time.deltaTime){
            while(paused){
                rope.SetPosition(0, harpoonStart.position);
                rope.SetPosition(1, harpoon.transform.position);
                rb.velocity = Vector3.zero;
                Debug.Log("AHHH");
                yield return null;
            }
            rb.velocity=direction*speed;
            Debug.Log(hitbox.active);
            rope.SetPosition(0, harpoonStart.position);
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
        speed = oriSpeed;
        StartCoroutine(ReturnHarpoon());
    }
    private IEnumerator ReturnHarpoon(){
        t = 0;
        if(playerHit&&playerDropPoint!=null){
            direction = playerDropPoint.position - harpoon.transform.position;
            direction.Normalize();
            Vector3 harpoonDist = harpoon.transform.position;
            Vector3 playerDropPointDist = playerDropPoint.position;
            harpoonDist.y = 0;
            playerDropPointDist.y = 0;
            Debug.Log("akljsdlkajsdasd");
            while(Vector3.Distance(harpoonDist, playerDropPointDist)>.5f){
                Debug.Log("is it stuck going to drop point?");
                rb.velocity = direction*speed/2f;
                t+=Time.deltaTime;
                opponent.transform.position = harpoon.transform.position;
                rope.SetPosition(0, harpoonStart.position);
                rope.SetPosition(1, harpoon.transform.position);
                harpoonDist = harpoon.transform.position;
                playerDropPointDist = playerDropPoint.position;
                harpoonDist.y = 0;
                playerDropPointDist.y = 0;
                if(Vector3.Distance(opponentLoc.position,transform.position)<2f){
                    break;
                }
                if(opponentLoc.position.y<0f){
                    break;
                }
                yield return null;
            }
        }
        direction = harpoonStart.transform.position - harpoon.transform.position;
        direction.Normalize();
        while((Vector3.Distance(harpoon.transform.position, harpoonStart.position)>7f&&!hitCheck.playerAttached)||
        (hitCheck.playerAttached&&Vector3.Distance(opponentLoc.position,transform.position)<2f)){
            while(paused){
                rope.SetPosition(0, harpoonStart.position);
                rope.SetPosition(1, harpoon.transform.position);
                rb.velocity = Vector3.zero;
                yield return null;
            }
            rb.velocity = direction*speed/2;
            t+=Time.deltaTime;
            rope.SetPosition(0, harpoonStart.position);
            rope.SetPosition(1, harpoon.transform.position);
            if(Vector3.Distance(opponentLoc.position,transform.position)<5f&&hitCheck.playerAttached){
                Debug.Log("Is breaking here?");
                break;
            }
            if(opponentLoc.position.y<0f&&hitCheck.playerAttached){
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
        opponentLoc.position = new Vector3(opponentLoc.position.x,0,opponentLoc.position.z);
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
    public Vector3 GetPosition(){
        return harpoon.transform.position;
    }
}
