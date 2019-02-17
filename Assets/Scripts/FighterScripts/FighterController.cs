using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class FighterController : MonoBehaviour
{
    /* Inspector Accessible Parameters */
    [SerializeField] float move_speed;

    /* Private Member Components */
    CharacterController character;
    [SerializeField] Animator animator;

    [Space]
    [Header("Actions")]
    /* Dash Actions */
    [SerializeField] Action dash_left;
    [SerializeField] Action dash_right;
    [SerializeField] Action dash_forward;
    [SerializeField] Action dash_backward;

    /* Punch Actions */
    [SerializeField] Action left_punch;
    [SerializeField] Action right_punch;
    Action left_right_punch;

    /* Kick Actions */
    [SerializeField] Action left_kick;
    [SerializeField] Action right_kick;
    Action left_right_kick;

    Action current_action;

    void Awake()
    {
        character = GetComponent<CharacterController>();
        current_action = null;
    }

    void Update()
    {
        if (current_action != null && current_action.IsDone())
        {
            current_action = null;
        }
    }

    public void Move(Vector3 direction) 
    {
        if (current_action == null || current_action.IsDone())
        {
            UnsafeMove(direction * move_speed);
        }
    } 

    public void UnsafeMove(Vector3 velocity)
    {
        character.SimpleMove(velocity);
        transform.forward = Vector3.ProjectOnPlane(
            -transform.position, 
            Vector3.up
        );
        SetBlend("Speed", character.velocity.magnitude);
    }

    void StartAction(Action action)
    {
        current_action = action;
        current_action.StartAction(this);
    }

    /* Dash Functions */
    public void DashLeft()     { StartAction(dash_left);     }
    public void DashRight()    { StartAction(dash_right);    }
    public void DashForward()  { StartAction(dash_forward);  }
    public void DashBackward() { StartAction(dash_backward); }

    /* Punch Functions */
    public void LeftPunch()      { StartAction(left_punch);       }
    public void RightPunch()     { StartAction(right_punch);      }
    public void LeftRightPunch() { StartAction(left_right_punch); }

    /* Kick Functions */
    public void LeftKick()      { StartAction(left_kick);       }
    public void RightKick()     { StartAction(right_kick);      }
    public void LeftRightKick() { StartAction(left_right_kick); }

    public void SetTrigger(string trigger) { animator.SetTrigger(trigger); }
    public void SetBlend(string name, float blend) 
    { 
        animator.SetFloat(name, Mathf.Clamp01(blend)); 
    }
}
