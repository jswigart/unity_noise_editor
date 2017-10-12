using UnityEngine;
using LibNoise.Generator;
using NodeEditorFramework;
using NodeEditorFramework.Utilities;

namespace LibNoiseNodes
{
    [System.Serializable]
    [Node(false, "Noise/Generator/Checker")]
    public class NodeModuleGeneratorChecker : NodeModuleBase
    {
        public override string GetID { get { return typeof(NodeModuleGeneratorChecker).Name.ToLower(); } }
                
        Checker _module = new Checker();

        public NodeModuleGeneratorChecker()
        {
            InitModule(_module);
        }

        protected override void PreGenerate()
        {
        }

        public override Node Create(Vector2 pos)
        {
            NodeModuleGeneratorChecker node = CreateInstance<NodeModuleGeneratorChecker>();
            node.rect = new Rect(pos.x, pos.y, NodeWidth, NodeHeight);
            node.name = "Checker";
            node.NodeWidth = 300.0f;
            node.NodeHeight = 500.0f;
            node.CreateOutput("Output", "Module", NodeSide.Right);

            return node;
        }

        protected override void NodeGUIProperties()
        {
        }
    }
}
