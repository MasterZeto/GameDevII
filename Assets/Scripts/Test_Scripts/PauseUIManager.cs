using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
namespace CommandPattern
{  

    public class PauseUIManager : MonoBehaviour
    {
        public Button bttnPrefab;
        public int maxQueued = 5;
        public Canvas UIparent;
        //change this to change dynamically, ex. with the customizations once they are implemented, might want it to be a list
        public List<Button> bttns;
        //To Do: change this to a dictionary, and have addToQueue in the PauseScript add different commands based on the value
        private Dictionary<Button, int> queueButtons;
        private Dictionary<Button, RectTransform> buttonLoc;
        private Vector3 currentPos, startPos;
        private Transform parent;
        //change dummyPauseScript to just PauseScript later once it is more fleshed out
        public GameObject pauseManager;
        private DummyPauseScript PauseScript;
        private int currentIndex=0;
        // Start is called before the first frame update
        void Start()
        {
            queueButtons = new Dictionary<Button, int>();
            buttonLoc = new Dictionary<Button, RectTransform>();
            PauseScript = pauseManager.GetComponent<DummyPauseScript>();
            for(int i = 0; i < PauseScript.numOfPossComs; i++){
                Button newBttn = Instantiate(bttnPrefab);
                RectTransform rect = newBttn.GetComponent<RectTransform>();
                rect.transform.SetParent(UIparent.transform);
                startPos.x = Screen.width/2-PauseScript.numOfPossComs/2*rect.sizeDelta.x+i*rect.sizeDelta.x;
                startPos.y = 4*rect.sizeDelta.y;
                startPos.z = 0;
                rect.transform.position=startPos;
                newBttn.GetComponentInChildren<Text>().text = "Action" + (i + 1);
                bttns.Add(newBttn);
            }
            foreach (Button bttn in bttns)
            {
                bttn.onClick.AddListener(() => AddToQueue(bttn));
            }
            startPos.x = Screen.width/2-maxQueued/2*bttns[0].GetComponent<RectTransform>().sizeDelta.x;
            startPos.y = 2*bttns[0].GetComponent<RectTransform>().sizeDelta.y;
            startPos.z = 0;
            currentPos = startPos;
            parent = bttns[0].GetComponent<RectTransform>().transform.parent;
            Hide();
        }

        // Update is called once per frame
        void Update()
        {
            
        }
        void AddToQueue(Button bttn){
            if(PauseScript.pauseQueue.Count<maxQueued){
                Button newBttn = Instantiate(bttn);
                RectTransform rect = newBttn.GetComponent<RectTransform>();
                queueButtons.Add(newBttn,currentIndex);
                buttonLoc.Add(newBttn,rect);
                currentIndex++;
                rect.transform.SetParent(parent);
                rect.transform.position = currentPos;
                currentPos.x += rect.sizeDelta.x;
                newBttn.onClick.AddListener(() => DeleteButton(newBttn));
                int index=bttns.IndexOf(bttn);
                PauseScript.addToQueue(index);
            }
        }
        void DeleteButton(Button bttn){
            //To Do: have spawned buttons after the removed one slide down, and update currentPos.x
            //To Do: find the command in the pauseQueue and remove it
            PauseScript.pauseQueue.RemoveAt(queueButtons[bttn]);
            int index = queueButtons[bttn];
            currentIndex--;
            queueButtons.Remove(bttn);
        
            List<Button> tempBttns = new List<Button>(queueButtons.Keys);
            foreach(Button otherBttn in tempBttns){
                if(queueButtons[otherBttn]>index){
                    queueButtons[otherBttn]--;
                    Vector3 newLoc = buttonLoc[otherBttn].transform.position;
                    newLoc.x -= buttonLoc[bttn].sizeDelta.x;
                    buttonLoc[otherBttn].transform.position=newLoc;
                }
            }
            currentPos.x -= buttonLoc[bttn].sizeDelta.x;
            Destroy(bttn.gameObject);
        }
        public void SetUp(){
            currentIndex = 0;
            queueButtons.Clear();
            currentPos = startPos;
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
                if(bttn.Key.gameObject != null){
                    //Destroy(bttn.Key.interactable = false);
                }
            }
        }
        public void updateQueueButtons(){
            if(queueButtons.Count==0){
                return;
            }
            Button usedBttn = null;
            List<Button> tempBttns;
            tempBttns = new List<Button>(queueButtons.Keys);
            foreach(Button bttn in tempBttns){
                if(queueButtons[bttn] == 0){
                    usedBttn = bttn;
                }
            }
            if(usedBttn!=null){
                queueButtons.Remove(usedBttn);
            }
            tempBttns = new List<Button>(queueButtons.Keys);
            foreach(Button otherBttn in tempBttns){
                queueButtons[otherBttn]--;
                Vector3 newLoc = buttonLoc[otherBttn].transform.position;
                newLoc.x -= buttonLoc[otherBttn].sizeDelta.x;
                buttonLoc[otherBttn].transform.position=newLoc;
            }
            Destroy(usedBttn.gameObject);
        }
    }
}
