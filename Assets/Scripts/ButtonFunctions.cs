﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class ButtonFunctions : MonoBehaviour
{
    public void Switch()
    {
        SceneManager.LoadScene("MechInstanceTest");
    }
}
