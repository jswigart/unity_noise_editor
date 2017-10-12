using UnityEngine;
using LibNoise.Operator;
using NodeEditorFramework;
using NodeEditorFramework.Utilities;

namespace LibNoiseNodes
{
    [System.Serializable]
    [Node(false, "Noise/Operator/Scale")]
    public class NodeModuleOperatorScale : NodeModuleBase
    {
        public override string GetID { get { return typeof(NodeModuleOperatorScale).Name.ToLower(); } }
        
        public float ScaleX = 0.0f;
        public float ScaleY = 0.0f;
        public float ScaleZ = 0.0f;

        Scale _module = new Scale();

        public NodeModuleOperatorScale()
        {
            InitModule(_module);

            ScaleX = (float)_module.X;
            ScaleY = (float)_module.Y;
            ScaleZ = (float)_module.Z;
        }

        protected override void PreGenerate()
        {
            _module.X = ScaleX;
            _module.Y = ScaleY;
            _module.Z = ScaleZ;
        }

        public override Node Create(Vector2 pos)
        {
            NodeModuleOperatorScale node = CreateInstance<NodeModuleOperatorScale>();
            node.rect = new Rect(pos.x, pos.y, NodeWidth, NodeHeight);
            node.name = "Scale";
            node.NodeWidth = 300.0f;
            node.NodeHeight = 500.0f;

            node.CreateInput("Input", "Module", NodeSide.Left, 20);
            node.CreateOutput("Output", "Module", NodeSide.Right);
            
            return node;
        }
        
        protected override void NodeGUIProperties()
        {
            ScaleX = RTEditorGUI.FloatField(new GUIContent("Scale X", "Scale on X Axis"), ScaleX);
            ScaleY = RTEditorGUI.FloatField(new GUIContent("Scale Y", "Scale on around Y Axis"), ScaleY);
            ScaleZ = RTEditorGUI.FloatField(new GUIContent("Scale Z", "Scale on around Z Axis"), ScaleZ);
        }
    }
}

