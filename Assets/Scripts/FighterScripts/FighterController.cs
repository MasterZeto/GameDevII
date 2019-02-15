using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* this is probably gonna go away. It's frankly pretty trash. I wanna make
   a system that handles movement, handles attacking, and also enables pausing
   so that we can put this thing on a character and make it happen, also so
   we can pass it into the CommandExecute so we can drive stuff from commands
   in a less jank way...

   So I might want to just like make this a component, and then it gets driven
   with a different controller, so one for input handling on the player, and one
   for driving it from an FSM

   This way we can handle stunning in here...
 */
public class FighterController : MonoBehaviour
{
    Action dash_left;
    Action dash_right;
    Action dash_forward;
    Action dash_back;

    Action left_punch;
    Action right_punch;
    Action both_punch;

    Action left_kick;
    Action right_kick;
    Action both_kick;
}
