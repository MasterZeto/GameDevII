using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EmeraldMoveTrack : MonoBehaviour
{
    GameObject child;
    float RotationSpeed = 100;
    float time = 0;
    int childCount = 0;

    void Start()
    {
        child = Resources.Load("LaserBullet") as GameObject;
    }

   //inject a laser buttle , rotate a bit and repeate this process
    void Update()
    {
        time += Time.deltaTime;
        transform.Rotate(Vector3.up * (RotationSpeed * Time.deltaTime));
        if (time > 1f&&childCount<=3)
        {
            time = 0;
            childCount++;
            StartCoroutine("LaserBullet");
        }

        if (childCount > 3)
        { Destroy(gameObject); }
    }


    IEnumerator LaserBullet()
    {
        Instantiate(child, transform.position, Quaternion.identity);
        yield return null;
    }


}
