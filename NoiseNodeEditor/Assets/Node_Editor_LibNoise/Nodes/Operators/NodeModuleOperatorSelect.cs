using UnityEngine;
using LibNoise.Operator;
using NodeEditorFramework;
using NodeEditorFramework.Utilities;

namespace LibNoiseNodes
{
    [System.Serializable]
    [Node(false, "Noise/Operator/Select")]
    public class NodeModuleOperatorSelect : NodeModuleBase
    {
        public override string GetID { get { return typeof(NodeModuleOperatorSelect).Name.ToLower(); } }
        
        public float Min = 0.0f;
        public float Max = 1.0f;
        public float FallOff = 1.0f;

        Select _module = new Select();

        public NodeModuleOperatorSelect()
        {
            InitModule(_module);

            Min = (float)_module.Minimum;
            Max = (float)_module.Maximum;
            FallOff = (float)_module.FallOff;
        }

        protected override void PreGenerate()
        {
            _module.Minimum = Min;
            _module.Maximum = Max;
            _module.FallOff = FallOff;
        }

        public override Node Create(Vector2 pos)
        {
            NodeModuleOperatorSelect node = CreateInstance<NodeModuleOperatorSelect>();
            node.rect = new Rect(pos.x, pos.y, NodeWidth, NodeHeight);
            node.name = "Select";
            node.NodeWidth = 300.0f;
            node.NodeHeight = 500.0f;

            node.CreateInput("Input 1", "Module", NodeSide.Left, 20);
            node.CreateInput("Input 2", "Module", NodeSide.Left, 20);

            node.CreateInput("Controller", "Module", NodeSide.Left, 20);

            node.CreateOutput("Output", "Module", NodeSide.Right);
            
            return node;
        }

        protected override void NodeGUIProperties()
        {
            Min = RTEditorGUI.FloatField(new GUIContent("Minimum", "Min clamp value"), Min);
            Max = RTEditorGUI.FloatField(new GUIContent("Maximum", "Max clamp value"), Max);
            FallOff = RTEditorGUI.FloatField(new GUIContent("Falloff", "Falloff value at edge transition"), FallOff);
        }
    }
}

