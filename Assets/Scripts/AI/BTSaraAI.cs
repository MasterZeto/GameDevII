using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Giga.AI.BehaviorTree;
using Giga.AI.Blackboard;

public class BTSaraAI: MonoBehaviour
{
    BehaviorTree behaviorTree;
    public FighterController character;

    int Random01()
    {
        return Random.Range(0, 2);
    }
    int Distance()
    {
        if (Vector3.Distance(character.transform.position, Blackboard.player_position) > 10f)
        { return 0; }
        else if (Vector3.Distance(character.transform.position, Blackboard.player_position) < 3f)
        { return 2; }
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

    void A1() { Debug.Log("Action A1"); }
    void A2() { Debug.Log("Action A2"); }
    void A3() { Debug.Log("Action A3"); }
    void A4() { Debug.Log("Action A4"); }
    void A5() { Debug.Log("Action A5"); }
    void A6() { Debug.Log("Action A6"); }

    void Start()
    {
        behaviorTree = new BehaviorTree(
            new SelectorNode(
                Distance,
                new List<Node>()
                {
                    new SequencerNode(
                        new List<Node>(){
                        new ActionNode(A1),
                        new ActionNode(A2)
                        }
                        ),

                     new ActionNode(A3),

                      new SequencerNode(
                        new List<Node>(){
                        new ActionNode(A4),
                        new ActionNode(A5),
                        new ActionNode(A6)
                        }
                        )
                        }
                )
        );
        

        Queue<ActionDelegate> actions = behaviorTree.Evaluate();

        while (actions.Count > 0)
        {
            ActionDelegate a = actions.Dequeue();
            a();
        }
    }
}
