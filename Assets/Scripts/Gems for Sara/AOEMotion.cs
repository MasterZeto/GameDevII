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


    GameObject player;
    GameObject opponent;
    FighterController fighter;
    GameObject AOE;

    float force = 100f;

    float deathTimer;
    float timer;
    Rigidbody rb;
    Vector3 direction;

    //ParticleSystem[] particles = new ParticleSystem[3];


    void Awake()
    {
        lr = GetComponent<LineRenderer>();
        g = Mathf.Abs(Physics.gravity.y);
    }


    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        opponent = GameObject.FindGameObjectWithTag("Opponent");
        fighter = opponent.GetComponent<FighterController>();
     //   transform.forward = opponent.transform.forward;
      //  transform.up = opponent.transform.up;
        //the angle is set to be 45 degree
        direction = Vector3.Lerp(transform.forward, transform.up, 0.1f);
    
        rb = GetComponent<Rigidbody>(); ;
        rb.AddForce(direction * force, ForceMode.Force);
        deathTimer = 300f;
        timer = 0;

        RendererArc();
        AOE = Resources.Load("AOE") as GameObject;




        // transform.LookAt(player.transform);
        //  z = transform.position.z;


    }


    void Update()
    {
        
        timer += Time.deltaTime;

         if (timer > deathTimer)
        {
            Destroy(gameObject);
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
        
        radianAngle = Mathf.Deg2Rad * 45;
        //i think the velocity here need to be changed
        Debug.Log("velocity is :" + rb.velocity.magnitude);
        float maxDistance = (rb.velocity.magnitude * rb.velocity.magnitude * Mathf.Sin(2 * radianAngle) )/ g;
        for (int i=0; i<=resolution;i++)
        {
            float t = (float)i / (float)resolution;
            arcArray[i] = CalculateArcPoint(t, maxDistance);
         
        }

        return arcArray;
   
    }

    Vector3 CalculateArcPoint(float t, float maxDistance)
    {
        float z = t * maxDistance;
        float y = transform.position.y + z * Mathf.Tan(radianAngle) - ((g * z * z) / (2 * rb.velocity.magnitude * rb.velocity.magnitude * Mathf.Cos(radianAngle) * Mathf.Cos(radianAngle)));
        float x = transform.position.x;
        return new Vector3(x, y, z);

    }
    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.tag == "Player")
            Destroy(gameObject);
    }
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Base")
        //instantiate particle
        {
            Debug.Log("AOE gem hit the ground");

          
            Instantiate(AOE, transform.position, Quaternion.identity);
       
            Destroy(gameObject);
        }
    }
    void Rotate(Vector3 point, Vector3 axis, float angle)
    {
        transform.RotateAround(point, axis, angle);

    }
}


