using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree
{
    public enum ENodeState
    {
        RUNNING,
        SUCCESS,
        FAILURE
    }

    [System.Serializable]
    public class Node
    {
        protected ENodeState state;
        protected List<Node> children = new List<Node>();

        private Dictionary<string, object> _data = new Dictionary<string, object>();
        private Node _parent;
        public Node Parent { get { return _parent; } set { _parent = value; } }

        #region Constructors
        public Node()
        {
            _parent = null;
        }

        public Node(List<Node> children)
        {
            foreach (Node child in children)
                Add(child);
        }
        #endregion

        #region Functions

        /// <summary>
        /// Adds a Node to the List of childrens from the current Node
        /// </summary>
        /// <param name="node">Node that will be added</param>
        private void Add(Node node)
        {
            node.Parent = this;
            children.Add(node);
        }

        /// <summary>
        /// Used to calculate the current Node state
        /// </summary>
        /// <returns>Returns current Node state</returns>
        public virtual ENodeState CalculateState()
        {
            return ENodeState.FAILURE;
        }

        /// <summary>
        /// Sets the value of a key in the data of the node
        /// </summary>
        /// <param name="key">Key that will be changed</param>
        /// <param name="value">Value that will be added</param>
        public void SetData(string key, object value)
        {
            _data[key] = value;
        }

        /// <summary>
        /// Trys to get data from the Node or its parents
        /// </summary>
        /// <param name="key">Key that is gonna be searched</param>
        /// <returns>Returns the searched value if it was found</returns>
        public object GetData(string key)
        {
            object value = null;
            if (_data.TryGetValue(key, out value))
                return value;

            Node tmp = _parent;
            while (tmp is not null)
            {
                value = tmp.GetData(key);
                if (value is not null)
                    return value;
                tmp = tmp.Parent;
            }
            return null;
        }

        /// <summary>
        /// Deletes data from the Node or its parents
        /// </summary>
        /// <param name="key">Data key which will be removed</param>
        /// <returns>Returns if it was a success or not</returns>
        public bool DeleteData(string key)
        {
            if (_data.ContainsKey(key))
            {
                _data.Remove(key);
                return true;
            }

            Node tmp = _parent;
            while (tmp is not null)
            {
                bool cleared = tmp.DeleteData(key);
                if (cleared)
                    return true;
                tmp = tmp.Parent;
            }
            return false;
        }

        /// <summary>
        /// Finds the root Node of the tree
        /// </summary>
        /// <param name="node">Node we are searching from</param>
        /// <returns>Returns the root Node reference</returns>
        public Node GetRoot(Node node)
        {
            if (node.Parent is null)
                return node;

            return GetRoot(node.Parent);
        }

        /// <summary>
        /// Changes a bool value of an animator
        /// </summary>
        /// <param name="animator">Used animator</param>
        /// <param name="paramName">Exact name of the bool</param>
        /// <param name="state">bool value it should change to</param>
        protected void SetAnimationBool(Animator animator, string paramName, bool state)
        {
            if (animator.GetBool(paramName) != state)
            {
                animator.SetBool(paramName, state);
            }
        }

        #endregion
    }
}
