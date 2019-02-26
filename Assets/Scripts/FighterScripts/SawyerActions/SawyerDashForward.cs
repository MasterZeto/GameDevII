using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SawyerDashForward : DashAction
{
    [SerializeField] string anim_name;
    public override void StartAction(FighterController fighter)
    {
        running = true;
        this.fighter = fighter;
        fighter.SetBoolTrue(anim_name);
        fighter.SetBoolFalse("ZigLeft");
        fighter.SetBoolFalse("ZigRight");

        StartCoroutine(DashForwardRoutine());
    }

    public override void Stop() { running = false; }
    public override void Pause() { }
    public override void Resume() { }

    private IEnumerator DashForwardRoutine()
    {
        for (float t = 0f; t < dash_duration && running; t += Time.deltaTime)
        {
            fighter.UnsafeMove(fighter.transform.forward * dash_speed);
            yield return null;
        }
        running = false;
    }
}
