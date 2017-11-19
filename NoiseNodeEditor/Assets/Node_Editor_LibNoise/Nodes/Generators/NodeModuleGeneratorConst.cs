using UnityEngine;
using LibNoise.Generator;
using NodeEditorFramework;
using NodeEditorFramework.Utilities;

namespace LibNoiseNodes
{
    [System.Serializable]
    [Node(false, "Noise/Generator/Const")]
    public class NodeModuleGeneratorConst : NodeModuleBase
    {
        public override string GetID { get { return typeof(NodeModuleGeneratorConst).Name.ToLower(); } }
                
        public float Value = 1.0f;

        Const _module = new Const();

        public NodeModuleGeneratorConst()
        {
            // pull the defaults from the noise module
            Value = (float)_module.Value;

            InitModule(_module);
        }

        protected override void PreGenerate()
        {
            _module.Value = (float)Value;
        }

        public override Node Create(Vector2 pos)
        {
            NodeModuleGeneratorConst node = CreateInstance<NodeModuleGeneratorConst>();
            node.rect = new Rect(pos.x, pos.y, NodeWidth, NodeHeight);
            node.name = "Const";
            node.NodeWidth = 300.0f;
            node.NodeHeight = 500.0f;
            node.CreateOutput("Output", "Module", NodeSide.Right);

            return node;
        }

        protected override void NodeGUIProperties()
        {
            GUILayout.BeginVertical();

            Value = RTEditorGUI.FloatField(new GUIContent("Value", "Constant Value"), Value);
                       
            GUILayout.EndVertical();
        }
    }
}
