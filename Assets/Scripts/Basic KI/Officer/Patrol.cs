using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree
{
    public class Patrol : Node
    {
        private Transform _thisTransform;
        private Animator _thisAnimator;
        private Transform[] _waypoints;
        private int _currentWaypointIndex;

        private float _waitTime = 1f;
        private float _waitCounter = 0f;
        private bool _waiting = false;


        public Patrol(Transform transform, Transform[] waypoints)
        {
            _thisTransform = transform;
            _waypoints = waypoints;
            _thisAnimator = transform.GetComponent<Animator>();
        }

        public override ENodeState CalculateState()
        {
            if (_waiting)
            {
                _waitCounter += Time.deltaTime;
                if (_waitCounter >= _waitTime)
                    _waiting = false;
            }
            else
            {
                Transform waypoint = _waypoints[_currentWaypointIndex];
                if (Vector3.Distance(_thisTransform.position, waypoint.position) < 0.01f)
                {
                    _thisTransform.position = waypoint.position;
                    _waitCounter = 0f;
                    _waiting = true;

                    _currentWaypointIndex = (_currentWaypointIndex + 1) % _waypoints.Length;
                }
                else
                {
                    _thisTransform.position = Vector3.MoveTowards(_thisTransform.position, waypoint.position, OfficerBT.speed * Time.deltaTime);
                    _thisTransform.LookAt(waypoint.position);
                }
            }

            return state = ENodeState.RUNNING;
        }

    } 
}
