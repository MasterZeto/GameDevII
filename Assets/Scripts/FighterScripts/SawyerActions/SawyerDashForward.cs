﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SawyerDashForward : DashAction
{
    [SerializeField] string anim_name;
    bool paused = false;
    public override void StartAction(FighterController fighter)
    {
        running = true;
        this.fighter = fighter;
        fighter.SetTrigger(anim_name);
        StartCoroutine(DashForwardRoutine());
    }

    public override void Stop() { running = false; }
    public override void Pause() { paused = true; }
    public override void Resume() { paused = false; }

    public override bool IsPaused() { return paused; }
    private IEnumerator DashForwardRoutine()
    {
        gameObject.GetComponent<SoundBox>().ExtraSFX();
        for (float t = 0f; t < dash_duration && running; t += Time.deltaTime)
        {
            while(paused){
                yield return null;
            }
            fighter.UnsafeMove(fighter.transform.forward * dash_speed);
            yield return null;
        }
        running = false;
    }
}
