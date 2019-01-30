using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PauseUIManager : MonoBehaviour
{
    public Button[] bttns;
    private Vector3 startPos, currentPos;
    // Start is called before the first frame update
    void Start()
    {
        foreach (Button bttn in bttns)
        {
            bttn.onClick.AddListener(() => addToQueue(bttn));
        }
        startPos=new Vector3(0,0,0);
        currentPos=startPos;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void addToQueue(Button bttn){
        //need to fix instantiating buttons, which will be added to another list and removed when clicked
        //Button newBttn=Instantiate(bttn);
        //newBttn.gameObject.transform.position=currentPos;
        //currentPos.x+=newBttn.GetComponent<RectTransform>().sizeDelta.x;
    }
}
