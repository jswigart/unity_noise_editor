using UnityEngine;
using LibNoise.Operator;
using NodeEditorFramework;
using NodeEditorFramework.Utilities;

namespace LibNoiseNodes
{
    [System.Serializable]
    [Node(false, "Noise/Operator/Exponent")]
    public class NodeModuleOperatorExponent : NodeModuleBase
    {
        public override string GetID { get { return typeof(NodeModuleOperatorExponent).Name.ToLower(); } }
        
        Exponent _module = new Exponent();

        public NodeModuleOperatorExponent()
        {
            InitModule(_module);
        }

        public override Node Create(Vector2 pos)
        {
            NodeModuleOperatorExponent node = CreateInstance<NodeModuleOperatorExponent>();
            node.rect = new Rect(pos.x, pos.y, NodeWidth, NodeHeight);
            node.name = "Exponent";
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

