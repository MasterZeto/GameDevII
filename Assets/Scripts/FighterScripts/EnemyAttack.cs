using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : Action
{
    [SerializeField] Hitbox hitbox;
    [SerializeField] float hit_duration;
    [SerializeField] float hit_delay;
    [SerializeField] string anim_name;

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
        for (float t = 0f; t < hit_delay; t += Time.deltaTime) 
        {
            yield return null;
        }
        hitbox.Fire(hit_duration);
    }

    public override bool IsDone() { return !hitbox.active; }
}
