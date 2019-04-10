using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Giga.AI.BehaviorTree;

public class BahaController : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] float shortDistance;
    [SerializeField] float fishFromBehindDistance;
    HarpoonAction harpoonCheck;
    FighterController fc;
    float t = 0;
    bool freezed;
    
    BehaviorTree tree;

    int Random01()
    {
        return Random.Range(0, 3);
    }

    int distPlayer(){
        if(Vector3.Distance(player.position, transform.position)>shortDistance){
            harpoonCheck.playerHit = false;
            return 0;
        }
        else{
            return 1;
        }
    }
    int gloat(){
        if(t>5){
            t = 0;
            return 0;
        }
        else{
            return 1;
        }
    }
    int playerAttached () {
        if(harpoonCheck.playerHit){
            harpoonCheck.playerHit = false;
            Debug.Log("player is attached");
            return 0;
        }
        Debug.Log("player is NOT attached");
        return 1;
    }
    int playerBehind(){
        if(Vector3.Distance(player.forward, transform.forward)>fishFromBehindDistance){
            Debug.Log("Should not be fishing!!!");
            return 0;
        }
        Debug.Log("Should fish for the player");
        return 1;
    }
    Queue<ActionDelegate> actions;

    void Awake()
    {
        harpoonCheck = GetComponent<HarpoonAction>();  
        fc = GetComponent<FighterController>();
        actions = new Queue<ActionDelegate>();
    }

    void Start()
    {
        tree = new BehaviorTree(
            new SelectorNode(
                distPlayer,
                new List<Node>()
                {
                    new SelectorNode(
                        gloat,
                        new List<Node>()
                        {
                            new SequencerNode(
                                new List<Node>{
                                    new ActionNode(fc.DashRight),
                                    new ActionNode(fc.DashForward),
                                    new ActionNode(fc.LeftPunch),
                                    new ActionNode(fc.RightKick),
                                }
                            ),
                            new SequencerNode(
                                new List<Node>{
                                    new ActionNode(fc.DashRight),
                                    new ActionNode(fc.LeftPunch),
                                    new ActionNode(fc.DashRight)
                                }
                            )
                        }
                    ),
                    new SelectorNode(
                        playerAttached,
                        new List<Node>(){
                            new SequencerNode(
                                new List<Node>(){
                                    new ActionNode(fc.LeftRightPunch),
                                }
                            ),
                            new SelectorNode(
                                playerBehind,
                                new List<Node>{
                                    new SelectorNode(
                                    gloat,
                                        new List<Node>(){
                                            new SequencerNode(
                                                new List<Node>{
                                                    new ActionNode(fc.LeftKick),
                                                    new ActionNode(fc.RightPunch),
                                                    new ActionNode(fc.RightKick)
                                                }
                                            ),
                                            new SequencerNode(
                                                new List<Node>{
                                                    new ActionNode(fc.LeftKick),
                                                    new ActionNode(fc.RightPunch),
                                                }
                                            )
                                        }
                                    ),
                                    new SequencerNode(
                                        new List<Node>{
                                            new ActionNode(fc.LeftRightKick),
                                            new ActionNode(fc.LeftRightPunch)
                                        }
                                    )
                                }
                            )
                        }
                    )
                }
            )
        );
    }
    public void SetFreezed(bool f)
    {
        freezed = f;
    }

    void Update()
    {
        //note: rightkick is gloat, leftpunch is harpoon, rightpunch is anchor, dash right is turn to player, dash forward is a very slow dash forward
        //LeftRightPunch is swing overhead, //leftkick is a gloat action for wasting time for animation
        // LeftRightKick is fish attack
        if(!freezed){
            Debug.Log(fc.IsActing());
        if (actions.Count == 0){
            actions = tree.Evaluate();
        } 

        if (!fc.IsActing()){
            (actions.Dequeue())();
        }
        t+=Time.deltaTime;
        }
    }
}