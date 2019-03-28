using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmeraldMoveTrack : MonoBehaviour
{
    GameObject child;
    float RotationSpeed = 100;

    void Start()
    {
        child = Resources.Load("LaserBullet") as GameObject;
    }

   //inject a laser buttle , rotate a bit and repeate this process
   //rotate 30 degree, inject a buttle 
    void Update()
    {
        transform.Rotate(Vector3.up * (RotationSpeed * Time.deltaTime));
      //  Instantiate(child, transform.position, Quaternion.identity);
    }
}
