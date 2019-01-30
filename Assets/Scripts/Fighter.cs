using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fighter : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 moveVector;
    private float verticalVelocity;
    public Collider[] attackHitboxes;
    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {   //projectile
        if(Input.GetKeyDown(KeyCode.G))
        {
            LaunchAttack(attackHitboxes[0]);
        }
        //arm1
        if (Input.GetKeyDown(KeyCode.H))
        {
            LaunchAttack(attackHitboxes[1]);
        }
        //arm2
        if (Input.GetKeyDown(KeyCode.J))
        {
            LaunchAttack(attackHitboxes[2]);
        }


        //handle jump 
        if (controller.isGrounded)
        {
            verticalVelocity = -1;
            if(Input.GetKeyDown(KeyCode.Space))
            {
                verticalVelocity = 10;
            }
        }
        else
        { //the player is in the air, apply gravity
            verticalVelocity -= 14 * Time.deltaTime;
        }
        moveVector = Vector3.zero;
        moveVector.x = Input.GetAxis("Horizontal")*5;
        moveVector.y = verticalVelocity;
        controller.Move(moveVector * Time.deltaTime);
    }
    void LaunchAttack(Collider col)
    {   // or can use an overlapsphere here which is cheaper but may require multiple spheres
        //test against colliders only in the Hitbox layer(now only contain the head and the torso)
        Collider[] cols = Physics.OverlapBox(col.bounds.center, col.bounds.extents, transform.rotation, LayerMask.GetMask("Hitbox")) ;
        foreach(Collider c in cols)
        {
            //check if the collider we hit is ourself 
            if (c.transform.parent.parent == transform)
                continue;
            //only print the collider from enemy  
            Debug.Log(c.name);
        }
    }



}
