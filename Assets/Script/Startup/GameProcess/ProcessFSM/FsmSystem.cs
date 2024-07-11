using System;
using System.Collections.Generic;
using UnityEngine;

public class ProcessFsmSystem
{
    private readonly Dictionary<string, IProcessStateNode> _nodes = new(5);
    private IProcessStateNode _curNode;
    private IProcessStateNode _preNode;
    public object Owner { private set; get; } //状态机持有者

    public ProcessFsmSystem(object obj) => Owner = obj;

    public void AddNode(Type type)
    {
        var stateNode = Activator.CreateInstance(type);
        if (string.IsNullOrEmpty(type.FullName)) throw new Exception("添加的节点为空");
        if (_nodes.ContainsKey(type.FullName)) throw new Exception($"状态节点已存在 : {type.FullName}");
        if (stateNode is not IProcessStateNode processStateNode) throw new Exception($"{type.FullName}没有继承接口");
        processStateNode.OnCreate(this);
        _nodes.Add(type.FullName, processStateNode);
    }

    public void ChangeState(string nodeName, object obj = null)
    {
        var node = TryGetNode(nodeName);
        if (node == null)
            throw new Exception($"没有找到状态节点 : {nodeName}");
        Debug.Log($"{_curNode.GetType().FullName} --> {node.GetType().FullName}");
        _preNode = _curNode;
        _curNode.OnExit();
        _curNode = node;
        _curNode.OnEnter(obj);
    }

    public void Run(string entryNode, object obj = null)
    {
        _curNode = TryGetNode(entryNode);
        _preNode = _curNode;
        if (_curNode == null)
            throw new Exception($"未找到进入的节点: {entryNode}");
        _curNode.OnEnter(obj);
    }

    private IProcessStateNode TryGetNode(string nodeName)
    {
        _nodes.TryGetValue(nodeName, out var result);
        return result;
    }
}