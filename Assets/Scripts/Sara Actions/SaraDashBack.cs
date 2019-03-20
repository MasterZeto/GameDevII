using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaraDashBack : DashAction
{
    [SerializeField] string anim_name;
    bool paused = false;
    public override void StartAction(FighterController fighter)
    {
        running = true;
        this.fighter = fighter;
        fighter.SetBoolTrue(anim_name);
        //  fighter.SetBoolFalse("ZigLeft");
        //  fighter.SetBoolFalse("ZigRight");

        StartCoroutine(DashBackwardRoutine());
    }

    public override void Stop() { running = false; }
    public override void Pause() { paused = true; }
    public override void Resume() { paused = false; }

    private IEnumerator DashBackwardRoutine()
    {
        for (float t = 0f; t < dash_duration && running; t += Time.deltaTime)
        {
            while (paused)
            {
                yield return null;
            }
            fighter.UnsafeMove(-fighter.transform.forward * dash_speed);
            yield return null;
        }
        running = false;
    }
}

