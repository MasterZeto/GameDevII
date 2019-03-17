using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseScript : MonoBehaviour
{
    //main script dealing w/ the pause stuff
    //replace fightercontroller here for w/e is responsible for pausing the enemy
    [SerializeField] FighterController enemy;
    [SerializeField] GameObject enemyGameObject;
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
    bool isProjectile = false;
    Collider col;
    [SerializeField] LineRenderer lr;
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
        lr.positionCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(col!=null&&pause&&!isProjectile){
            Debug.Log(col.bounds.center);
            Debug.Log(col);
            col=enemy.GetHitbox(ref isProjectile);
            predictor.transform.position=col.gameObject.transform.position;
            predictor.SetActive(true);
            if(col.enabled){
                Debug.Log("unenabled");
                col.enabled = false;
            }
        }
        //move this to input handler later
        if(Input.GetAxisRaw("Pause") == 1 && up){
            up = false;
            if(!pause&&!executing){
                playerActions.pause = true;
                Time.timeScale = 0;
                pause = true;
                UIManager.SetUp();
                pauseQueue.Clear();
                camCon.pause = true;
                camCon.moveable = true;
                camOrigPos=Camera.main.transform.position;
                GameObject.FindWithTag("Player").GetComponent<SoundBox>().TimeSlowSFX();
                if(enemy!=null){
                    //stop enemy's action, somehow. If possible, find the hit box of the action before its disabled and have something that highlights that.
                    col=enemy.GetHitbox(ref isProjectile);
                    if(col!=null&&!isProjectile){
                        Debug.Log("the boy is here");
                        predictor.transform.localScale=col.bounds.size;
                        predictor.transform.eulerAngles=enemy.transform.eulerAngles;
                    }
                    else if(col!=null){
                        //check type of path somewhere to adjust line renderer?
                        StartCoroutine(DrawPath());
                    }
                    enemy.Pause();  
                }   
            }
            else if(pause&&!executing){
                Time.timeScale = 1;
                executing = true;
                pause = false;
                Debug.Log("unpause time");
                GameObject.FindWithTag("Player").GetComponent<SoundBox>().TimeSlowStop();                
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
        //camOrigPos=camCon.NewPos();
        /*while(Vector3.Distance(mainCam.transform.position, camCon.NewPos())>.4f){
            //camCon.GoBack();
            Debug.Log("Stuck here?");
            Debug.Log(Vector3.Distance(mainCam.transform.position, camCon.NewPos()));
            yield return null;
        }*/
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
            if(col!=null){
                col.enabled = true;
            }
            if(isProjectile){
                lr.positionCount = 0;
            }
            isProjectile = false;
        }
        playerActions.pause = false;
        UIManager.HidePauseHeat();
    }
    public void addToQueue(int i){
            pauseQueue.Add(possibleComs[i]);
        }
    public int GetPosComIndex(int x){
        return possibleComs.IndexOf(pauseQueue[x]);
    }
    //used for the queue :/
    public delegate void voidDelegate();
    private IEnumerator DrawPath(){
        int counter = 0;
        while(counter!=1){
            yield return null;
            counter++;
        }
        int i = 0;
        lr.positionCount = 2;
        lr.transform.position = col.gameObject.transform.position;
        Debug.Log(col.gameObject.transform.position);
        Debug.Log(col.gameObject);
        //CharacterController cc = lr.gameObject.GetComponent<CharacterController>();
        lr.SetPosition(i, lr.gameObject.transform.position);
        for(float t = enemy.GetRemainingTime(); t < enemy.GetHitDuration(); t+=Time.unscaledDeltaTime){
            Debug.Log(col.gameObject.transform.position);
            Vector3 direction = enemyGameObject.transform.forward;
            direction.y = 0;
            direction.Normalize();
            lr.transform.position+=(direction*Time.unscaledDeltaTime*enemy.GetProjectileSpeed()*2);
            i++;
            lr.positionCount = i + 1;
            //Debug.Log(i);
            lr.SetPosition(i, lr.gameObject.transform.position);
            yield return null;
        }
        /*Vector3 direction = lr.gameObject.transform.position;
        float y = direction.y;
        direction.y = 0;
        direction.Normalize();
        direction.x*=-5;
        direction.z*=-5;
        direction.y = y;
        lr.SetPosition(1, direction);*/
        yield return null;
    }
}
