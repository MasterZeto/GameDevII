using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(LineRenderer))]
public class PearlRenderer : MonoBehaviour
{
    LineRenderer lr;

    public int resolution;

    private void Awake()
    {
        lr = GetComponent<LineRenderer>();

    }


    void Start()
    {
        
    }

    void Renderer()
    {

        lr.positionCount=resolution+1;


    }




    // Update is called once per frame
    void Update()
    {
        
    }
}
