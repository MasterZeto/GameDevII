using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

    public class PauseUIManager : MonoBehaviour
    {
        //public List<Button> bttnPrefabList;
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
        private PauseScript PauseScript;
        private int currentIndex=0;
        [SerializeField] FighterController fc;
        [SerializeField] RectTransform startPosition;
        //probably will change it so it isn't hard coded lmao, but that's for l8r
        List<float> heatVal = new List<float>();
        [SerializeField] Image pause_heat;
        float currentHeat = 0;
        // Start is called before the first frame update
        void Start()
        {
            pause_heat.gameObject.SetActive(false);
            queueButtons = new Dictionary<Button, int>();
            buttonLoc = new Dictionary<Button, RectTransform>();
            PauseScript = pauseManager.GetComponent<PauseScript>();
            fc.GetHeatValues(ref heatVal);
            //foreach(Button bttn in bttns){
                //Button newBttn = Instantiate(bttnPrefabList[i]);
                //RectTransform rect = newBttn.GetComponent<RectTransform>();
                //rect.transform.SetParent(UIparent.transform);
                //startPos.x = Screen.width/2-10/2*rect.sizeDelta.x+i*rect.sizeDelta.x;
                //startPos.y = 4*rect.sizeDelta.y;
                //startPos.z = 0;
                //rect.transform.position=startPos;
                /*Text text=newBttn.GetComponentInChildren<Text>();
                switch(i){
                    case 0:
                        text.text = "Dash Left";
                        break;
                    case 1:
                        text.text = "Dash Right";
                        break;
                    case 2:
                        text.text = "Dash Forward";
                        break;
                    case 3:
                        text.text = "Dash Backward";
                        break;
                    case 4:
                        text.text = "Punch Left";
                        break;
                    case 5:
                        text.text = "Punch Right";
                        break;
                    case 6:
                        text.text = "Punch Left Right";
                        break;
                    case 7:
                        text.text = "Kick Left";
                        break;
                    case 8:
                        text.text = "Kick Right";
                        break;
                    case 9:
                        text.text = "Kick Left Right";
                        break;
                }*/
                //bttns.Add(newBttn);
        //}
            foreach (Button bttn in bttns)
            {
                bttn.onClick.AddListener(() => AddToQueue(bttn));
//                Debug.Log("click added");
            }
            if(startPosition!=null){
                startPos = startPosition.position;
            }
            else{
                startPos.x = Screen.width/2-maxQueued/2*bttns[0].GetComponent<RectTransform>().sizeDelta.x+70;
                startPos.y = .5f*bttns[0].GetComponent<RectTransform>().sizeDelta.y;
                startPos.z = 0;
            }
            currentPos = startPos;
            parent = bttns[0].GetComponent<RectTransform>().transform.parent;
            Hide();
        }

        // Update is called once per frame
        void Update()
        {
            
        }
        public void AddByInput(int i){
            AddToQueue(bttns[i]);
        }
        void AddToQueue(Button bttn){
            if(fc.stunned) return;
            int index=bttns.IndexOf(bttn);
            if(PauseScript.pauseQueue.Count<maxQueued&&currentHeat+heatVal[index]<fc.max_heat){
                Button newBttn = Instantiate(bttn);
                RectTransform rect = newBttn.GetComponent<RectTransform>();
                queueButtons.Add(newBttn,currentIndex);
                buttonLoc.Add(newBttn,rect);
                currentIndex++;
                rect.transform.SetParent(parent);
                rect.transform.position = currentPos;
                currentPos.x += rect.sizeDelta.x;
                newBttn.onClick.AddListener(() => DeleteButton(newBttn));
                PauseScript.addToQueue(index);
                currentHeat+=heatVal[index];
                pause_heat.rectTransform.localScale = new Vector3(
            (fc.max_heat - currentHeat) / fc.max_heat, 1f, 1f
        );
            }
        }
        void DeleteButton(Button bttn){
            //To Do: have spawned buttons after the removed one slide down, and update currentPos.x
            //To Do: find the command in the pauseQueue and remove it
            int index = PauseScript.GetPosComIndex(queueButtons[bttn]);
            currentHeat -= heatVal[index];
            pause_heat.rectTransform.localScale = new Vector3(
            (fc.max_heat - currentHeat) / fc.max_heat, 1f, 1f
        );
            PauseScript.pauseQueue.RemoveAt(queueButtons[bttn]);
            index = queueButtons[bttn];
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
            currentHeat = fc.heat;
            pause_heat.rectTransform.localScale = new Vector3(
            (fc.max_heat - currentHeat) / fc.max_heat, 1f, 1f
        );
            pause_heat.gameObject.SetActive(true);
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
                    bttn.Key.interactable = false;
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
        public void HidePauseHeat(){
            pause_heat.gameObject.SetActive(false);
        }

        public int PauseQueueCount()
        {
            return PauseScript.pauseQueue.Count;
        }

        public int PauseQueueIndex()
        {
            return currentIndex;
        }
    }

