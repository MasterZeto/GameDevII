using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileAttack : Action
{
    public Hitbox hitbox;
    [SerializeField] float hit_duration;
    //will there be a delay?
    [SerializeField] float hit_delay;
    [SerializeField] string anim_name;
    [SerializeField] GameObject projectile;
    [SerializeField] Transform projectileLoc;
    [SerializeField] float speed = 5f;
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
        for (float t = 0f; t < hit_delay; t += Time.deltaTime) 
        {
            yield return null;
        }
        GameObject firedProjectile = Instantiate(projectile, projectileLoc.position, Quaternion.identity);
        //Debug.Log(firedProjectile);
        hitbox._collider = firedProjectile.GetComponent<BoxCollider>();
        char_cont = firedProjectile.GetComponent<CharacterController>();
        hitbox.Fire(hit_duration);
        Debug.Log(speed*firedProjectile.transform.forward);
        delayDone = true;
        for(float t = 0f; t < hit_duration; t +=Time.deltaTime){
            char_cont.Move(speed*transform.forward*Time.deltaTime);
            yield return null;
        }
        Destroy(firedProjectile);
    }

    public override bool IsDone() { return !hitbox.active&&delayDone; }
}
