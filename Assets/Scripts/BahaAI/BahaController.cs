using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Giga.AI.BehaviorTree;

public class BahaController : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] float shortDistance;
    FighterController fc;
    float t = 0;
    
    BehaviorTree tree;

    int Random01()
    {
        return Random.Range(0, 3);
    }

    int distPlayer(){
        if(Vector3.Distance(player.position, transform.position)>shortDistance){
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
    Queue<ActionDelegate> actions;

    void Awake()
    {  
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
                                    new ActionNode(fc.RightKick)
                                }
                            ),
                            new SequencerNode(
                                new List<Node>{
                                    new ActionNode(fc.DashRight),
                                    new ActionNode(fc.LeftPunch)
                                }
                            )
                        }
                    ),
                    new SelectorNode(
                        gloat,
                        new List<Node>(){
                            new SequencerNode(
                                new List<Node>{
                                    new ActionNode(fc.DashRight),
                                    new ActionNode(fc.RightPunch),
                                    new ActionNode(fc.RightKick)
                                }
                            ),
                            new SequencerNode(
                                new List<Node>{
                                    new ActionNode(fc.DashRight),
                                    new ActionNode(fc.RightPunch),
                                }
                            )
                        }
                    )
                }
            )
        );
    }

    void Update()
    {
        //note: rightkick is gloat, leftpunch is harpoon, rightpunch is anchor, dash right is turn to player, dash forward is a very slow dash forward
        Debug.Log(fc.IsActing());
        if (actions.Count == 0){
            actions = tree.Evaluate();
        } 

        if (!fc.IsActing()){
            (actions.Dequeue())();
        }
        t+=Time.deltaTime;
        Debug.Log(distPlayer());
    }
}