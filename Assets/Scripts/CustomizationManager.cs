using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Add in a static way of setting these instance values, that way we can
   communicate with this from various places when we instantiate so we can
   talk to it from the PartSelectors and link up those two scenes finally
 */
public class CustomizationManager : MonoBehaviour
{
    public static int left_arm_part
    {
        get { return instance.left_arm_part_intern; }
        set { instance.left_arm_part_intern = value; }
    }

    public static int left_leg_part
    {
        get { return instance.left_leg_part_intern; }
        set { instance.left_leg_part_intern = value; }
    }

    public static int right_arm_part
    {
        get { return instance.right_arm_part_intern; }
        set { instance.right_arm_part_intern = value; }
    }

    public static int right_leg_part
    {
        get { return instance.right_leg_part_intern; }
        set { instance.right_leg_part_intern = value; }
    }

    [SerializeField] [Range(0, PartTypes.NUM_TYPES-1)] int left_arm_part_intern;
    [SerializeField] [Range(0, PartTypes.NUM_TYPES-1)] int left_leg_part_intern;
    [SerializeField] [Range(0, PartTypes.NUM_TYPES-1)] int right_arm_part_intern;
    [SerializeField] [Range(0, PartTypes.NUM_TYPES-1)] int right_leg_part_intern;

    static CustomizationManager instance = null;

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        if (instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }
}
