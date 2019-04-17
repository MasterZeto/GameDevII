using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashForward : DashAction
{
    public override void StartAction(FighterController fighter)
    {
        running = true;
        this.fighter = fighter;
        fighter.SetTrigger("DashForward");
        StartCoroutine(DashForwardRoutine());        
    }

    public override void Stop() { running = false; }
    public override void Pause() {}
    public override void Resume() {}

    private IEnumerator DashForwardRoutine()
    {
        gameObject.GetComponent<SoundBox>().ExtraSFX();
        for (float t = 0f; t < dash_duration && running; t += Time.deltaTime)
        {
            fighter.UnsafeMove(fighter.transform.forward * dash_speed);
            yield return null;
        }
        running = false;
    }
}
