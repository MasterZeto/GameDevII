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
    [SerializeField] public Animator animator;
    [SerializeField] Transform opponent;
    [SerializeField] GameObject opponent_object;

    [Space]
    [Header("Actions")]
    /* Dash Actions */
    [SerializeField] Action dash_left;
    [SerializeField] Action dash_right;
    [SerializeField] Action dash_forward;
    [SerializeField] Action dash_backward;
    [SerializeField] Action dash_backward_left;
    [SerializeField] Action dash_backward_right;


    /* Punch Actions */
    [SerializeField] Action left_punch;
    [SerializeField] Action right_punch;
    [SerializeField] Action left_right_punch;

    /* Kick Actions */
    [SerializeField] Action left_kick;
    [SerializeField] Action right_kick;
    [SerializeField] Action left_right_kick;

    Action current_action;

    public float heat { get; private set; }

    public bool pause = false;

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
        if (current_action == null&&!pause&&animator.enabled)
        {
            heat = Mathf.Clamp(heat - (heat_dissipation_rate * Time.deltaTime), 0, max_heat);
        }
        SetBlend("SpeedVertical", Vector3.Dot(character.velocity.normalized, transform.forward));
        SetBlend("SpeedHorizontal", Vector3.Dot(character.velocity.normalized, transform.right));

        //Debug.Log(Vector3.Dot(character.velocity.normalized, transform.forward));
    }

    public void Move(Vector3 direction) 
    {
        if (animator.enabled==true && !pause)
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
        character.SimpleMove(velocity * move_speed);
        transform.forward = Vector3.ProjectOnPlane(
            opponent.position - transform.position, 
            Vector3.up
        );
    }

    public void StartAction(Action action)
    {
        if (max_heat - heat > action.GetHeat() && current_action == null)
        {
            current_action = action;
            current_action.StartAction(this);
            heat += current_action.GetHeat();
        }
    }

    public bool IsActing()
    {
        return (current_action != null);
    }

    /* Dash Functions */
    public void DashLeft()     { StartAction(dash_left);     }
    public void DashRight()    { StartAction(dash_right);    }
    public void DashForward()  { StartAction(dash_forward);  }
    public void DashBackward() { StartAction(dash_backward); }
    public void DashBackLeft() { StartAction(dash_backward_left); }
    public void DashBackRight(){ StartAction(dash_backward_right); }


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
    public void SetBoolTrue(string bool_name)
    {
        animator.SetBool(bool_name,true);
    }
    public void SetBoolFalse(string bool_name)
    {
        animator.SetBool(bool_name, false);
    }
    public void SetBlend(string name, float blend) 
    { 
        animator.SetFloat(name, blend); 
    }

    public void Pause()
    {
        if (current_action != null) current_action.Pause();
        animator.enabled = false;
        pause = true;
    }
    public void Stun(){
        Pause();
        //set stunned animation here?
    }

    public void Resume()
    {
        if (current_action != null) current_action.Resume();
        animator.enabled = true;
        pause = false;
    }
    public GameObject GetOpponent()
    {
        return opponent_object;
    }

    /*hitbox stuff */
    public Collider GetHitbox(ref bool isProjectile){
        isProjectile = false;
        if(current_action!=null&&current_action!=dash_left&&current_action!=dash_right&&current_action!=dash_forward&&current_action!=dash_backward){
            EnemyAttack attack = current_action as EnemyAttack;
            if(attack==null){
                SawyerSwingAttack swingAttack = current_action as SawyerSwingAttack;
                if(swingAttack!=null) return swingAttack.hitbox._collider;
                else{
                    ProjectileAttack project = current_action as ProjectileAttack;
                    if(project!=null){
                        isProjectile = true;
                        return project.hitbox._collider;
                    }
                }
            }
            else{
                return attack.hitbox._collider;
            }
            return null;
            
        }
        return null;
    }
    /* projectile predicting stuff */
    public float GetRemainingTime(){
        ProjectileAttack project = current_action as ProjectileAttack;
        if(project!=null) return project.GetRemainingDuration();
        return -1;
    }
    public float GetHitDuration(){
        ProjectileAttack project = current_action as ProjectileAttack;
        if(project!=null) return project.GetDuration();
        return -1;
    }
    public float GetProjectileSpeed(){
        ProjectileAttack project = current_action as ProjectileAttack;
        if(project!=null) return project.GetSpeed();
        return -1;
    }
    //used to get heat for pause menu heat bar
    public void GetHeatValues(ref List<float> heatVal){
        heatVal.Add(dash_left.GetHeat());
        heatVal.Add(dash_right.GetHeat());
        heatVal.Add(dash_forward.GetHeat());
        heatVal.Add(dash_backward.GetHeat());
        heatVal.Add(left_punch.GetHeat());
        heatVal.Add(right_punch.GetHeat());
        heatVal.Add(left_right_punch.GetHeat());
        heatVal.Add(left_kick.GetHeat());
        heatVal.Add(right_kick.GetHeat());
        heatVal.Add(left_right_kick.GetHeat());
    }

}
