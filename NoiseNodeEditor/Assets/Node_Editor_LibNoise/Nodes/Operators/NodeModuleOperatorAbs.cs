using UnityEngine;
using LibNoise.Operator;
using NodeEditorFramework;
using NodeEditorFramework.Utilities;

namespace LibNoiseNodes
{
    [System.Serializable]
    [Node(false, "Noise/Operator/Abs")]
    public class NodeModuleOperatorAbs : NodeModuleBase
    {
        public override string GetID { get { return typeof(NodeModuleOperatorAbs).Name.ToLower(); } }
        
        Abs _module = new Abs();

        public NodeModuleOperatorAbs()
        {
            InitModule(_module);
        }

        public override Node Create(Vector2 pos)
        {
            NodeModuleOperatorAbs node = CreateInstance<NodeModuleOperatorAbs>();
            node.rect = new Rect(pos.x, pos.y, NodeWidth, NodeHeight);
            node.name = "Abs";
            node.NodeWidth = 300.0f;
            node.NodeHeight = 500.0f;

            node.CreateInput("Input", "Module", NodeSide.Left, 20);
            node.CreateOutput("Output", "Module", NodeSide.Right);

            return node;
        }
        
        protected override void NodeGUIProperties()
        {            
        }
    }
}

