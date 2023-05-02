using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum BT_Status
{
    RUNNING,
    FAILURE,
    SUCCESS
}

public abstract class BT_Node 
{
    protected string _name;
    private int _depth;
    public int Depth
    {
        get { return _depth; }
        set { _depth = value; }
    }

    protected List<BT_Node> childs = new List<BT_Node>();    

    public BT_Node(string name) //root
    {
        _name = name;
        _depth = 0;
    }

    public virtual BT_Status Process()
    {
        //Debug.Log("Node: " + _name + " depth : " + _depth);
        foreach (var child in childs)
        {
            BT_Status status = child.Process();
            //Debug.Log(child._name + " - Status : " + status);
        }
        return BT_Status.SUCCESS;
    }

    public void AddNode(BT_Node newNode)
    {
        newNode.Depth = this._depth + 1;
        childs.Add(newNode);
    }

}
