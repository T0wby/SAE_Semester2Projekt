using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree
{
    public class Sequence : Node
    {
        //And operator

        #region Constructors
        public Sequence() : base()
        {

        }

        public Sequence(List<Node> children) : base(children)
        {

        }
        #endregion

        public override ENodeState CalculateState()
        {
            bool childIsRunning = false;

            foreach (Node node in children)
            {
                switch (node.CalculateState())
                {
                    case ENodeState.FAILURE:
                        state = ENodeState.FAILURE;
                        return state;
                    case ENodeState.RUNNING:
                        childIsRunning = true;
                        continue;
                    case ENodeState.SUCCESS:
                        continue;
                    default:
                        state = ENodeState.SUCCESS;
                        return state;
                }
            }

            return state = childIsRunning ? ENodeState.RUNNING : ENodeState.SUCCESS;
        }
    } 
}
