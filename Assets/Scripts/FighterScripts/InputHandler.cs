using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    float h, v, lp, rp, lk, rk, cool;
    bool lp_ready, rp_ready, lk_ready, rk_ready;

    JoystickPosition p0, p1, p2;
    float p1_time, p2_time;

    void Awake()
    {
        cam=Camera.main.GetComponent<CameraController>();
        fighter = GetComponent<FighterController>();
        p0 = p1 = p2 = JoystickPosition.CENTER;
        p1_time = p2_time = 0f;
        lp_ready = rp_ready = lk_ready = rk_ready = true;
    }

    void Update()
    {
        GetInput();

        TryDash();
        TryPunch();
        TryKick();

        fighter.Move((fighter.transform.right * h) + (fighter.transform.forward * v));
        cam.Move(v, h);

        UpdateDashTracking();        

        if (cool > 0f) { cool -= Time.deltaTime; }
    }

    void GetInput()
    {
        h = Input.GetAxisRaw("Horizontal");
        v = Input.GetAxisRaw("Vertical");
        lp = Input.GetAxisRaw("LeftPunch");
        rp = Input.GetAxisRaw("RightPunch");
        lk = Input.GetAxisRaw("LeftKick");
        rk = Input.GetAxisRaw("RightKick");

        // do the stuff to figure out if the thing is back down again...
        if (!lp_ready && lp <= 0.001f) lp_ready = true;
        if (!rp_ready && rp <= 0.001f) rp_ready = true;
        if (!lk_ready && lk <= 0.001f) lk_ready = true;
        if (!rk_ready && rk <= 0.001f) rk_ready = true;
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
            (Time.time - p2_time) < dash_window &&
            cool <= 0f)
        {
            switch (p0)
            {
                case JoystickPosition.LEFT:  fighter.DashLeft();     break;
                case JoystickPosition.RIGHT: fighter.DashRight();    break;
                case JoystickPosition.UP:    fighter.DashForward();  break;
                case JoystickPosition.DOWN:  fighter.DashBackward(); break;
            }
            cool = dash_cooldown;
        }
    }

    void UpdateDashTracking()
    {
        if (p0 != p1)
        {
            p2 = p1; p1 = p0; 
            p2_time = p1_time; p1_time = Time.time;
        }
        
        if (cool > 0f) { cool -= Time.deltaTime; }
    }

    void TryPunch()
    {
        // TODO check both
        if (lp >= 0.999f && lp_ready)
        { 
            Debug.Log("LeftPunch"); 
            fighter.LeftPunch();
            lp_ready = false;
        }
        if (rp >= 0.999f && rp_ready) 
        {
            Debug.Log("RightPunch");
            fighter.RightPunch();
            rp_ready = false;
        }
    }

    void TryKick()
    {
        // TODO check both
        if (lk >= 0.999f && lk_ready)
        {
            fighter.LeftKick();
            lk_ready = false;
        }
        if (rk >= 0.999f && rk_ready)
        {
            fighter.RightKick();
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


};
