﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//[RequireComponent(typeof(LineRenderer))]
public class AOEMotion : MonoBehaviour
{   //trail prediction support
    LineRenderer lr;
    public float velocity;
    public float angle;
    public int resolution = 20;
    float g;
    float radianAngle;
    Vector3[] arcArray = new Vector3[21];
    float time = 0f;
    float speed = 0.005f;



    GameObject player;
    GameObject opponent;
    FighterController fighter;
    GameObject AOE;

  //  float force = 50f;

  

   // Rigidbody rb;
   // Vector3 direction;

    //ParticleSystem[] particles = new ParticleSystem[3];


  


    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        opponent = GameObject.FindGameObjectWithTag("Opponent");
        fighter = opponent.GetComponent<FighterController>();
        lr = GetComponent<LineRenderer>();
        g = Mathf.Abs(Physics.gravity.y);
        transform.LookAt(player.transform);
        radianAngle = Mathf.Deg2Rad * 45;
    
        //   transform.forward = opponent.transform.forward;
        //  transform.up = opponent.transform.up;
        //the angle is set to be 45 degree
        //   direction = Vector3.Lerp(transform.forward, transform.up, 0.7f);
        //   rb = GetComponent<Rigidbody>(); ;
        //  rb.AddForce(-direction * force, ForceMode.Impulse);



        RendererArc();
        AOE = Resources.Load("AOE") as GameObject;




        // transform.LookAt(player.transform);
        //  z = transform.position.z;


    }


    void Update()
    {
        if (fighter.pause != true)
        {
            // lr.positionCount = 0;


            time += 30 * Time.deltaTime;
            float z = transform.position.z -time * speed;

            //  float y = transform.position.y + z * Mathf.Tan(radianAngle) - ((g * z * z) / (2 *30f *30f * Mathf.Cos(radianAngle) * Mathf.Cos(radianAngle)));
            float y = transform.position.y + time*speed * Mathf.Tan(radianAngle) - ((g * time*speed * time*speed) / (2 *2f* Mathf.Cos(radianAngle) * Mathf.Cos(radianAngle)));
            float x = transform.position.x;
            transform.position = new Vector3(x, y, z);
        }

        //  transform.position += transform.forward * speed * Time.deltaTime;
        //Rotates the transform about axis passing through point in world coordinates by angle degrees.
        //This modifies both the position and the rotation of the transform.
        //speed of rotation
        //transform.RotateAround(opponent.transform.position, player.transform.position - opponent.transform.position, 650 * Time.deltaTime);


    }
    void RendererArc()
    {
        lr.positionCount = resolution + 1;
        lr.SetPositions(CalculateArcArray());


    }

    Vector3[] CalculateArcArray()
    {
        
       // radianAngle = Mathf.Deg2Rad * 45;
        //i think the velocity here need to be changed
    //    Debug.Log("velocity is :" + rb.velocity.magnitude);
        //300f here is the velocity
        float maxDistance = (20f*10f*Mathf.Sin(2 * radianAngle) )/ g;
        for (int i=0; i<=resolution;i++)
        {
            float t = (float)i / (float)resolution;
            arcArray[i] = CalculateArcPoint(t, maxDistance);
         
        }

        return arcArray;
   
    }

    Vector3 CalculateArcPoint(float t, float maxDistance)
    {
        float z = transform.position.z - t * maxDistance;

        //  float y = transform.position.y + z * Mathf.Tan(radianAngle) - ((g * z * z) / (2 *30f *30f * Mathf.Cos(radianAngle) * Mathf.Cos(radianAngle)));
        float y = transform.position.y + t*maxDistance* Mathf.Tan(radianAngle) - ((g * t*maxDistance * t*maxDistance) / (2 * 20f * 10f * Mathf.Cos(radianAngle) * Mathf.Cos(radianAngle)));
        float x = transform.position.x;
        return new Vector3(x, y, z);

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Base"||other.gameObject.tag =="Player")
        //instantiate particle
        {
            Debug.Log("AOE gem hit the ground or the player");

          
            Instantiate(AOE, transform.position, Quaternion.identity);
       
            Destroy(gameObject);
        }
    }


}


