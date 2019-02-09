using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Command:MonoBehaviour
{
    //child classes have to override the function
    public abstract void Execute(Collider col);
    //child classes have the option to override the function
    public virtual void LaunchAttack(Collider col) {
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
        }


    }
}

//child classes
public class ArmAttack: Command
{

    public override void Execute(Collider armcol)
    {
        LaunchAttack(armcol);
        //anim.SetBool("punch", true);
    }

}

public class ProjectileAttack:Command
{
    public override void Execute(Collider procol)
    {
        LaunchAttack(procol);
    }
}

public class DoNothing:Command
{
    public override void Execute(Collider col)
    {

    }
}