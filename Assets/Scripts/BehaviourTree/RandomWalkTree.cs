using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree
{
    public class RandomWalkTree : MyTree
    {
        protected float _currentWalkTime = 0f;
        protected float _maxWalkTime;
        public float CurrentWalkTime { get { return _currentWalkTime; } set { _currentWalkTime = value; } }
        public float MaxWalkTime { get { return _maxWalkTime; } set { _maxWalkTime = value; } }

        protected override Node SetupTree()
        {
            return new Node();
        }
    }
}
