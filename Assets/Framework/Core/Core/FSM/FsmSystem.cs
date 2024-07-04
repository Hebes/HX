using System;
using System.Collections.Generic;
using UnityEngine;

/*--------脚本描述-----------

描述:
    有限状态机

-----------------------*/


namespace Framework.Core
{
    public interface IStateNode
    {
        /// <summary>
        /// 创建出来的时候会执行
        /// </summary>
        /// <param name="machine"></param>
        void OnCreate(FsmSystem machine);

        /// <summary>
        /// 状态切换的时候会执行
        /// </summary>
        void OnEnter();

        /// <summary>
        /// 轮询
        /// </summary>
        void OnUpdate();

        /// <summary>
        /// 退出
        /// </summary>
        void OnExit();
    }
}

namespace Framework.Core
{
    public class FsmSystem
    {
        private readonly Dictionary<string, IStateNode> _nodes = new(100);
        private IStateNode _curNode;
        private IStateNode _preNode;

        /// <summary>
        /// 状态机持有者
        /// </summary>
        public System.Object Owner { private set; get; }

        /// <summary>
        /// 当前运行的节点名称
        /// </summary>
        public string CurrentNode => _curNode != null ? _curNode.GetType().FullName : string.Empty;

        /// <summary>
        /// 之前运行的节点名称
        /// </summary>
        public string PreviousNode => _preNode != null ? _preNode.GetType().FullName : string.Empty;


        private FsmSystem()
        {
        }

        public FsmSystem(System.Object owner) => Owner = owner;

        /// <summary>
        /// 更新状态机
        /// </summary>
        public void Update() => _curNode?.OnUpdate();

        /// <summary>
        /// 启动状态机
        /// </summary>
        public void Run<TNode>() where TNode : IStateNode
        {
            var nodeType = typeof(TNode);
            var nodeName = nodeType.FullName;
            Run(nodeName);
        }

        public void Run(Type entryNode) => Run(entryNode.FullName);

        public void Run(string entryNode)
        {
            _curNode = TryGetNode(entryNode);
            _preNode = _curNode;
            if (_curNode == null)
                throw new Exception($"未找到进入的节点: {entryNode}");
            _curNode.OnEnter();
        }

        /// <summary>
        /// 加入一个节点
        /// </summary>
        public void AddNode<TNode>() where TNode : IStateNode
        {
            var stateNode = Activator.CreateInstance(typeof(TNode)) as IStateNode;
            AddNode(stateNode);
        }

        public void AddNode(IStateNode stateNode)
        {
            if (stateNode == null)
                throw new Exception("添加的节点为空");

            var nodeType = stateNode.GetType();
            var nodeName = nodeType.FullName;
            if (string.IsNullOrEmpty(nodeName))
                throw new Exception("添加的节点为空");
            if (_nodes.ContainsKey(nodeName))
                throw new Exception($"状态节点已存在 : {nodeName}");
            stateNode.OnCreate(this);
            _nodes.Add(nodeName, stateNode);
        }

        /// <summary>
        /// 转换状态节点
        /// </summary>
        public void ChangeState<TNode>() where TNode : IStateNode
        {
            var nodeName = typeof(TNode).FullName;
            ChangeState(nodeName);
        }

        public void ChangeState(Type nodeType)
        {
            var nodeName = nodeType.FullName;
            ChangeState(nodeName);
        }

        public void ChangeState(string nodeName)
        {
            if (string.IsNullOrEmpty(nodeName))
                throw new Exception("转换的节点为空");

            var node = TryGetNode(nodeName);
            if (node == null)
                throw new Exception($"Can not found state node : {nodeName}");
            Log($"{_curNode.GetType().FullName} --> {node.GetType().FullName}");
            _preNode = _curNode;
            _curNode.OnExit();
            _curNode = node;
            _curNode.OnEnter();
        }

        private IStateNode TryGetNode(string nodeName)
        {
            _nodes.TryGetValue(nodeName, out var result);
            return result;
        }

        private void Log(string str)
        {
            Debug.Log(str);
        }
    }
}