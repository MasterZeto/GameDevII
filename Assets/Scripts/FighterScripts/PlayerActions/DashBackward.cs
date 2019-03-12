using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashBackward : DashAction
{
    public override void StartAction(FighterController fighter)
    {
        running = true;
        this.fighter = fighter;
        fighter.SetTrigger("DashBackward");
        StartCoroutine(DashBackwardRoutine());        
    }

    public override void Stop() { running = false; }
    public override void Pause() {}
    public override void Resume() {}

    private IEnumerator DashBackwardRoutine()
    {
        for (float t = 0f; t < dash_duration && running; t += Time.deltaTime)
        {
            fighter.UnsafeMove(-fighter.transform.forward * dash_speed);
            yield return null;
        }
        running = false;
    }
}
