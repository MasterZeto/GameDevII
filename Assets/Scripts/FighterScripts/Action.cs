using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Action : MonoBehaviour
{
    [SerializeField] float heat;

    protected FighterController fighter;
    protected bool running = false;

    public abstract void StartAction(FighterController fighter);
    public abstract void Stop();
    public abstract void Pause();
    public abstract void Resume();
    
    public virtual bool IsDone() { return !running; }

    public float GetHeat() { return heat; }
}
