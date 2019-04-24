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
    [SerializeField] Camera bahaCam;
    [SerializeField] float max_player_dist = 10f;
    Camera mainCam;
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
    bool brokeEarly = false;
    float t;
    bool returning = false;

    public void Start(){
        //anchor.IsActive(false);
        bahaCam.enabled = false;
        mainCam = Camera.main;
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
        brokeEarly = false;
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
        /* if(Vector3.Distance(transform.position, opponentLoc.position)>max_player_dist){
            Debug.Log("breaking early");
            brokeEarly = true;
            returning = true;
            StartCoroutine(ReturnHarpoon());
            yield break;
        }*/
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
            rope.SetPosition(0, harpoonStart.position);
            rope.SetPosition(1, harpoon.transform.position);
            if(hitCheck.playerAttached){
                playerHit = true;
                break;
            } 
            else if(Vector3.Distance(transform.position, harpoon.transform.position)>max_player_dist){
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
            mainCam.enabled = false;
            bahaCam.enabled = true;
            direction = playerHoistPoint.position - harpoon.transform.position;
            direction.Normalize();
            while(harpoon.transform.position.y< playerHoistPoint.position.y){
                Debug.Log("is it stuck going to hoist point?");
                Debug.Log(direction);
                while(paused){
                    rope.SetPosition(0, harpoonStart.position);
                    rope.SetPosition(1, harpoon.transform.position);
                    rb.velocity = Vector3.zero;
                    Debug.Log("AHHH");
                    yield return null;
                }
                rb.velocity = direction*speed/1.5f;
                t+=Time.deltaTime;
                opponent.transform.position = harpoon.transform.position;
                rope.SetPosition(0, harpoonStart.position);
                rope.SetPosition(1, harpoon.transform.position);
                yield return null;
            }
            direction = playerDropPoint.position - harpoon.transform.position;
            direction.Normalize();
            while(harpoon.transform.position.y> playerDropPoint.position.y){
                Debug.Log("is it stuck going to drop point?");
                while(paused){
                    rope.SetPosition(0, harpoonStart.position);
                    rope.SetPosition(1, harpoon.transform.position);
                    rb.velocity = Vector3.zero;
                    Debug.Log("AHHH");
                    yield return null;
                }
                rb.velocity = direction*speed/1.5f;
                t+=Time.deltaTime;
                opponent.transform.position = harpoon.transform.position;
                rope.SetPosition(0, harpoonStart.position);
                rope.SetPosition(1, harpoon.transform.position);
                yield return null;
            }
            if(hitCheck.playerAttached){
                hitCheck.UnparentPlayer();
            }
            if(hitCheck.isStunned){
                hitCheck.isStunned = false;
            }
             if(mainCam.enabled == false){
                yield return new WaitForSeconds(.05f);
                mainCam.enabled = true;
                bahaCam.enabled = false;
            }
        }
        direction = harpoonStart.transform.position - harpoon.transform.position;
        direction.Normalize();
        while(Vector3.Distance(harpoon.transform.position, harpoonStart.position)>5f&&!brokeEarly){
            while(paused){
                rb.velocity = Vector3.zero;
                rope.SetPosition(0, harpoonStart.position);
                rope.SetPosition(1, harpoon.transform.position);
                yield return null;
            }
            direction = harpoonStart.transform.position - harpoon.transform.position;
            direction.Normalize();
            rb.velocity = direction*speed/2;
            t+=Time.deltaTime;
            rope.SetPosition(0, harpoonStart.position);
            rope.SetPosition(1, harpoon.transform.position);
            if(Vector3.Distance(opponentLoc.position,transform.position)<2f&&hitCheck.playerAttached){
                break;
            }
            yield return null;
        }
        harpoon.SetActive(false);
        if(bahaCam.enabled){
            bahaCam.enabled = false;
            mainCam.enabled = true;
        }
        rope.positionCount = 0;
        rb.velocity = Vector3.zero;
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
