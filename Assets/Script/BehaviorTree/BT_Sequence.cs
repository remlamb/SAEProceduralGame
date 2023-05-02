using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BT_Sequence : BT_Node
{
    private int _idxCurrentChild = 0;
    public BT_Sequence(string name) : base("SEQUENCE " + name)
    {
    }

    public override BT_Status Process()
    {
        if (_idxCurrentChild < childs.Count)
        {
            BT_Status status = childs[_idxCurrentChild].Process();
            if(status == BT_Status.FAILURE)
            {
                _idxCurrentChild = 0;
                return BT_Status.FAILURE;
            }
            else if(status == BT_Status.RUNNING)
            {
                return BT_Status.RUNNING;
            }
            else
            {
                _idxCurrentChild++;
                return BT_Status.RUNNING;
            }
        }
        else
        {
            _idxCurrentChild = 0;
            return BT_Status.SUCCESS;
        }
    }

}
