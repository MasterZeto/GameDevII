﻿using System.Collections;
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
        if(t>10){
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
                            new ActionNode(fc.RightKick),
                            new ActionNode(fc.LeftPunch)
                        }
                    ),
                    new SequencerNode(
                        new List<Node>{
                            new ActionNode(fc.RightPunch),
                            new ActionNode(fc.RightKick)
                        }
                    )
                }
            )
        );
    }

    void Update()
    {
        //note: rightkick is gloat, leftpunch is harpoon, rightpunch is anchor
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