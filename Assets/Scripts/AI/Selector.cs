using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree
{
    public class Selector : Node
    {
        //Or operator

        #region Constructors
        public Selector() : base()
        {

        }

        public Selector(List<Node> children) : base(children)
        {

        }
        #endregion

        public override ENodeState CalculateState()
        {
            foreach (Node node in children)
            {
                switch (node.CalculateState())
                {
                    case ENodeState.FAILURE:
                        continue;
                    case ENodeState.RUNNING:
                        state = ENodeState.RUNNING;
                        return state;
                    case ENodeState.SUCCESS:
                        state = ENodeState.SUCCESS;
                        return state;
                    default:
                        continue;
                }
            }

            return state = ENodeState.FAILURE;
        }
    } 
}
