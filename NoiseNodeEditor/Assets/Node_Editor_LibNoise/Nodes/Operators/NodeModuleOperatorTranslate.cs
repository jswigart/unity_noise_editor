using UnityEngine;
using LibNoise.Operator;
using NodeEditorFramework;
using NodeEditorFramework.Utilities;

namespace LibNoiseNodes
{
    [System.Serializable]
    [Node(false, "Noise/Operator/Translate")]
    public class NodeModuleOperatorTranslate : NodeModuleBase
    {
        public override string GetID { get { return typeof(NodeModuleOperatorTranslate).Name.ToLower(); } }
        
        public float TranslateX = 0.0f;
        public float TranslateY = 0.0f;
        public float TranslateZ = 0.0f;

        Translate _module = new Translate();

        public NodeModuleOperatorTranslate()
        {
            InitModule(_module);

            TranslateX = (float)_module.X;
            TranslateY = (float)_module.Y;
            TranslateZ = (float)_module.Z;
        }

        protected override void PreGenerate()
        {
            _module.X = TranslateX;
            _module.Y = TranslateY;
            _module.Z = TranslateZ;
        }

        public override Node Create(Vector2 pos)
        {
            NodeModuleOperatorTranslate node = CreateInstance<NodeModuleOperatorTranslate>();
            node.rect = new Rect(pos.x, pos.y, NodeWidth, NodeHeight);
            node.name = "Translate";
            node.NodeWidth = 300.0f;
            node.NodeHeight = 500.0f;

            node.CreateInput("Input", "Module", NodeSide.Left, 20);
            node.CreateOutput("Output", "Module", NodeSide.Right);
            
            return node;
        }
        
        protected override void NodeGUIProperties()
        {
            TranslateX = RTEditorGUI.FloatField(new GUIContent("Translate X", "Translate around X Axis"), TranslateX);
            TranslateY = RTEditorGUI.FloatField(new GUIContent("Translate Y", "Translate around Y Axis"), TranslateX);
            TranslateZ = RTEditorGUI.FloatField(new GUIContent("Translate Z", "Translate around Z Axis"), TranslateX);
        }
    }
}

