using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SawyerDashBackLeft : DashAction
{
    [SerializeField] string anim_name;
    public override void StartAction(FighterController fighter)
    {
        running = true;
        this.fighter = fighter;
        fighter.SetBoolTrue(anim_name);
        fighter.SetBoolFalse("ZigRight");
        fighter.SetBoolFalse("DashForward");
        StartCoroutine(DashLeftRoutine());
    }

    public override void Stop() { running = false; }
    public override void Pause() { }
    public override void Resume() { }

    private IEnumerator DashLeftRoutine()
    {
        Vector3 moveDirection = Vector3.Lerp(-transform.right, -transform.forward, 0.7f);
        Debug.Log("move");
        for (float t = 0f; t < dash_duration && running; t += Time.deltaTime)
        {
            fighter.Move(moveDirection * dash_speed);
            yield return null;
        }
        running = false;
    }
}
