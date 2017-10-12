using UnityEngine;
using LibNoise.Operator;
using NodeEditorFramework;
using NodeEditorFramework.Utilities;

namespace LibNoiseNodes
{
    [System.Serializable]
    [Node(false, "Noise/Operator/Power")]
    public class NodeModuleOperatorPower : NodeModuleBase
    {
        public override string GetID { get { return typeof(NodeModuleOperatorPower).Name.ToLower(); } }
        
        Power _module = new Power();

        public NodeModuleOperatorPower()
        {
            InitModule(_module);
        }

        public override Node Create(Vector2 pos)
        {
            NodeModuleOperatorPower node = CreateInstance<NodeModuleOperatorPower>();
            node.rect = new Rect(pos.x, pos.y, NodeWidth, NodeHeight);
            node.name = "Power";
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

