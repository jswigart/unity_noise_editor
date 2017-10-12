using UnityEngine;
using LibNoise.Operator;
using NodeEditorFramework;
using NodeEditorFramework.Utilities;

namespace LibNoiseNodes
{
    [System.Serializable]
    [Node(false, "Noise/Operator/Min")]
    public class NodeModuleOperatorMin : NodeModuleBase
    {
        public override string GetID { get { return typeof(NodeModuleOperatorMin).Name.ToLower(); } }
        
        Min _module = new Min();

        public NodeModuleOperatorMin()
        {
            InitModule(_module);
        }

        public override Node Create(Vector2 pos)
        {
            NodeModuleOperatorMin node = CreateInstance<NodeModuleOperatorMin>();
            node.rect = new Rect(pos.x, pos.y, NodeWidth, NodeHeight);
            node.name = "Min";
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

