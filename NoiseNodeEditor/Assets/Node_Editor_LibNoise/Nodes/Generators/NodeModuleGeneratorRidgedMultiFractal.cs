using UnityEngine;
using LibNoise.Generator;
using NodeEditorFramework;
using NodeEditorFramework.Utilities;

namespace LibNoiseNodes
{
    [System.Serializable]
    [Node(false, "Noise/Generator/RidgedMultiFractal")]
    public class NodeModuleGeneratorRidgedMultiFractal : NodeModuleBase
    {
        public override string GetID { get { return typeof(NodeModuleGeneratorRidgedMultiFractal).Name.ToLower(); } }
                
        public float Frequency = 1.0f;
        public float Lacunarity = 1.0f;
        public int OctaveCount = 2;
        public int Seed = 0;

        public LibNoise.QualityMode Quality;

        RidgedMultifractal _module = new RidgedMultifractal();

        public NodeModuleGeneratorRidgedMultiFractal()
        {
            // pull the defaults from the noise module
            Frequency = (float)_module.Frequency;
            Lacunarity = (float)_module.Lacunarity;
            OctaveCount = _module.OctaveCount;
            Seed = _module.Seed;
            Quality = _module.Quality;

            InitModule(_module);
        }

        protected override void PreGenerate()
        {
            _module.Frequency = (float)Frequency;
            _module.Lacunarity = (float)Lacunarity;
            _module.OctaveCount = OctaveCount;
            _module.Seed = Seed;
            _module.Quality = Quality;
        }

        public override Node Create(Vector2 pos)
        {
            NodeModuleGeneratorRidgedMultiFractal node = CreateInstance<NodeModuleGeneratorRidgedMultiFractal>();
            node.rect = new Rect(pos.x, pos.y, NodeWidth, NodeHeight);
            node.name = "RidgedMultiFractal";
            node.NodeWidth = 300.0f;
            node.NodeHeight = 500.0f;
            node.CreateOutput("Output", "Module", NodeSide.Right);

            return node;
        }

        protected override void NodeGUIProperties()
        {
            GUILayout.BeginVertical();
            
            Frequency = RTEditorGUI.FloatField(new GUIContent("Frequency", "Frequency of the first octave"), Frequency);
            Lacunarity = RTEditorGUI.FloatField(new GUIContent("Lacunarity", "Set Lacunarity"), Lacunarity);
            OctaveCount = RTEditorGUI.IntField(new GUIContent("OctaveCount", "How many octaves to generate"), OctaveCount);
            Seed = RTEditorGUI.IntField(new GUIContent("Seed", "Random Seed"), Seed);

#if UNITY_EDITOR
            Quality = (LibNoise.QualityMode)UnityEditor.EditorGUILayout.EnumPopup(new GUIContent("Quality", "Quality Mode for noise"), Quality);
#else
            GUILayout.Label (new GUIContent ("Quality: " + Quality.ToString (), "Quality Mode for noise"));
#endif            
            GUILayout.EndVertical();
        }
    }
}
