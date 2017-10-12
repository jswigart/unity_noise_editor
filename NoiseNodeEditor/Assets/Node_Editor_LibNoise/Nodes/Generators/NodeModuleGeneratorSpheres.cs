using UnityEngine;
using LibNoise.Generator;
using NodeEditorFramework;
using NodeEditorFramework.Utilities;

namespace LibNoiseNodes
{
    [System.Serializable]
    [Node(false, "Noise/Generator/Spheres")]
    public class NodeModuleGeneratorSpheres : NodeModuleBase
    {
        public override string GetID { get { return typeof(NodeModuleGeneratorSpheres).Name.ToLower(); } }
                
        public float Frequency = 1.0f;

        Spheres _module = new Spheres();

        public NodeModuleGeneratorSpheres()
        {
            // pull the defaults from the noise module
            Frequency = (float)_module.Frequency;

            InitModule(_module);
        }

        protected override void PreGenerate()
        {
            _module.Frequency = (float)Frequency;
        }

        public override Node Create(Vector2 pos)
        {
            NodeModuleGeneratorSpheres node = CreateInstance<NodeModuleGeneratorSpheres>();
            node.rect = new Rect(pos.x, pos.y, NodeWidth, NodeHeight);
            node.name = "Spheres";
            node.NodeWidth = 300.0f;
            node.NodeHeight = 500.0f;
            node.CreateOutput("Output", "Module", NodeSide.Right);

            return node;
        }

        protected override void NodeGUIProperties()
        {
            GUILayout.BeginVertical();
            
            Frequency = RTEditorGUI.FloatField(new GUIContent("Frequency", "Frequency of the first octave"), Frequency);
                       
            GUILayout.EndVertical();
        }
    }
}
