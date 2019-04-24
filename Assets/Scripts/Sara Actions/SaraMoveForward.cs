using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaraMoveForward : DashAction
{
    [SerializeField] string anim_name;
    GameObject Player;
    GameObject Opponent;
    bool paused = false;
    public override void StartAction(FighterController fighter)
    {
        running = true;
        this.fighter = fighter;
        Player = GameObject.FindGameObjectWithTag("Player");
        Opponent = GameObject.FindGameObjectWithTag("Opponent");
        StartCoroutine(MoveForwardRoutine());
    }

    public override void Stop() { running = false; }
    public override void Pause() { paused = true; }
    public override void Resume() { paused = false; }

    private IEnumerator MoveForwardRoutine()
    {
        for (float t = 0f; t < dash_duration && running; t += Time.deltaTime)
        {   //this action should be done when sara and player are really close
            float distance = Vector3.Distance(Opponent.transform.position, Player.transform.position);
            if (distance < 0.02f)
            {
                yield return null;
            }
            while (paused)
            {
                yield return null;
            }
            fighter.UnsafeMove(fighter.transform.forward * dash_speed);
            yield return null;
        }
        running = false;
    }
}

