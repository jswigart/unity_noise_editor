using UnityEngine;
using LibNoise.Generator;
using NodeEditorFramework;
using NodeEditorFramework.Utilities;

namespace LibNoiseNodes
{
    [System.Serializable]
    [Node(false, "Noise/Generator/Gradient")]
    public class NodeModuleGeneratorGradient : NodeModuleBase
    {
        public override string GetID { get { return typeof(NodeModuleGeneratorGradient).Name.ToLower(); } }

        private float _min;
        private float _max;

        LibNoise.Generator.Gradient _module = new LibNoise.Generator.Gradient();

        public NodeModuleGeneratorGradient()
        {
            InitModule(_module);
            
            _min = (float)_module.Min;
            _max = (float)_module.Max;
        }

        protected override void PreGenerate()
        {
            _module.Min = _min;
            _module.Max = _max;
        }

        public override Node Create(Vector2 pos)
        {
            NodeModuleGeneratorGradient node = CreateInstance<NodeModuleGeneratorGradient>();
            node.rect = new Rect(pos.x, pos.y, NodeWidth, NodeHeight);
            node.name = "Gradient";
            node.NodeWidth = 300.0f;
            node.NodeHeight = 500.0f;
            node.CreateOutput("Output", "Module", NodeSide.Right);

            return node;
        }

        protected override void NodeGUIProperties()
        {
            using (var horizontalScope = new GUILayout.HorizontalScope("box"))
            {
                GUILayout.FlexibleSpace();
                const float sz = 70.0f;
                _min = RTEditorGUI.FloatField("Min", _min, GUILayout.Width(sz));
                _max = RTEditorGUI.FloatField("Max", _max, GUILayout.Width(sz));
                GUILayout.FlexibleSpace();
            }
        }
    }
}
