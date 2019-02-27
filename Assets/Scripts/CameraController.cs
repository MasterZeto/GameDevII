using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Transform follow;
    [SerializeField] float lookAtHeight;
    [Range(0,1)]
    [SerializeField] float speed;
    [SerializeField] float moveSpeed = 10;
    public bool pause = false;
    public bool moveable = false;

    Vector3 offset;

    void Start()
    {
        offset = transform.position - follow.position;
    }

    void Update()
    {
        if(!pause){
            transform.position = Vector3.Lerp(transform.position, follow.position + (follow.forward * offset.z)
                 + (Vector3.up * offset.y), speed);
            transform.LookAt(follow.position + (lookAtHeight * Vector3.up), Vector3.up);
        }
    }
    public void Move(float vert, float hori){
        if(moveable){
            Vector3 forward=transform.forward;
            forward.y=0;
            forward.Normalize();
            transform.position+=forward*vert*Time.unscaledDeltaTime*moveSpeed;
            transform.eulerAngles-=new Vector3(0,1,0)*Time.unscaledDeltaTime*moveSpeed*hori;
        }
    }
    //used to go back to the normal position before doing cinematic type stuff
    public void GoBack(){
        transform.position = Vector3.Lerp(transform.position, follow.position + (follow.forward * offset.z)
                 + (Vector3.up * offset.y), speed);
            transform.LookAt(follow.position + (lookAtHeight * Vector3.up), Vector3.up);
    }
    public Vector3 NewPos(){
        return follow.position + (follow.forward * offset.z)
                 + (Vector3.up * offset.y);
    }
}
