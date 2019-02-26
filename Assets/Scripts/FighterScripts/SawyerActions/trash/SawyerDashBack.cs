using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SawyerDashBack : DashAction
{
    bool right = true;
    public override void StartAction(FighterController fighter)
    {
        running = true;
        this.fighter = fighter;
        StartCoroutine(DashRightRoutine());
    }

    public override void Stop() { running = false; }
    public override void Pause() { }
    public override void Resume() { }

    private IEnumerator DashRightRoutine()
    {
        right = !right;
        if (right)
        {
            Vector3 moveDirection = Vector3.Lerp(transform.right, -transform.forward, 0.5f);
            for (float t = 0f; t < dash_duration && running; t += Time.deltaTime)
            {
                fighter.RelativeMove(moveDirection * dash_speed);
                yield return null;
            }
            running = false;
        }
        else
        {

            Vector3 moveDirection = Vector3.Lerp(-transform.right, -transform.forward, 0.5f);
            for (float t = 0f; t < dash_duration && running; t += Time.deltaTime)
            {
                fighter.RelativeMove(moveDirection * dash_speed);
                yield return null;
            }
            running = false;

        }


    }
}