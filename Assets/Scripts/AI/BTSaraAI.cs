using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Giga.AI.BehaviorTree;
using Giga.AI.Blackboard;

[RequireComponent(typeof(FighterController))]
public class BTSaraAI: MonoBehaviour
{
   // [SerializeField] Hitbox hitbox;
    BehaviorTree behaviorTree;
    FighterController Sara;
    bool freezed = false;

    //time variable?
    float t = 0;
    bool first = false;
    bool second = false;
    bool third = false;
    //?
    Queue<ActionDelegate> actions;
  

    void Awake()
    {
        Sara = GetComponent<FighterController>();
        actions = new Queue<ActionDelegate>();
    }
    //return 0,1,2,3
    int Random03()
    {
        return Random.Range(0, 4);
    }
    int Distance()
    {
        if (Vector3.Distance(Sara.transform.position, Blackboard.player_position) > 20f)
        { return 0; }
        else if (Vector3.Distance(Sara.transform.position, Blackboard.player_position) < 10f)
        {  return 2; }
        else { return 1; }

    }


    /*
        Essentially, I can define, as much as I'd like, various subtrees of this
        tree, and then build the behavior tree incrementally. So say I have two
        different "Behaviors" or "Strategies": an offensive mode, and defensive
        mode. 

        So, I can have a selector node at the top of the tree, that takes into
        account various bits of data about the game state, and concludes which
        of these modes I should use.

        Now, I can define subtrees (though they're not exactly subtrees, so yeah)

        but I can define nodes. So my Defensive node might be a selector node,
        and say my Offensive node is a sequencer node (cycles through a bunch of
        attacks). I can define it like this:

        SelectorNode Defensive = new SelectorNode(...);   // all the stuff for the
                                                          // defensive subtree
        SequencerNode Offensive = new SequencerNode(...); // same for here

        BehaviorTree tree = new BehaviorTree(
            SelectorNode(
                select_offense_or_defense,
                List<Node>()
                {
                    Defensive,
                    Offensive
                }
            )
        );

        So here, we need not necessarily define these all in one big nasty tree,
        but instead can define instances of these classes which the tree can then
        traverse during evaluation. This makes the designing of these trees less
        of a nightmare, because then we can define these kinds of "subroutines"
        which helps with organization but also with compartmentalizing the design
        of the behaviors for the purpose of development
     */

    void A1() {
        first = true;
        second = false;
        third = false;
        Debug.Log("left arm projection");
        Sara.LeftPunch(); }
    void A2() {
        first = true;
        second = false;
        third = false;
        Debug.Log("right arm projection");
        Sara.RightPunch(); }
    void A3() {
        first = false;
        second = true;
        third = false;
        Debug.Log("dash away A3");
        Sara.DashBackward();
    
    }
    void A4() {
        first = false;
        second = false;
        third = true;
        Debug.Log("jump highly and generate wind A4");
        Sara.LeftRightKick(); }
    void A5() {
        first =false;
        second = false;
        third = true;
        Debug.Log("cool down A5");
        Sara.LeftKick();
    }
   // void A6() { Debug.Log("empty here"); }
    void A7() {
        first = true;
        second = false;
        third = false;
        Debug.Log("tilt right");
        Sara.DashLeft(); }
    void A8() {
        first = true;
        second = false;
        third = false;
        Debug.Log("tilt left");
        Sara.DashRight();}
    void A9() {
        first = false;
        second = false;
        third = true;
        Sara.DashForward();
        Debug.Log("A9");


    }

    void Start()
    {
        behaviorTree = new BehaviorTree(
            new SelectorNode(
                Distance,
                new List<Node>()
                {  //left right projectile here and tilt left and right
                   
                    new SequencerNode(
                        new List<Node>(){
                        new ActionNode(A1),
                        new ActionNode(A7),
                        new ActionNode(A2),
                        new ActionNode(A8)
                        }
                        ),
                    //dash away from player
                     new ActionNode(A3),
                     //air push with a cool down 
                      new SequencerNode(
                        new List<Node>(){
                        new ActionNode(A9),//dash toward player a bit (angry
                        new ActionNode(A4),//air push 
                        new ActionNode(A5)//cool down and dash away
                     //   new ActionNode(A6)
                        }
                        )
                        }
                )
        );
    }

     //   Queue<ActionDelegate> actions = behaviorTree.Evaluate();

        void Update()
        {

        if (!freezed)
        {
            Debug.Log(Sara.IsActing());
            if (actions.Count == 0) actions = behaviorTree.Evaluate();
            else
            {
                if (Distance() != 0 && first)
                { actions = behaviorTree.Evaluate(); }
                if (Distance() != 2 && third)
                { actions = behaviorTree.Evaluate(); }
                if (Distance() != 1 && second)
                { actions = behaviorTree.Evaluate(); }

            }
            //  else if (Distance() == 2&&middle) actions = behaviorTree.Evaluate();

              if (!Sara.IsActing()) actions.Dequeue()();
            if (!Sara.IsActing()) actions = behaviorTree.Evaluate();



            //do I need to update distance here?
        }
        }

    public void SetFreezed(bool f)
    {
        freezed = f;
        //if (f) { sawyer.Pause(); } else { sawyer.Resume(); }
    }

    //  while (actions.Count > 0)
    //  {
    //ActionDelegate a = actions.Dequeue();
    // a();
    //}

}
