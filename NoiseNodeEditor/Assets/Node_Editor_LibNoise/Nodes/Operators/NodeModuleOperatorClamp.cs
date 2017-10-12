using UnityEngine;
using LibNoise.Operator;
using NodeEditorFramework;
using NodeEditorFramework.Utilities;

namespace LibNoiseNodes
{
    [System.Serializable]
    [Node(false, "Noise/Operator/Clamp")]
    public class NodeModuleOperatorClamp : NodeModuleBase
    {
        public override string GetID { get { return typeof(NodeModuleOperatorClamp).Name.ToLower(); } }
        
        public float Min = 0.0f;
        public float Max = 1.0f;

        Clamp _module = new Clamp();

        public NodeModuleOperatorClamp()
        {
            InitModule(_module);

            Min = (float)_module.Minimum;
            Max = (float)_module.Maximum;
        }

        protected override void PreGenerate()
        {
            _module.Minimum = (float)Min;
            _module.Maximum = (float)Max;
        }

        public override Node Create(Vector2 pos)
        {
            NodeModuleOperatorClamp node = CreateInstance<NodeModuleOperatorClamp>();
            node.rect = new Rect(pos.x, pos.y, NodeWidth, NodeHeight);
            node.name = "Clamp";
            node.NodeWidth = 300.0f;
            node.NodeHeight = 500.0f;

            node.CreateInput("Input", "Module", NodeSide.Left, 20);
            node.CreateOutput("Output", "Module", NodeSide.Right);

            return node;
        }

        protected override void NodeGUIProperties()
        {
            Min = RTEditorGUI.FloatField(new GUIContent("Minimum", "Min clamp value"), Min);
            Max = RTEditorGUI.FloatField(new GUIContent("Maximum", "Max clamp value"), Max);
        }
    }
}

