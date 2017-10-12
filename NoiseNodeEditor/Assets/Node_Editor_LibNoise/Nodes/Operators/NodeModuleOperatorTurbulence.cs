using UnityEngine;
using LibNoise.Operator;
using NodeEditorFramework;
using NodeEditorFramework.Utilities;

namespace LibNoiseNodes
{
    [System.Serializable]
    [Node(false, "Noise/Operator/Turbulence")]
    public class NodeModuleOperatorTurbulence : NodeModuleBase
    {
        public override string GetID { get { return typeof(NodeModuleOperatorTurbulence).Name.ToLower(); } }
        
        public float Frequency = 1.0f;
        public float Power = 1.0f;
        public int Roughness = 1;
        public int Seed = 0;

        Turbulence _module = new Turbulence();

        public NodeModuleOperatorTurbulence()
        {
            // pull the defaults from the noise module
            Frequency = (float)_module.Frequency;
            Power = (float)_module.Power;
            Roughness = _module.Roughness;
            Seed = _module.Seed;

            InitModule(_module);
        }

        protected override void PreGenerate()
        {
            _module.Frequency = Frequency;
            _module.Power = Power;
            _module.Roughness = Roughness;
            _module.Seed = Seed;
        }

        public override Node Create(Vector2 pos)
        {
            NodeModuleOperatorTurbulence node = CreateInstance<NodeModuleOperatorTurbulence>();
            node.rect = new Rect(pos.x, pos.y, NodeWidth, NodeHeight);
            node.name = "Turbulence";
            node.NodeWidth = 300.0f;
            node.NodeHeight = 500.0f;
            node.CreateInput("Input", "Module", NodeSide.Left, 20);
            node.CreateOutput("Output", "Module", NodeSide.Right);
            
            return node;
        }

        protected override void NodeGUIProperties()
        {
            GUILayout.BeginVertical();

            Frequency = RTEditorGUI.FloatField(new GUIContent("Frequency", "Frequency of the first octave"), Frequency);
            Power = RTEditorGUI.FloatField(new GUIContent("Power", "Set Power"), Power);
            Roughness = RTEditorGUI.IntField(new GUIContent("Roughness", "Set Roughness"), Roughness);
            Seed = RTEditorGUI.IntField(new GUIContent("Seed", "Random Seed"), Seed);
                        
            GUILayout.EndVertical();            
        }
    }
}
