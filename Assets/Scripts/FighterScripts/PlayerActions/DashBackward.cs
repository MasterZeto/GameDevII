﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashBackward : DashAction
{
    Vector3 move;
    [SerializeField] GameObject landing;
    public override void StartAction(FighterController fighter)
    {
        running = true;
        this.fighter = fighter;
        fighter.SetTrigger("DashBackward");
        if (gameObject.tag == "Player")
        {
            Vector3 position = transform.position + Predictor(transform.forward);
            Instantiate(landing, position, Quaternion.identity);
        }
        StartCoroutine(DashBackwardRoutine());        
    }

    public override void Stop() { running = false; }
    public override void Pause() {}
    public override void Resume() {}

    private IEnumerator DashBackwardRoutine()
    {
        gameObject.GetComponent<SoundBox>().ExtraSFX();
        for (float t = 0f; t < dash_duration && running; t += Time.deltaTime)
        {
            fighter.UnsafeMove(-fighter.transform.forward * dash_speed);
            yield return null;
        }
        running = false;
    }
    public Vector3 Predictor(Vector3 forward)
    {  //here forward should be fighter.transform.forward 
        //10f is the move speed in unsafe move
        move =- forward * dash_duration * dash_speed * 10f;
        return move;
    }
}
