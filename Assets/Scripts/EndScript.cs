using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

public class EndScript : MonoBehaviour
{
    public Button bttn;
    // Start is called before the first frame update
    void Start()
    {
        bttn.onClick.AddListener(() => PlayAgain());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void PlayAgain(){
        SceneManager.LoadScene("FirstPlayable");
    }
}
