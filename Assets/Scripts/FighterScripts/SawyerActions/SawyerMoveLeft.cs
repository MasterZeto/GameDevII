using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SawyerMoveLeft : Action
{
    private float move_scale;
    private Vector3 move_dir;
    private float move_time;

    public override void Pause() { move_scale = 0f; }
    public override void Resume() { move_scale = 1f; }

    public override void StartAction(FighterController fighter)
    {
        running = true;
        this.fighter = fighter;
        move_scale = 1.0f;
        move_dir = new Vector3(-1f, 0f, 0f);
        move_time = 1f;
        StartCoroutine(RunLeftRoutine());
    }

    public override void Stop()
    {
        StopCoroutine(RunLeftRoutine());
        running = false;
    }

    private IEnumerator RunLeftRoutine()
    {
        for (float t = 0f; t < move_time; t += Time.deltaTime * move_scale)
        {
            fighter.RelativeMove(move_dir);
            yield return null;
        }
        running = false;
        Debug.Log("Move left status: " + running);
    }
}
