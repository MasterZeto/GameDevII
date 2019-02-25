using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SawyerDashLeft :DashAction
{
    public override void StartAction(FighterController fighter)
    {
        running = true;
        this.fighter = fighter;
        StartCoroutine(DashLeftRoutine());
    }

    public override void Stop() { running = false; }
    public override void Pause() { }
    public override void Resume() { }

    private IEnumerator DashLeftRoutine()
    {
        Vector3 moveDirection = Vector3.Lerp(-transform.right, transform.forward, 0.5f);
        Debug.Log("move");
        for (float t = 0f; t < dash_duration && running; t += Time.deltaTime)
        {
            fighter.RelativeMove(moveDirection * dash_speed);
            yield return null;
        }
        running = false;
    }
}
