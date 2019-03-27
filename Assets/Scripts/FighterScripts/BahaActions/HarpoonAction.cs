using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarpoonAction : Action
{
    public HitboxForGems hitbox;
    [SerializeField] float speed = 5f;
    [SerializeField] GameObject harpoon;
    [SerializeField] Transform harpoonStart;
    [SerializeField] float hit_duration;
    [SerializeField] float hit_delay;
    [SerializeField] string anim_name;
    Rigidbody rb;
    LineRenderer rope;
    Harpooned hitCheck;
    bool paused = false;
    bool delayDone = false;
    public bool playerHit { get; private set; }
    float timeTohit;
    GameObject opponent;
    Transform opponentLoc;
    Vector3 direction;

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
        direction = opponentLoc.position - transform.position;
        playerHit = false;
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
        direction.y = 0;
        direction.Normalize();
        hitbox.Fire(hit_duration);
        for( float t = 0f; t < hit_duration; t+= Time.deltaTime){
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
            if(hitCheck.isStunned) break;
            timeTohit = t;
            yield return null;
        }
        StartCoroutine(ReturnHarpoon());
    }
    private IEnumerator ReturnHarpoon(){
        float t = 0;
        while(Vector3.Distance(harpoon.transform.position, harpoonStart.position)>1f && t < timeTohit*2){
            while(paused){
                rb.velocity = Vector3.zero;
                yield return null;
            }
            rb.velocity=-direction*speed/2;
            t+=Time.deltaTime;
            direction.y = 0;
            direction.Normalize();
            rope.SetPosition(1, harpoon.transform.position);
            yield return null;
        }
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
}
