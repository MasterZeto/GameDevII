using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    [SerializeField] float rate;

    void Update()
    {
        transform.Rotate(0f, rate * Time.deltaTime, 0f);
    }
}
