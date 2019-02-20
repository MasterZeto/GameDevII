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
    public List<voidDelegate> pauseQueue;
    private List<voidDelegate> possibleComs;
    FighterController playerActions;
    bool pause = false;
    bool executing = false;
    bool up = true;
    Collider col;
    // Start is called before the first frame update
    void Start()
    {
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
            if(enemy!=null){
                //stop enemy's action, somehow. If possible, find the hit box of the action before its disabled and have something that highlights that.
                enemy.Pause();
                col=enemy.GetHitbox();
                if(col!=null){
                    Debug.Log("the boy is here");
                    predictor.SetActive(true);
                    predictor.transform.localScale=col.bounds.size;
                    predictor.transform.position=col.bounds.center;
                }      
            }   
            }
            else if(pause&&!executing){
                Time.timeScale = 1;
                executing = true;
                pause = false;
                StartCoroutine(ExecuteMoves());
            }
        }
        if(Input.GetAxisRaw("Pause") == 0){
            up = true;
        }
    }
    private IEnumerator ExecuteMoves(){
        Debug.Log("sdkfhsdkfh");
        UIManager.Hide();
        foreach(voidDelegate action in pauseQueue){
            action();
            UIManager.updateQueueButtons();
            //change this to be based on when something finishes running instead of being hard coded.
            yield return new WaitForSeconds(.5f);
        }
        executing = false;
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
