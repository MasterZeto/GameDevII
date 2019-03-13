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
    public delegate void ActionDelegate();
    public delegate int SelectDelegate();

    public class ActionQueue
    {
        public static void ConcatenateQueue(Queue<ActionDelegate> q1, Queue<ActionDelegate> q2)
        {
            while (q2.Count > 0) { q1.Enqueue(q2.Dequeue()); }
        }
    }

    public abstract class Node 
    {
        protected List<Node> children;

        public Node(List<Node> children) 
        {
            this.children = children;
        }

        public abstract Queue<ActionDelegate> Evaluate();
    }

    public class SelectorNode : Node 
    {
        SelectDelegate select;

        public SelectorNode(SelectDelegate selection_function, List<Node> children)
         : base(children) 
        {
            select = selection_function;
        }

        public override Queue<ActionDelegate> Evaluate()
        {
            int selection = select();
            
            return children[selection].Evaluate();
        }
    }

    public class SequencerNode : Node 
    {
        public SequencerNode(List<Node> children) : base(children) {}

        public override Queue<ActionDelegate> Evaluate()
        {
            Queue<ActionDelegate> sequence = new Queue<ActionDelegate>();

            for (int i = 0; i < children.Count; i++)
            {
                ActionQueue.ConcatenateQueue(sequence, children[i].Evaluate());
            }

            return sequence;
        }
    }

    // Node wrapper around the Action class
    public class ActionNode : Node 
    {
        ActionDelegate action;

        public ActionNode(ActionDelegate action) : base(null) 
        { 
            this.action = action; 
        }

        public override Queue<ActionDelegate> Evaluate() 
        { 
            Queue<ActionDelegate> action_queue = new Queue<ActionDelegate>();
            action_queue.Enqueue(action);
            return action_queue;
        }  
    }

    public class BehaviorTree
    {
        // store the actual treeeeeeeeeeeeee
        Node root;

        public BehaviorTree(Node root) 
        {
            this.root = root;
        }

        public Queue<ActionDelegate> Evaluate()
        {
            return root.Evaluate();
        }
    }

    /* should probably return a queue of actions or something, that way it can
        have some way of returning some more complex stuff. So like say I have
        something like this:

        root
         |
         Sequencer
         |   | 
         A1  Sequencer
             |   |   |
             A2  A3  A4
    

        If I was to evaluate this behavior tree, I should actually return the
        following queue: [A1, A2, A3, A4] and iterate through it. Do A1 then when
        done do A2, and so on. 

        So I should have a function which evaluates the tree, then returns a
        queue of all the actions to take
    
     */
}