using UnityEngine;
using LibNoise.Operator;
using NodeEditorFramework;
using NodeEditorFramework.Utilities;

namespace LibNoiseNodes
{
    [System.Serializable]
    [Node(false, "Noise/Operator/Rotate")]
    public class NodeModuleOperatorRotate : NodeModuleBase
    {
        public override string GetID { get { return typeof(NodeModuleOperatorRotate).Name.ToLower(); } }
        
        public float RotateX = 0.0f;
        public float RotateY = 0.0f;
        public float RotateZ = 0.0f;

        Rotate _module = new Rotate();

        public NodeModuleOperatorRotate()
        {
            InitModule(_module);

            RotateX = (float)_module.X;
            RotateY = (float)_module.Y;
            RotateZ = (float)_module.Z;
        }

        protected override void PreGenerate()
        {
            _module.X = RotateX;
            _module.Y = RotateY;
            _module.Z = RotateZ;
        }

        public override Node Create(Vector2 pos)
        {
            NodeModuleOperatorRotate node = CreateInstance<NodeModuleOperatorRotate>();
            node.rect = new Rect(pos.x, pos.y, NodeWidth, NodeHeight);
            node.name = "Rotate";
            node.NodeWidth = 300.0f;
            node.NodeHeight = 500.0f;

            node.CreateInput("Input", "Module", NodeSide.Left, 20);
            node.CreateOutput("Output", "Module", NodeSide.Right);
            
            return node;
        }
        
        protected override void NodeGUIProperties()
        {
            RotateX = RTEditorGUI.FloatField(new GUIContent("Rotate X", "Rotation around X Axis"), RotateX);
            RotateY = RTEditorGUI.FloatField(new GUIContent("Rotate Y", "Rotation around Y Axis"), RotateX);
            RotateZ = RTEditorGUI.FloatField(new GUIContent("Rotate Z", "Rotation around Z Axis"), RotateX);
        }
    }
}

