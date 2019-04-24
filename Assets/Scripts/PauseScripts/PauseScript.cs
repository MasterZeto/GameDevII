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
    [SerializeField] Camera bahaCam = null;
    [SerializeField] GameObject hurtBoxHighlight = null;
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
    [SerializeField] LineRenderer lr = null;
    bool bahaCamEnabled = false;
    public bool doneExecuting {get; private set; }
    BoxCollider hurtbox = null;
    Vector3 dashPredictorPosition;
    Vector3 dashPredictorForward;
    [SerializeField] GameObject dashPredictor = null;
    // Start is called before the first frame update
    void Start()
    {
        doneExecuting = true;
        mainCam = Camera.main;
        camCon = Camera.main.GetComponent<CameraController>();
        UIManager = GetComponent<PauseUIManager>();
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
        if(lr!=null) lr.positionCount = 0;
        GameObject hurtboxObj = enemyGameObject.transform.Find("HurtBoxes").gameObject;
        if(hurtboxObj!=null) hurtbox = hurtboxObj.GetComponent<BoxCollider>();
        if(hurtBoxHighlight!=null) hurtBoxHighlight.SetActive(false);
        if(dashPredictor!=null) dashPredictor.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(col!=null&&pause&&!isProjectile){
            col=enemy.GetHitbox(ref isProjectile);
            predictor.SetActive(true);
            if(col!=null&&col.enabled){
                predictor.transform.position=col.gameObject.transform.position;
                col.enabled = false;
            }
        }
    }

    public void TogglePause()
    {
        if (!pause && !executing)
        {
            doneExecuting = false;
            if (bahaCam != null && bahaCam.enabled)
            {
                bahaCamEnabled = true;
                bahaCam.enabled = false;
                mainCam.enabled = true;
            }
            playerActions.pause = true;
            Time.timeScale = 0;
            pause = true;
            UIManager.SetUp();
            pauseQueue.Clear();
            camCon.pause = true;
            camCon.moveable = true;
            camOrigPos = Camera.main.transform.position;
            GameObject.FindWithTag("Player").GetComponent<SoundBox>().TimeSlowSFX();
            dashPredictorPosition = playerActions.gameObject.transform.position;
            dashPredictorForward = playerActions.gameObject.transform.forward;
            if (enemy != null)
            {
                //stop enemy's action, somehow. If possible, find the hit box of the action before its disabled and have something that highlights that.
                col = enemy.GetHitbox(ref isProjectile);
                Debug.Log(col);
                if (col != null && !isProjectile)
                {
                    Debug.Log("the boy is here");
                    predictor.SetActive(true);
                    predictor.transform.localScale = col.bounds.size;
                    predictor.transform.eulerAngles = enemy.transform.eulerAngles;
                }
                else if (col != null)
                {
                    //check type of path somewhere to adjust line renderer?
                    StartCoroutine(DrawPath());
                }
                SetHurtBox();
                enemy.Pause();
            }
        }
        else if (pause && !executing)
        {
            Time.timeScale = 1;
            executing = true;
            pause = false;
            Debug.Log("unpause time");
            GameObject.FindWithTag("Player").GetComponent<SoundBox>().TimeSlowStop();
            if (dashPredictor != null) dashPredictor.SetActive(false);
            StartCoroutine(ExecuteMoves());
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
            if(action == possibleComs[6]||action == possibleComs[9]){
                yield return new WaitForSeconds(1f);
            }
            else{
                yield return new WaitForSeconds(.5f);
            }
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
        if(bahaCamEnabled){
            bahaCam.enabled = true;
            mainCam.enabled = false;
            bahaCamEnabled = false;
        }
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
        HideHurtBox();
        UIManager.HidePauseHeat();
        doneExecuting = true;
    }
    public void addToQueue(int i){
            pauseQueue.Add(possibleComs[i]);
            DashAction dash = playerActions.GetDash(i);
            if(dash!=null){
                dashPredictorPosition+=dash.Predictor(playerActions, ref dashPredictorForward, dashPredictorPosition);
            }
            if(dashPredictor!=null){
                dashPredictor.SetActive(true);
                dashPredictor.transform.position = dashPredictorPosition;
            }  
        }

    public void PopBackQueue()
    {
        if (pause)
        {
            pauseQueue.RemoveAt(pauseQueue.Count - 1);
            UIManager.PopBackQueue();
        }
    }

    public void UpdateDash(int i){
        DashAction dash = playerActions.GetDash(possibleComs.IndexOf(pauseQueue[i]));
        if(dash!=null){
                //dashPredictorPosition-=dash.Predictor(playerActions);
                if(dashPredictor!=null) dashPredictor.transform.position = dashPredictorPosition;
            }
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
        HarpoonAction harp = enemy.GetHarpoonAction();
        //Debug.Log(harp.GetDirection()*Time.unscaledDeltaTime*harp.GetSpeed());
        //CharacterController cc = lr.gameObject.GetComponent<CharacterController>();
        if(harp==null||harp.GetRemainingTime()>=harp.GetHitDuration()){
            lr.positionCount = 0;
        }
        else{
            lr.positionCount = 2;
            Vector3[] positionArray= new Vector3[2];
            lr.transform.position = harp.GetPosition();
            positionArray[0]=lr.gameObject.transform.position;
            for(float t = harp.GetRemainingTime(); t < harp.GetHitDuration(); t+=Time.unscaledDeltaTime){
                //Debug.Log(harp.GetDirection()*Time.unscaledDeltaTime*harp.GetSpeed());
                lr.positionCount = 2;
                lr.gameObject.transform.position+=(harp.GetDirection()*Time.unscaledDeltaTime*harp.GetSpeed());
                positionArray[1] = lr.gameObject.transform.position;
                lr.SetPositions(positionArray);
                if(doneExecuting){
                    lr.positionCount = 0;
                    break;
                }
                yield return null;
            }
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
    private void SetHurtBox(){
        if(hurtbox!=null&&hurtBoxHighlight!=null){
            hurtBoxHighlight.SetActive(true);
            hurtBoxHighlight.transform.localScale=hurtbox.bounds.size;
            hurtBoxHighlight.transform.position=hurtbox.bounds.center;
            hurtBoxHighlight.transform.eulerAngles=enemy.transform.eulerAngles;
        }

    }
    private void HideHurtBox(){
        if(hurtBoxHighlight!=null) hurtBoxHighlight.SetActive(false);
    }
}
