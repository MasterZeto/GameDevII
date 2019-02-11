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
        public List<Collider> attackHitboxes=GameObject.Find("MurryMech 1").GetComponent<Fighter>().attackHitboxes;
        public Animator anim=GameObject.Find("MurryMech 1").GetComponent<Animator>();
        public int punch=Animator.StringToHash("punch");
        public abstract void Execute(Command command, Transform transform,Collider col);
        public virtual void Move(Command command, Transform transform){}
        public virtual void LaunchAttack(Collider col, Transform transform)
        {

            if(col==null)
            {
                Debug.Log("no col");
            }
            // or can use an overlapsphere here which is cheaper but may require multiple spheres
            //test against colliders only in the Hitbox layer(now only contain the head and the torso)
            Collider[] cols = Physics.OverlapBox(col.bounds.center, col.bounds.extents, transform.rotation, LayerMask.GetMask("Hitbox"));
            foreach (Collider c in cols)
            {
                //check if the collider we hit is ourself 
                if (c.transform.root == transform)
                    continue;
                //only print the collider from enemy  
                Debug.Log(c.name);
                float damage = 0;
                switch(c.name)
                {

                    case "Head":
                        damage = 30;
                        break;
                    case "Torso":
                        damage = 10;
                        break;
                    default:
                        Debug.Log("unable to identify body part, make sure the name matches the switch case");
                        break;
                
                
                }

                c.SendMessageUpwards("TakeDamage",damage);
            }


        }
    }
      
    
    public class PauseGame : Command{
        public override void Execute(Command command, Transform transform,Collider col = null){
            Time.timeScale = 0.0f;
        }
    }
    public class UnpauseGame : Command{
        public override void Execute(Command command, Transform transform,Collider col = null){
            Time.timeScale = 1.0f;
        }
    }
    public class dummyCom: Command{
        public override void Execute(Command command, Transform transform,Collider col = null){
            Debug.Log("24234234");
        }
    }
    public class dummyCom2: Command{
        public override void Execute(Command command, Transform transform,Collider col = null){
            Debug.Log("this is another fake command, yay");
        }
    }
    public class dummyCom3: Command{
        public override void Execute(Command command, Transform transform,Collider col = null){
            Debug.Log("third testing command!1!!1!!one!");
        }
    }
    //note: current unscaled movement is a bit nauseating, sorry about that
    public class MoveForwardUnscaled: Command{
        public override void Execute(Command command, Transform transform,Collider col = null){
            Debug.Log("Don't use this one yet, oops sorry");
        }
        public override void Move(Command command, Transform transform){
            if(speed<maxSpeed){
                speed=speed+Acceleration*Time.unscaledDeltaTime;
            }
            transform.position+=new Vector3(0,0,speed*Time.unscaledDeltaTime);
        }
    }
    public class MoveBackUnscaled: Command{
        public override void Execute(Command command, Transform transform,Collider col = null){
            Debug.Log("Don't use this one yet, oops sorry");
        }
        public override void Move(Command command, Transform transform){
            if(speed>-maxSpeed){
                speed=speed-Acceleration*Time.unscaledDeltaTime;
            }
            transform.position+=new Vector3(0,0,speed*Time.unscaledDeltaTime);
        }
    }
    public class MoveLeftUnscaled: Command{
        public override void Execute(Command command, Transform transform, Collider col = null){
            Debug.Log("Don't use this one yet, oops sorry");
        }
        public override void Move(Command command, Transform transform){
            if(speed>-maxSpeed){
                speed=speed-Acceleration*Time.unscaledDeltaTime;
            }
            transform.position+=new Vector3(speed*Time.unscaledDeltaTime,0,0);
        }
    }
    public class MoveRightUnscaled: Command{
        public override void Execute(Command command, Transform transform,Collider col = null){
            Debug.Log("Don't use this one yet, oops sorry");
        }
        public override void Move(Command command, Transform transform){
            if(speed<maxSpeed){
                speed=speed+Acceleration*Time.unscaledDeltaTime;
            }
            transform.position+=new Vector3(speed*Time.unscaledDeltaTime,0,0);
        }
    }
      public class ArmAttack : Command
    {
        public override void Execute(Command command, Transform transform, Collider col)
        {
            anim.SetTrigger(punch);
            LaunchAttack(attackHitboxes[0],transform);
     

       
        }
  
    }

    public class ProjectileAttack : Command
    {
        public override void Execute(Command command, Transform transform, Collider col)
        {
            LaunchAttack(attackHitboxes[1],transform);
        }
    }

    public class DoNothing : Command
    {
        public override void Execute(Command command, Transform transform, Collider col)
        {

        }
    }
     
}
