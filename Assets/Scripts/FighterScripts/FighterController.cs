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

    /* Dash Actions */
    Action dash_left;
    Action dash_right;
    Action dash_forward;
    Action dash_backward;

    /* Punch Actions */
    Action left_punch;
    Action right_punch;
    Action left_right_punch;

    /* Kick Actions */
    Action left_kick;
    Action right_kick;
    Action left_right_kick;

    public void Awake()
    {
        character = GetComponent<CharacterController>();
    }

    public void Move(Vector3 direction) 
    {
        character.SimpleMove(direction.normalized * move_speed);
        transform.forward = Vector3.ProjectOnPlane(-transform.position, Vector3.up);
    } 

    /* Dash Functions */
    public void DashLeft()     { dash_left.Start(this);     }
    public void DashRight()    { dash_right.Start(this);    }
    public void DashForward()  { dash_forward.Start(this);  }
    public void DashBackward() { dash_backward.Start(this); }

    /* Punch Functions */
    public void LeftPunch()      { left_punch.Start(this);       }
    public void RightPunch()     { right_punch.Start(this);      }
    public void LeftRightPunch() { left_right_punch.Start(this); }

    /* Kick Functions */
    public void LeftKick()      { left_kick.Start(this);       }
    public void RightKick()     { right_kick.Start(this);      }
    public void LeftRightKick() { left_right_kick.Start(this); }

}
