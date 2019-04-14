using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AOEMotion : MonoBehaviour
{

    GameObject player;
    GameObject opponent;

    float force = 10f;

    float deathTimer;
    float timer;
    Rigidbody rb;
    Vector3 direction;




    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        opponent = GameObject.FindGameObjectWithTag("Opponent");
        direction = Vector3.Lerp(opponent.transform.forward, opponent.transform.up, 0.5f);
        rb = GetComponent<Rigidbody>(); ;
        rb.AddForce(direction * force, ForceMode.Impulse);
        deathTimer = 3f;
        timer = 0;
       
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

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.tag == "Player")
            Destroy(gameObject);
    }
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag != "Player")
            Destroy(gameObject);
    }
    void Rotate(Vector3 point, Vector3 axis, float angle)
    {
        transform.RotateAround(point, axis, angle);

    }
}


