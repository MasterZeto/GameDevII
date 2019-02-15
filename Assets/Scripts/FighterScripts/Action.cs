using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Action
{
    FighterController fighter;
    
    public abstract void Start(FighterController fighter);
    public abstract void Stop();
    public abstract void Pause();
    public abstract void Resume();
    public abstract bool IsDone();
}