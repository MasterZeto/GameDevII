using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmeraldChild : MonoBehaviour
{
    GameObject player;
    float speed = 17f;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        transform.LookAt(player.transform);
    }


    void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
    }
}
