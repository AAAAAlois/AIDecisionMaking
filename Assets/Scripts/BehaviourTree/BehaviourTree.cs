using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviourTree : Node
{
   public BehaviourTree()
    {
        name = "Tree";
    }

    public BehaviourTree(string n)
    {
        name = n;
    }

    public override Status Process()
    {
        if (children.Count == 0) return Status.SUCCESS; //make sure before the leaves are added to the tree, the Process() function can still run
        return children[currentChild].Process();    //behaviour tree runs its children, not itself
    }

    struct NodeLevel
    {
        public int level;
        public Node node;
    }

    public void PrintTree()
    {
        string treePrintOut = "";
        Stack<NodeLevel> nodeStack = new Stack<NodeLevel>();
        Node currentNode = this;
        nodeStack.Push(new NodeLevel { level = 0, node = currentNode });

        while(nodeStack.Count != 0)
        {
            NodeLevel nextNode = nodeStack.Pop();
            treePrintOut += nextNode.level + ":" + nextNode.node.name + "\n";
            for(int i = nextNode.node.children.Count -1; i >= 0; i--)
            {
                nodeStack.Push(new NodeLevel { level = nextNode.level + 1, node = nextNode.node.children[i] });
            }
        }

        Debug.Log(treePrintOut);
    }
}
