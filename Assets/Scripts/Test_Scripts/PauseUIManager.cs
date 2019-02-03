using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
namespace CommandPattern
{
    

public class PauseUIManager : MonoBehaviour
{
    //change this to change dynamically, ex. with the customizations once they are implemented
    public Button[] bttns;
    //To Do: change this to a dictionary, and have addToQueue in the PauseScript add different commands based on the value
    private Dictionary<Button, int> queueButtons;
    private Vector3 startPos, currentPos;
    private Transform parent;
    //change dummyPauseScript to just PauseScript later once it is more fleshed out
    public GameObject pauseManager;
    private DummyPauseScript PauseScript;
    private int currentIndex=0;
    // Start is called before the first frame update
    void Start()
    {
        queueButtons=new Dictionary<Button, int>();
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
        queueButtons.Add(newBttn,currentIndex);
        currentIndex++;
        rect.transform.SetParent(parent);
        rect.transform.position=currentPos;
        currentPos.x+=rect.sizeDelta.x;
        newBttn.onClick.AddListener(() => DeleteButton(newBttn));
        int index=System.Array.IndexOf(bttns,bttn);
        PauseScript.addToQueue(index);
    }
    void DeleteButton(Button bttn){
        //To Do: have spawned buttons after the removed one slide down, and update currentPos.x
        //To Do: find the command in the pauseQueue and remove it
        PauseScript.pauseQueue.RemoveAt(queueButtons[bttn]);
        int index=queueButtons[bttn];
        currentIndex--;
        queueButtons.Remove(bttn);
        Destroy(bttn.gameObject);
        List<Button> tempBttns=new List<Button>(queueButtons.Keys);
        foreach(Button otherBttn in tempBttns){
            if(queueButtons[otherBttn]>index){
               queueButtons[otherBttn]--;
            }
        }
    }
    public void SetUp(){
        currentIndex=0;
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
        foreach(var bttn in queueButtons){
            //To do: change so that it doesn't destroy itself until the action is done
            Destroy(bttn.Key.gameObject);
        }
    }
}
}
