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

public class InputHandler : MonoBehaviour
{
    [SerializeField] float dash_window;
    [SerializeField] float dash_deadzone;
    [SerializeField] float dash_cooldown;

    FighterController fighter;

    float h, v, cool;

    JoystickPosition p0, p1, p2;
    float p1_time, p2_time;

    void Awake()
    {
        fighter = GetComponent<FighterController>();
        p0 = p1 = p2 = JoystickPosition.CENTER;
        p1_time = p2_time = 0f;
    }

    void Update()
    {
        h = Input.GetAxisRaw("Horizontal");
        v = Input.GetAxisRaw("Vertical");

        TryDash(h, v);
        TryPunch();
        TryKick();

        fighter.Move((fighter.transform.right * h) + (fighter.transform.forward * v));

        UpdateDashTracking();        

        if (cool > 0f) { cool -= Time.deltaTime; }
    }

    void TryDash(float h, float v)
    {
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
        if (Input.GetAxisRaw("LeftPunch") > 0.9f)  Debug.Log("LeftPunch");
        if (Input.GetAxisRaw("RightPunch") > 0.9f) Debug.Log("RightPunch");
    }

    void TryKick()
    {
        if (Input.GetAxisRaw("LeftKick") > 0.9f)  Debug.Log("LeftKick");
        if (Input.GetAxisRaw("RightKick") > 0.9f) Debug.Log("RightKick");
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
