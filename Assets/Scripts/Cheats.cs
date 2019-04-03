using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Cheats : MonoBehaviour
{
    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) Application.Quit();

        if (Input.GetKeyDown(KeyCode.Alpha1)) { SceneManager.LoadScene("MainMenu"); }
        if (Input.GetKeyDown(KeyCode.Alpha2)) { SceneManager.LoadScene("SawyerBracketScene"); }
        if (Input.GetKeyDown(KeyCode.Alpha3)) { SceneManager.LoadScene("SawyerFight"); }
        if (Input.GetKeyDown(KeyCode.Alpha4)) { SceneManager.LoadScene("SaraBracketScene"); }
        if (Input.GetKeyDown(KeyCode.Alpha5)) { SceneManager.LoadScene("SaraOHara"); }
        if (Input.GetKeyDown(KeyCode.Alpha6)) { SceneManager.LoadScene("BajaBracketScene"); }
        if (Input.GetKeyDown(KeyCode.Alpha7)) { SceneManager.LoadScene("BahaTest"); }
        if (Input.GetKeyDown(KeyCode.Alpha8)) { SceneManager.LoadScene("EndCutscene"); }      
    }
}
