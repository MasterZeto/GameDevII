using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Transform follow;
    [SerializeField] float lookAtHeight;

    Vector3 offset;

    void Start()
    {
        offset = transform.position - follow.position;
    }

    void Update()
    {
        transform.position = follow.position + (follow.forward * offset.z)
             + (Vector3.up * offset.y);
        transform.LookAt(follow.position + (lookAtHeight * Vector3.up), Vector3.up);
    }
}
