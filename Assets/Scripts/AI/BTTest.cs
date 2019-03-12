using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Giga.AI.BehaviorTree;

public class BTTest : MonoBehaviour
{
    public class A1 : Action
    {
        public override void Pause()
        {
            throw new System.NotImplementedException();
        }

        public override void Resume()
        {
            throw new System.NotImplementedException();
        }

        public override void StartAction(FighterController fighter)
        {
            throw new System.NotImplementedException();
        }

        public override void Stop()
        {
            throw new System.NotImplementedException();
        }
    }
    public class A2 : Action
    {
        public override void Pause()
        {
            throw new System.NotImplementedException();
        }

        public override void Resume()
        {
            throw new System.NotImplementedException();
        }

        public override void StartAction(FighterController fighter)
        {
            throw new System.NotImplementedException();
        }

        public override void Stop()
        {
            throw new System.NotImplementedException();
        }
    }
    public class A3 : Action
    {
        public override void Pause()
        {
            throw new System.NotImplementedException();
        }

        public override void Resume()
        {
            throw new System.NotImplementedException();
        }

        public override void StartAction(FighterController fighter)
        {
            throw new System.NotImplementedException();
        }

        public override void Stop()
        {
            throw new System.NotImplementedException();
        }
    }
    public class A4 : Action
    {
        public override void Pause()
        {
            throw new System.NotImplementedException();
        }

        public override void Resume()
        {
            throw new System.NotImplementedException();
        }

        public override void StartAction(FighterController fighter)
        {
            throw new System.NotImplementedException();
        }

        public override void Stop()
        {
            throw new System.NotImplementedException();
        }
    }
    public class A5 : Action
    {
        public override void Pause()
        {
            throw new System.NotImplementedException();
        }

        public override void Resume()
        {
            throw new System.NotImplementedException();
        }

        public override void StartAction(FighterController fighter)
        {
            throw new System.NotImplementedException();
        }

        public override void Stop()
        {
            throw new System.NotImplementedException();
        }
    }
    public class A6 : Action
    {
        public override void Pause()
        {
            throw new System.NotImplementedException();
        }

        public override void Resume()
        {
            throw new System.NotImplementedException();
        }

        public override void StartAction(FighterController fighter)
        {
            throw new System.NotImplementedException();
        }

        public override void Stop()
        {
            throw new System.NotImplementedException();
        }
    }

    BehaviorTree behaviorTree;

    int Random01()
    {
        return Random.Range(0, 2);
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

    void Start()
    {
        behaviorTree = new BehaviorTree(
            new SelectorNode(
                Random01,
                new List<Node>() 
                {
                    new ActionNode(new A1()),
                    new SequencerNode(
                        new List<Node>()
                        {
                            new ActionNode(new A2()),
                            new ActionNode(new A3()),
                            new SelectorNode(
                                Random01,
                                new List<Node>()
                                {
                                    new ActionNode(new A4()),
                                    new ActionNode(new A5())
                                }
                            ),
                            new ActionNode(new A6())                            
                        }
                    )
                }
            )
        );

        Queue<Action> actions = behaviorTree.Evaluate();

        while (actions.Count > 0)
        {
            Action a = actions.Dequeue();
            Debug.Log(a.GetType().Name);
        }
    }
}
