using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Giga.AI.Blackboard;

public enum JoystickPosition
{
    CENTER,
    LEFT,
    RIGHT,
    UP,
    DOWN
};

/* see about expanding this kind of "get down" behavior I'm using for the punches and kicks to
   the dash behavior, then I can clean up some of this nasty shit...
 */

public class InputHandler : MonoBehaviour
{
    [SerializeField] float dash_window;
    [SerializeField] float dash_deadzone;
    [SerializeField] float dash_cooldown;

    FighterController fighter;
    CameraController cam;
    [SerializeField] PauseScript pause;

    float h, v, lp, rp, lk, rk, p, cool;
    bool lp_ready, rp_ready, lk_ready, rk_ready, p_ready;

    JoystickPosition p0, p1, p2;
    float p1_time, p2_time;

    [SerializeField] PauseUIManager pauseUI;

    bool control_active;

    void Awake()
    {
        cam=Camera.main.GetComponent<CameraController>();
        fighter = GetComponent<FighterController>();
        p0 = p1 = p2 = JoystickPosition.CENTER;
        p1_time = p2_time = 0f;
        lp_ready = rp_ready = lk_ready = rk_ready = p_ready = true;
        control_active = true;
    }

    void Update()
    {
        if (control_active)
        {
            GetInput();

            if (TryPause()) return;

            TryDash();
            TryPunch();
            TryKick();

            fighter.Move((fighter.transform.right * h) + (fighter.transform.forward * v));
            cam.Move(v, h);

            UpdateDashTracking();        
        }

        if (cool > 0f) { cool -= Time.unscaledDeltaTime; }

        Blackboard.player_position = transform.position;
    }

    void GetInput()
    {
        h = Input.GetAxisRaw("Horizontal");
        v = Input.GetAxisRaw("Vertical");
        lp = Input.GetAxisRaw("LeftPunch");
        rp = Input.GetAxisRaw("RightPunch");
        lk = Input.GetAxisRaw("LeftKick");
        rk = Input.GetAxisRaw("RightKick");
        p = Input.GetAxisRaw("Pause");

        // do the stuff to figure out if the thing is back down again...
        if (!lp_ready && lp <= 0.001f) lp_ready = true;
        if (!rp_ready && rp <= 0.001f) rp_ready = true;
        if (!lk_ready && lk <= 0.001f) lk_ready = true;
        if (!rk_ready && rk <= 0.001f) rk_ready = true;
        if (!p_ready  && p <= 0.001f)  p_ready = true;
    }

    bool TryPause()
    {
        if (p >= 0.999f && p_ready)
        {
            p_ready = false;
            pause.TogglePause();
            return true;
        } 

        return false;
    }

    void TryDash()
    {
        /* this is nasty, try to rework this to be like the stuff I'm doing with the buttons...
           not urgent but worth investigating */
        p0 = GetJoystickPosition(h, v);

        if (p0 != p1 &&
            p0 == p2 && 
            p0 != JoystickPosition.CENTER && 
            p1 == JoystickPosition.CENTER &&
            (Time.unscaledTime - p2_time) < dash_window &&
            cool <= 0f &&!fighter.stunned)
        {
            if(!cam.pause){
                switch (p0)
                {
                    case JoystickPosition.LEFT:  fighter.DashLeft();     break;
                    case JoystickPosition.RIGHT: fighter.DashRight();    break;
                    case JoystickPosition.UP:    fighter.DashForward();  break;
                    case JoystickPosition.DOWN:  fighter.DashBackward(); break;
                }
            }
            else{
                switch (p0)
                {
                    case JoystickPosition.LEFT:  pauseUI.AddByInput(0); break;
                    case JoystickPosition.RIGHT: pauseUI.AddByInput(1); break;
                    case JoystickPosition.UP:    pauseUI.AddByInput(2); break;
                    case JoystickPosition.DOWN:  pauseUI.AddByInput(3); break;
                }
            }
            cool = dash_cooldown;
        }
    }

    void UpdateDashTracking()
    {
        if (p0 != p1)
        {
            p2 = p1; p1 = p0; 
            p2_time = p1_time; p1_time = Time.unscaledTime;
        }
        
        if (cool > 0f) { cool -= Time.unscaledDeltaTime; }
    }

    void TryPunch()
    {
        // TODO check both
        if (lp >= 0.999f && lp_ready && rp >= 0.999 && rp_ready)
        {
            if(!cam.pause){
                fighter.LeftRightPunch();
            }
            else{
                pauseUI.AddByInput(6);
            }
            lp_ready = false;
            rp_ready = false;
        }
        if (lp >= 0.999f && lp_ready)
        { 
            if(!cam.pause){
                fighter.LeftPunch();
            }
            else{
                pauseUI.AddByInput(4);
            }
            lp_ready = false;
        }
        if (rp >= 0.999f && rp_ready) 
        {
 //           Debug.Log("RightPunch");
            if(!cam.pause){
                fighter.RightPunch();
            }
            else{
                pauseUI.AddByInput(5);
            }
            rp_ready = false;
        }
    }

    void TryKick()
    {
        // TODO check both
        if (lk >= 0.999f && lk_ready && rk >= 0.999 && rk_ready)
        {
            if(!cam.pause){
                fighter.LeftRightKick();
            }
            else{
                pauseUI.AddByInput(9);
            }
            lp_ready = false;
            rp_ready = false;
        }
        if (lk >= 0.999f && lk_ready)
        {
            if(!cam.pause){
                fighter.LeftKick();
            }
            else{
                pauseUI.AddByInput(7);
            }
            lk_ready = false;
        }
        if (rk >= 0.999f && rk_ready)
        {
            if(!cam.pause){
                fighter.RightKick();
            }
            else{
                pauseUI.AddByInput(8);
            }
            rk_ready = false;
        }
    }

    JoystickPosition GetJoystickPosition(float h, float v)
    {
        if (h > dash_deadzone && Mathf.Abs(v) <= dash_deadzone) return JoystickPosition.RIGHT;
        if (h < -dash_deadzone && Mathf.Abs(v) <= dash_deadzone) return JoystickPosition.LEFT;
        if (v > dash_deadzone && Mathf.Abs(h) <= dash_deadzone) return JoystickPosition.UP;
        if (v < -dash_deadzone && Mathf.Abs(h) <= dash_deadzone) return JoystickPosition.DOWN;
        return JoystickPosition.CENTER;
    }

    public void SetControlActive(bool active)
    {
        control_active = active;
    }
};
