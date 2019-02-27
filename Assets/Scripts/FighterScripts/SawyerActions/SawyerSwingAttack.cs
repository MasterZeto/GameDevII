using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SawyerSwingAttack : Action
{
    public Hitbox hitbox;
    [SerializeField] float hit_duration;
    [SerializeField] float hit_delay;
    [SerializeField] string anim_name;
    bool delay_done = false;

    public override void StartAction(FighterController fighter)
    {
        this.fighter = fighter;
        fighter.SetTrigger(anim_name);

        transform.gameObject.GetComponent<SoundBox>().MissSFX();

        fighter.SetBoolFalse("DashForward");
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
       delay_done = false;
        for (float t = 0f; t < hit_delay; t += Time.deltaTime)
        {
            yield return null;
        }
       delay_done = true;
        hitbox.Fire(hit_duration);
    }

    public override bool IsDone() { return !hitbox.active&&delay_done; }
}