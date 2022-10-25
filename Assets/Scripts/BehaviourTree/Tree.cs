using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace BehaviorTree
{
    public abstract class MyTree : MonoBehaviour
    {
        private Node _root = null;
        protected NavMeshAgent _agent;
        protected int _enemyLayerMask;

        protected void Start()
        {
            _root = SetupTree();
        }

        private void Update()
        {
            if (_root is not null)
            {
                _root.CalculateState();
            }
        }

        protected abstract Node SetupTree();
    } 
}
