using UnityEngine;
using LibNoise.Generator;
using NodeEditorFramework;
using NodeEditorFramework.Utilities;

namespace LibNoiseNodes
{
    [System.Serializable]
    [Node(false, "Noise/Generator/Voronoi")]
    public class NodeModuleGeneratorVoronoi : NodeModuleBase
    {
        public override string GetID { get { return typeof(NodeModuleGeneratorVoronoi).Name.ToLower(); } }
        
        public float Displacement = 1.0f; 
        public float Frequency = 1.0f;        
        public int Seed = 0;
        public bool UseDistance;

        Voronoi _module = new Voronoi();

        public NodeModuleGeneratorVoronoi()
        {
            // pull the defaults from the noise module
            Displacement = (float)_module.Displacement;
            Frequency = (float)_module.Frequency;            
            Seed = _module.Seed;
            UseDistance = _module.UseDistance;

            InitModule(_module);
        }

        protected override void PreGenerate()
        {
            _module.Frequency = (float)Frequency;
            _module.Displacement = (float)Frequency;
            _module.Seed = Seed;
            _module.UseDistance = UseDistance;
        }

        public override Node Create(Vector2 pos)
        {
            NodeModuleGeneratorVoronoi node = CreateInstance<NodeModuleGeneratorVoronoi>();
            node.rect = new Rect(pos.x, pos.y, NodeWidth, NodeHeight);
            node.name = "Voronoi";
            node.NodeWidth = 300.0f;
            node.NodeHeight = 500.0f;
            node.CreateOutput("Output", "Module", NodeSide.Right);

            return node;
        }

        protected override void NodeGUIProperties()
        {
            GUILayout.BeginVertical();

            Displacement = RTEditorGUI.FloatField(new GUIContent("Displacement", "Displacement value of voronoi cells"), Frequency);
            Frequency = RTEditorGUI.FloatField(new GUIContent("Frequency", "Frequency of the first octave"), Frequency);
            Seed = RTEditorGUI.IntField(new GUIContent("Seed", "Random Seed"), Seed);
            UseDistance = RTEditorGUI.Toggle(UseDistance, new GUIContent("UseDistance", "Apply Distance to Seed point to output value"));
            
            GUILayout.EndVertical();
        }
    }
}
