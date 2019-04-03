using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideCameraController : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] Transform opponent;
    [SerializeField] Transform camera_position_target;
    [SerializeField] new Camera camera;

    void Update()
    {
        transform.position = Vector3.Lerp(player.position, opponent.position, 0.5f);
        transform.forward  = player.right;

        float distance = Vector3.Distance(player.position, opponent.position);

        distance *= 1f;

        float camera_distance = (distance / 2f) / Mathf.Tan(camera.fieldOfView * Mathf.PI / 360f);

        Debug.Log(camera.fieldOfView);

        camera_position_target.localPosition = Mathf.Max(camera_distance, 5f) * Vector3.forward;
    }
}
