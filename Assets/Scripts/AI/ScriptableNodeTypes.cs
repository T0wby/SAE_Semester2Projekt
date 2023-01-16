using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/NodeType")]
public class ScriptableNodeTypes : ScriptableObject
{
    [SerializeField] private List<string> _classNames;

    public List<Type> ClassTypes => _classNames.ConvertAll(name => Type.GetType(name));
}
