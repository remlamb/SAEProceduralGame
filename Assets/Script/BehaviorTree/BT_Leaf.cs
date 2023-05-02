using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class BT_Leaf : BT_Node
{
    private Func<BT_Status> _performAction;
    public BT_Leaf(string name, Func<BT_Status> performAction) : base(name)
    {
        _performAction = performAction;
    }

    public override BT_Status Process()
    {
        BT_Status status = _performAction();
        //Debug.Log(_name + " - STATUS : " + status);
        return _performAction();
    }
}
