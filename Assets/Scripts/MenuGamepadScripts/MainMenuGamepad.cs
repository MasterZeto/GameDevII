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
        new_game_button.Select();
        quit_game_button.Select();
    }
}
