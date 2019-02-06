using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    Generic container for behavior tree stuff, I'm not sure how these things are
    gonna get built, but I can figure that out later i guess, probably just
    load them in from a text file and build them dynamically
 */
namespace Giga.AI.BehaviorTree
{
    public delegate Node SelectDelegate();

    public abstract class Node 
    {
        protected Node parent;
        protected List<Node> children;

        public Node() {}

        public abstract Node next();
    }

    public abstract class SelectorNode : Node 
    {
        // wish I had a way of passing a function in here... then I don't have
        // to make it abstract

        SelectDelegate select;

        public SelectorNode(SelectDelegate d) : base()
        {
            select = d;
        }

    }

    public class SequencerNode : Node 
    {
        int current_node = -1;

        public SequencerNode() : base()
        {
        }

        // something to traverse into, cause otherwise things will blow up
        // i want to do it so that the next can be independant, so that I don't
        // have to store state because it'll just make things messier, so I'm 
        // concerned about traversal, basically, when I'm navigating into this, how
        // do I handle

        public override Node next()
        {
            current_node++;
            if (children.Count == current_node)
            {
                return parent.next();
            }
            else
            {
                return children[current_node];
            }
        }
    }

    public class ActionNode : Node 
    {
        // Action action;  // command pattern action
        public ActionNode(/* take in action object */) : base()
        {

        }  

        public bool finished
        {
            get
            {
                return false; // should look into the action to see if it's done
            }
        }

        // should return an action pointer eventually, so it can get updated per tick
        public void perform() { /* tell action to begin performing behavior */ }

        public override Node next() { return parent.next(); }
    }

    public class BehaviorTree
    {
        public BehaviorTree(/* some thing to define the tree */) {}
    }
}