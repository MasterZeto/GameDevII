using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class FighterController : MonoBehaviour
{
    /* Inspector Accessible Parameters */
    [SerializeField] float move_speed;
    [SerializeField] public float max_heat;
    [SerializeField] float heat_dissipation_rate;

    [Space]
    /* Private Member Components */
    CharacterController character;
    [SerializeField] Animator animator;
    [SerializeField] Transform opponent;
    [SerializeField] GameObject opponent_object;

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

    public float heat { get; private set; }

    void Awake()
    {
        character = GetComponent<CharacterController>();
        current_action = null;
        heat = 0;
    }

    void Update()
    {
        if (current_action != null && current_action.IsDone())
        {
            current_action = null;
        }
        if (current_action == null)
        {
            heat = Mathf.Clamp(heat - (heat_dissipation_rate * Time.deltaTime), 0, max_heat);
        }
        SetBlend("Speed", character.velocity.magnitude);
    }

    public void Move(Vector3 direction) 
    {
        if (current_action == null || current_action.IsDone())
        {
            UnsafeMove(direction);
        }
    } 

    // so here do a Vector3.up, right left, etc, and it'll make it relative
    public Vector3 RelativeMove(Vector3 direction)
    {
        Vector3 moveDirection = (transform.right * direction.x) +
           (transform.up * direction.y) +
           (transform.forward * direction.z);
           Move(moveDirection);
           return moveDirection;
    }

    public void UnsafeMove(Vector3 velocity)
    {
        character.SimpleMove(velocity);
        transform.forward = Vector3.ProjectOnPlane(
            opponent.position - transform.position, 
            Vector3.up
        );
    }

    void StartAction(Action action)
    {
        if (max_heat - heat > action.GetHeat() && current_action == null)
        {
            current_action = action;
            current_action.StartAction(this);
            heat += current_action.GetHeat();
        }
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

    public void SetTrigger(string trigger) 
    { 
        animator.SetTrigger(trigger); 
    }

    public void SetBlend(string name, float blend) 
    { 
        animator.SetFloat(name, Mathf.Clamp01(blend)); 
    }

    public void Pause()
    {
        if (current_action != null) current_action.Pause();
        animator.enabled = false;
    }

    public void Resume()
    {
        if (current_action != null) current_action.Resume();
        animator.enabled = true;
    }
    public GameObject GetOpponent()
    {
        return opponent_object;
    }
}
