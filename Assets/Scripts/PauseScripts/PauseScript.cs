using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseScript : MonoBehaviour
{
    //main script dealing w/ the pause stuff
    //replace fightercontroller here for w/e is responsible for pausing the enemy
    [SerializeField] FighterController enemy;
    [SerializeField] PauseUIManager UIManager;
    [SerializeField] GameObject predictor;
    [SerializeField] List<Camera> actionCams;
    public List<voidDelegate> pauseQueue;
    private List<voidDelegate> possibleComs;
    FighterController playerActions;
    Camera mainCam;
    CameraController camCon;
    Vector3 camOrigPos;
    bool pause = false;
    bool executing = false;
    bool up = true;
    Collider col;
    // Start is called before the first frame update
    void Start()
    {
        UIManager = GetComponent<PauseUIManager>();
        camCon = Camera.main.GetComponent<CameraController>();
        pauseQueue = new List<voidDelegate>();
        possibleComs = new List<voidDelegate>();
        possibleComs.Add(() => playerActions.DashLeft());
        possibleComs.Add(() => playerActions.DashRight());
        possibleComs.Add(() => playerActions.DashForward());
        possibleComs.Add(() => playerActions.DashBackward());
        possibleComs.Add(() => playerActions.LeftPunch());
        possibleComs.Add(() => playerActions.RightPunch());
        possibleComs.Add(() => playerActions.LeftRightPunch());
        possibleComs.Add(() => playerActions.LeftKick());
        possibleComs.Add(() => playerActions.RightKick());
        possibleComs.Add(() => playerActions.LeftRightKick());
        playerActions = GameObject.FindWithTag("Player").GetComponent<FighterController>();
        predictor.SetActive(false);
        foreach(Camera cam in actionCams){
            cam.enabled = false;
        }
        mainCam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        //move this to input handler later
        if(Input.GetAxisRaw("Pause") == 1 && up){
            up = false;
            if(!pause&&!executing){
                Time.timeScale = 0;
                pause = true;
                UIManager.SetUp();
                pauseQueue.Clear();
                camCon.pause = true;
                camCon.moveable = true;
                camOrigPos=Camera.main.transform.position;
                if(enemy!=null){
                    //stop enemy's action, somehow. If possible, find the hit box of the action before its disabled and have something that highlights that.
                    col=enemy.GetHitbox();
                    if(col!=null){
                        Debug.Log("the boy is here");
                        predictor.SetActive(true);
                        predictor.transform.localScale=col.bounds.size;
                        predictor.transform.position=col.bounds.center;
                    }
                    enemy.Pause();      
                }   
            }
            else if(pause&&!executing){
                Time.timeScale = 1;
                executing = true;
                pause = false;
                Debug.Log("unpause time");
                StartCoroutine(ExecuteMoves());
            }
        }
        if(Input.GetAxisRaw("Pause") == 0){
            up = true;
        }
    }
    private IEnumerator ExecuteMoves(){
        camCon.moveable = false;
        camCon.pause = false;
        camOrigPos=camCon.NewPos();
        while(Vector3.Distance(camOrigPos, Camera.main.transform.position)>.25f){
            //camCon.GoBack();
            yield return null;
        }
        UIManager.Hide();
        int i = 0;
        foreach(voidDelegate action in pauseQueue){
            //probably shouldn't hard code this...
            if(playerActions.max_heat-playerActions.heat<30||action==possibleComs[0]||action==possibleComs[1]||action==possibleComs[2]||action==possibleComs[3]){
            }
            else{
                if(i==0){
                    actionCams[actionCams.Count-1].enabled = false;
                    mainCam.enabled = false;
                }
                else{
                    actionCams[i-1].enabled = false;
                }
                actionCams[i].enabled = true;
                i++;
                if(i == actionCams.Count){
                    i = 0;
                }
            }
            Debug.Log("does this run?");
            action();
            UIManager.updateQueueButtons();
            //change this to be based on when something finishes running instead of being hard coded.
            yield return new WaitForSeconds(.5f);
        }
        if(i==0){
            actionCams[actionCams.Count-1].enabled = false;
        }
        else{
            actionCams[i-1].enabled = false;
        }
        mainCam.enabled = true;
        executing = false;
        camCon.pause = false;
        if(enemy!=null){
            //resume enemy's action, somehow
            enemy.Resume();
            predictor.SetActive(false);
        }
    }
    public void addToQueue(int i){
            pauseQueue.Add(possibleComs[i]);
        }
    //used for the queue :/
    public delegate void voidDelegate();
}
