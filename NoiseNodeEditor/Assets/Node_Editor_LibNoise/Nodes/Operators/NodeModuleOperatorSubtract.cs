using UnityEngine;
using LibNoise.Operator;
using NodeEditorFramework;
using NodeEditorFramework.Utilities;

namespace LibNoiseNodes
{
    [System.Serializable]
    [Node(false, "Noise/Operator/Subtract")]
    public class NodeModuleOperatorSubtract : NodeModuleBase
    {
        public override string GetID { get { return typeof(NodeModuleOperatorSubtract).Name.ToLower(); } }
        
        Subtract _module = new Subtract();

        public NodeModuleOperatorSubtract()
        {
            InitModule(_module);
        }

        public override Node Create(Vector2 pos)
        {
            NodeModuleOperatorSubtract node = CreateInstance<NodeModuleOperatorSubtract>();
            node.rect = new Rect(pos.x, pos.y, NodeWidth, NodeHeight);
            node.name = "Subtract";
            node.NodeWidth = 300.0f;
            node.NodeHeight = 500.0f;

            node.CreateInput("Input 1", "Module", NodeSide.Left, 20);
            node.CreateInput("Input 2", "Module", NodeSide.Left, 20);
            node.CreateOutput("Output", "Module", NodeSide.Right);
            
            return node;
        }
        
        protected override void NodeGUIProperties()
        {
        }
    }
}

