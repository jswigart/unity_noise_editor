using UnityEngine;
using LibNoise.Operator;
using NodeEditorFramework;
using NodeEditorFramework.Utilities;

namespace LibNoiseNodes
{
    [System.Serializable]
    [Node(false, "Noise/Operator/Terrace")]
    public class NodeModuleOperatorTerrace : NodeModuleBase
    {
        public override string GetID { get { return typeof(NodeModuleOperatorTerrace).Name.ToLower(); } }
        
        public bool Inverted = false;

        Terrace _module = new Terrace();

        public NodeModuleOperatorTerrace()
        {
            InitModule(_module);

            Inverted = _module.IsInverted;
        }

        protected override void PreGenerate()
        {
            _module.IsInverted = Inverted;
        }

        public override Node Create(Vector2 pos)
        {
            NodeModuleOperatorTerrace node = CreateInstance<NodeModuleOperatorTerrace>();
            node.rect = new Rect(pos.x, pos.y, NodeWidth, NodeHeight);
            node.name = "Terrace";
            node.NodeWidth = 300.0f;
            node.NodeHeight = 500.0f;

            node.CreateInput("Input", "Module", NodeSide.Left, 20);
            node.CreateOutput("Output", "Module", NodeSide.Right);
            
            return node;
        }
        
        protected override void NodeGUIProperties()
        {
            Inverted = RTEditorGUI.Toggle(Inverted, new GUIContent("Inverted", "Invert the curve"));
        }
    }
}

