﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Add this stuff to the main command pattern script later, some commands are just here for testing

namespace CommandPattern
{
    public abstract class Command{
        public abstract void Execute(Command command);
    }
    
    public class PauseGame : Command{
        public override void Execute(Command command){
            Time.timeScale = 0.0f;
        }
    }
    public class UnpauseGame : Command{
        public override void Execute(Command command){
            Time.timeScale = 1.0f;
        }
    }
    public class dummyCom: Command{
        public override void Execute(Command command){
            Debug.Log("24234234");
        }
    }
}
