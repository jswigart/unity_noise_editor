using UnityEngine;
using LibNoise.Operator;
using NodeEditorFramework;
using NodeEditorFramework.Utilities;

namespace LibNoiseNodes
{
    [System.Serializable]
    [Node(false, "Noise/Operator/Curve")]
    public class NodeModuleOperatorCurve : NodeModuleBase
    {
        public override string GetID { get { return typeof(NodeModuleOperatorCurve).Name.ToLower(); } }
        
        // todo: expose a curve editor

        Curve _module = new Curve();

        public NodeModuleOperatorCurve()
        {
            InitModule(_module);
        }

        public override Node Create(Vector2 pos)
        {
            NodeModuleOperatorCurve node = CreateInstance<NodeModuleOperatorCurve>();
            node.rect = new Rect(pos.x, pos.y, NodeWidth, NodeHeight);
            node.name = "Curve";
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

