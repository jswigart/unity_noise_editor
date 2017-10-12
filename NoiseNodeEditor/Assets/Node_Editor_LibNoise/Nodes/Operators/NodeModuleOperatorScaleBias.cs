using UnityEngine;
using LibNoise.Operator;
using NodeEditorFramework;
using NodeEditorFramework.Utilities;

namespace LibNoiseNodes
{
    [System.Serializable]
    [Node(false, "Noise/Operator/ScaleBias")]
    public class NodeModuleOperatorScaleBias : NodeModuleBase
    {
        public override string GetID { get { return typeof(NodeModuleOperatorScaleBias).Name.ToLower(); } }
        
        public float Scale = 0.0f;
        public float Bias = 0.0f;

        ScaleBias _module = new ScaleBias();

        public NodeModuleOperatorScaleBias()
        {
            InitModule(_module);

            Scale = (float)_module.Scale;
            Bias = (float)_module.Bias;
        }

        protected override void PreGenerate()
        {
            _module.Scale = Scale;
            _module.Bias = Bias;
        }

        public override Node Create(Vector2 pos)
        {
            NodeModuleOperatorScaleBias node = CreateInstance<NodeModuleOperatorScaleBias>();
            node.rect = new Rect(pos.x, pos.y, NodeWidth, NodeHeight);
            node.name = "ScaleBias";
            node.NodeWidth = 300.0f;
            node.NodeHeight = 500.0f;

            node.CreateInput("Input", "Module", NodeSide.Left, 20);
            node.CreateOutput("Output", "Module", NodeSide.Right);
            
            return node;
        }
        
        protected override void NodeGUIProperties()
        {
            Scale = RTEditorGUI.FloatField(new GUIContent("Scale", "Scale to apply"), Scale);
            Bias = RTEditorGUI.FloatField(new GUIContent("Bias", "Bias to apply with scale"), Bias);
        }
    }
}

