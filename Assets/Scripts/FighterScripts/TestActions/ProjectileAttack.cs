using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileAttack : Action
{
    //maybe make an abstract class for Projectiles overall l8r
    public Hitbox hitbox;
    [SerializeField] float hit_duration;
    //will there be a delay for animation or w/e?
    [SerializeField] float hit_delay;
    [SerializeField] string anim_name;
    [SerializeField] GameObject projectile;
    [SerializeField] Transform projectileLoc;
    [SerializeField] float speed = 5f;
    float t;
    //possibily used for different movement bullet patterns, used for predictor line.
    string path = "Straight";
    CharacterController char_cont;
    bool delayDone = false;

    public override void StartAction(FighterController fighter) 
    {
        this.fighter = fighter;
        fighter.SetTrigger(anim_name);
        StartCoroutine(HitWithDelayRoutine());
    }

    public override void Stop() 
    {

    }
    public override void Pause() 
    {
        if (hitbox.active) { hitbox.Pause(); }
    }
    public override void Resume() 
    {
        if (hitbox.active) { hitbox.Resume(); }
    }

    private IEnumerator HitWithDelayRoutine()
    {
        delayDone = false;
        for (t = 0f; t < hit_delay; t += Time.deltaTime) 
        {
            yield return null;
        }
        GameObject firedProjectile = Instantiate(projectile, projectileLoc.position, Quaternion.identity);
        //Debug.Log(firedProjectile);
        Hitbox firedHitbox = firedProjectile.GetComponent<Hitbox>();
        hitbox._collider = firedProjectile.GetComponent<BoxCollider>();
        firedHitbox._collider = firedProjectile.GetComponent<BoxCollider>();
        //char_cont = firedProjectile.GetComponent<CharacterController>();
        hitbox.Fire(hit_duration);
        firedHitbox.Fire(hit_duration);
        delayDone = true;
        for(t = 0f; t < hit_duration; t +=Time.deltaTime){
          //  char_cont.Move(speed*transform.forward*Time.deltaTime);
            hitbox._collider.gameObject.transform.position+=speed*transform.forward*Time.deltaTime;
            yield return null;
        }
        while(hitbox.active){
            yield return null;
        }
        Destroy(firedProjectile);
    }

    public string GetPathType(){return path;}
    public float GetRemainingDuration(){
        if(delayDone){
            return t;
        }
        return 0; 
    }
    public float GetDuration() {return hit_duration;}
    public float GetSpeed() {return speed;}

    public override bool IsDone() { return !hitbox.active&&delayDone; }
}
