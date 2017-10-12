using UnityEngine;
using LibNoise.Operator;
using NodeEditorFramework;
using NodeEditorFramework.Utilities;

namespace LibNoiseNodes
{
    [System.Serializable]
    [Node(false, "Noise/Operator/Invert")]
    public class NodeModuleOperatorInvert : NodeModuleBase
    {
        public override string GetID { get { return typeof(NodeModuleOperatorInvert).Name.ToLower(); } }
        
        Invert _module = new Invert();

        public NodeModuleOperatorInvert()
        {
            InitModule(_module);
        }

        public override Node Create(Vector2 pos)
        {
            NodeModuleOperatorInvert node = CreateInstance<NodeModuleOperatorInvert>();
            node.rect = new Rect(pos.x, pos.y, NodeWidth, NodeHeight);
            node.name = "Invert";
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

