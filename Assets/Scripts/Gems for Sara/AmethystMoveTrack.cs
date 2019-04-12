﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmethystMoveTrack : MonoBehaviour
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
        //Vector3 playerPos= new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z);
        transform.LookAt(player.transform);

    }
    void Update()
    {
        timer += Time.deltaTime;
        transform.position += transform.forward * speed * Time.deltaTime;
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

