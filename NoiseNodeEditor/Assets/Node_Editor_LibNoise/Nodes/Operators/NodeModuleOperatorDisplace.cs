using UnityEngine;
using LibNoise.Operator;
using NodeEditorFramework;
using NodeEditorFramework.Utilities;

namespace LibNoiseNodes
{
    [System.Serializable]
    [Node(false, "Noise/Operator/Displace")]
    public class NodeModuleOperatorDisplace : NodeModuleBase
    {
        public override string GetID { get { return typeof(NodeModuleOperatorDisplace).Name.ToLower(); } }
        
        Displace _module = new Displace();

        public NodeModuleOperatorDisplace()
        {
            InitModule(_module);
        }

        public override Node Create(Vector2 pos)
        {
            NodeModuleOperatorDisplace node = CreateInstance<NodeModuleOperatorDisplace>();
            node.rect = new Rect(pos.x, pos.y, NodeWidth, NodeHeight);
            node.name = "Displace";
            node.NodeWidth = 300.0f;
            node.NodeHeight = 500.0f;

            node.CreateInput("Input", "Module", NodeSide.Left, 20);
            node.CreateInput("Displace X", "Module", NodeSide.Left, 20);
            node.CreateInput("Displace Y", "Module", NodeSide.Left, 20);
            node.CreateInput("Displace Z", "Module", NodeSide.Left, 20);

            node.CreateOutput("Output", "Module", NodeSide.Right);

            return node;
        }
        
        protected override void NodeGUIProperties()
        {            
        }
    }
}

