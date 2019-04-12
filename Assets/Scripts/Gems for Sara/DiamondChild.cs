using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiamondChild : MonoBehaviour
{
    GameObject player;
    float speed = 17f;
    float deathTimer;
    float timer;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        deathTimer = 3f;
        timer = 0;
        //  transform.LookAt(player.transform);
    }


    void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
        timer += Time.deltaTime;
        if (timer > deathTimer)
        {
            Destroy(gameObject);
        }
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
}
