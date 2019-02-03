using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
namespace CommandPattern
{
    

public class PauseUIManager : MonoBehaviour
{
    public Button[] bttns;
    //To Do: change this to a dictionary, and have addToQueue in the PauseScript add different commands based on the value
    private List<Button> queueButtons;
    private Vector3 startPos, currentPos;
    private Transform parent;
    //change dummyPauseScript to just PauseScript later once it is more fleshed out
    public GameObject pauseManager;
    private DummyPauseScript PauseScript;
    // Start is called before the first frame update
    void Start()
    {
        queueButtons=new List<Button>();
        PauseScript=pauseManager.GetComponent<DummyPauseScript>();
        foreach (Button bttn in bttns)
        {
            bttn.onClick.AddListener(() => AddToQueue(bttn));
        }
        startPos=new Vector3(0,0,0);
        currentPos=startPos;
        parent=bttns[0].GetComponent<RectTransform>().transform.parent;
        Hide();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void AddToQueue(Button bttn){
        Button newBttn=Instantiate(bttn);
        RectTransform rect=newBttn.GetComponent<RectTransform>();
        rect.transform.SetParent(parent);
        rect.transform.position=currentPos;
        currentPos.x+=rect.sizeDelta.x;
        newBttn.onClick.AddListener(() => DeleteButton(newBttn));
        PauseScript.addToQueue();
    }
    void DeleteButton(Button bttn){
        //To Do: have spawned buttons after the removed one slide down, and update currentPos.x
        //To Do: find the command in the pauseQueue and remove it
        queueButtons.Remove(bttn);
        Destroy(bttn.gameObject);
    }
    public void SetUp(){
        queueButtons.Clear();
        currentPos=startPos;
        foreach(Button bttn in bttns){
            bttn.gameObject.SetActive(true);
        }
    }
    public void Hide(){
        foreach(Button bttn in bttns){
            bttn.gameObject.SetActive(false);
        }
        foreach(Button bttn in queueButtons){
            //To do: change so that it doesn't destroy itself until the action is done
            Destroy(bttn.gameObject);
        }
    }
}
}
