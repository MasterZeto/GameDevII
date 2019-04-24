using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BracketGamepad : MonoBehaviour
{
    [SerializeField] Button button;
    
    void Update()
    {
        if (Input.GetAxisRaw("Submit") >= 0.999f && button.IsActive())
            button.onClick.Invoke();
    }
}
