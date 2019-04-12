using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmeraldChild : MonoBehaviour
{
    GameObject player;
    float speed = 25f;
    float deathTimer;
    float timer;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        deathTimer = 3f;
        timer = 0;
        transform.LookAt(player.transform.position + new Vector3(0, 4.0f, 0));
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
