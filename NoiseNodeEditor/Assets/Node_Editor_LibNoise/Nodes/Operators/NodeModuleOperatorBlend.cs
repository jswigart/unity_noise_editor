﻿using UnityEngine;
using LibNoise.Operator;
using NodeEditorFramework;
using NodeEditorFramework.Utilities;

namespace LibNoiseNodes
{
    [System.Serializable]
    [Node(false, "Noise/Operator/Blend")]
    public class NodeModuleOperatorBlend : NodeModuleBase
    {
        public override string GetID { get { return typeof(NodeModuleOperatorBlend).Name.ToLower(); } }
        
        Blend _module = new Blend();

        public NodeModuleOperatorBlend()
        {
            InitModule(_module);
        }

        public override Node Create(Vector2 pos)
        {
            NodeModuleOperatorBlend node = CreateInstance<NodeModuleOperatorBlend>();
            node.rect = new Rect(pos.x, pos.y, NodeWidth, NodeHeight);
            node.name = "Blend";
            node.NodeWidth = 300.0f;
            node.NodeHeight = 500.0f;

            node.CreateInput("Input 1", "Module", NodeSide.Left, 20);
            node.CreateInput("Input 2", "Module", NodeSide.Left, 20);

            node.CreateInput("Blend", "Module", NodeSide.Left, 20);

            node.CreateOutput("Output", "Module", NodeSide.Right);

            return node;
        }
        
        protected override void NodeGUIProperties()
        {            
        }
    }
}

