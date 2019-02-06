using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Add this stuff to the main command pattern script later, some commands are just here for testing

namespace CommandPattern
{
    public abstract class Command{
        protected float speed = 0f;
        protected float maxSpeed = 3f;
        protected float Acceleration = 6f;
        public abstract void Execute(Command command);
        public virtual void Move(Command command, float deltaTime, Transform transform){}
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
    public class dummyCom2: Command{
        public override void Execute(Command command){
            Debug.Log("this is another fake command, yay");
        }
    }
    public class dummyCom3: Command{
        public override void Execute(Command command){
            Debug.Log("third testing command!1!!1!!one!");
        }
    }
    //note: current unscaled movement is a bit nauseating, sorry about that
    public class MoveForwardUnscaled: Command{
        public override void Execute(Command command){
            Debug.Log("Don't use this one yet, oops sorry");
        }
        public override void Move(Command command, float deltaTime, Transform transform){
            if(speed<maxSpeed){
                speed=speed+Acceleration*deltaTime;
            }
            transform.position+=new Vector3(0,0,speed*deltaTime);
        }
    }
    public class MoveBackUnscaled: Command{
        public override void Execute(Command command){
            Debug.Log("Don't use this one yet, oops sorry");
        }
        public override void Move(Command command, float deltaTime, Transform transform){
            if(speed>-maxSpeed){
                speed=speed-Acceleration*deltaTime;
            }
            transform.position+=new Vector3(0,0,speed*deltaTime);
        }
    }
    public class MoveLeftUnscaled: Command{
        public override void Execute(Command command){
            Debug.Log("Don't use this one yet, oops sorry");
        }
        public override void Move(Command command, float deltaTime, Transform transform){
            if(speed>-maxSpeed){
                speed=speed-Acceleration*deltaTime;
            }
            transform.position+=new Vector3(speed*deltaTime,0,0);
        }
    }
    public class MoveRightUnscaled: Command{
        public override void Execute(Command command){
            Debug.Log("Don't use this one yet, oops sorry");
        }
        public override void Move(Command command, float deltaTime, Transform transform){
            if(speed<maxSpeed){
                speed=speed+Acceleration*deltaTime;
            }
            transform.position+=new Vector3(speed*deltaTime,0,0);
        }
    }  
}
