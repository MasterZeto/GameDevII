using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuGamepad : MonoBehaviour
{
    [SerializeField] Button new_game_button;
    [SerializeField] Button quit_game_button;

    void Update()
    {
        if (Input.GetAxisRaw("Submit") >= 0.999f) new_game_button.onClick.Invoke();
        if (Input.GetAxisRaw("Cancel") >= 0.999f) quit_game_button.onClick.Invoke();
    }
}
