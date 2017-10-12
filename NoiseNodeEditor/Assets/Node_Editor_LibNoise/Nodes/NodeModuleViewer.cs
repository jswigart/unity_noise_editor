using UnityEngine;
using LibNoise.Operator;
using NodeEditorFramework;
using NodeEditorFramework.Utilities;

namespace LibNoiseNodes
{
    [System.Serializable]
    [Node(false, "Noise/Viewer")]
    public class NodeModuleViewer : Node
    {
        public override string GetID { get { return typeof(NodeModuleViewer).Name.ToLower(); } }

        public float PreviewLeft = -5.0f;
        public float PreviewRight = 5.0f;
        public float PreviewTop = -5.0f;
        public float PreviewBottom = 5.0f;
        public bool PreviewSeamless = false;
        public NodeModuleBase.ColorMode Colorization = NodeModuleBase.ColorMode.Terrain;

        public bool DependenciesMet { get; protected set; }

        const int _previewWidth = 512;
        const int _previewHeight = 512;
        protected Texture2D _previewTexture = null;
        
        int _nextSequenceId = -1;
        int _textureSequenceId = -1;

        public NodeModuleViewer()
        {
        }

        public override Node Create(Vector2 pos)
        {
            NodeModuleViewer node = CreateInstance<NodeModuleViewer>();
            node.rect = new Rect(pos.x, pos.y, _previewWidth + 20.0f, _previewHeight + 150.0f);
            node.name = "Viewer";

            node.CreateInput("Input", "Module", NodeSide.Left, 20);

            return node;
        }

        protected void NodeGUIProperties()
        {
        }

        protected override void NodeGUI()
        {
            GUILayout.BeginHorizontal();
            GUILayout.BeginVertical();

            for (int i = 0; i < Inputs.Count; ++i)
            {
                GUILayout.Label(Inputs[0].name);
                InputKnob(i);
            }

            GUILayout.EndVertical();
            GUILayout.EndHorizontal();

            NodeGUIProperties();

            DrawPreview();

            if (GUI.changed)
                NodeEditor.RecalculateFrom(this);
        }
        
        protected void DrawPreview()
        {
            Material mtrl = new Material(Shader.Find("Transparent/Diffuse"));
            mtrl.SetColor("_Color", Color.white);
            mtrl.SetTexture("_MainTex", _previewTexture);
            
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            Rect rect = GUILayoutUtility.GetRect(_previewWidth, _previewHeight, GUI.skin.box);
            if (_previewTexture != null)
            {
                //RTEditorGUI.DrawTexture(_previewTexture, _previewWidth, GUI.skin.box);
                //GUI.BeginGroup(rect);
                
                GUI.color = _textureSequenceId == _nextSequenceId ? Color.white : new Color(1.0f, 1.0f, 1.0f, 0.25f);
                GUI.DrawTexture(rect, _previewTexture, ScaleMode.ScaleToFit, true);
                GUI.color = Color.white;

                //UnityEditor.EditorGUI.DrawTextureAlpha(rect, _previewTexture);
                //UnityEditor.EditorGUI.DrawPreviewTexture(rect, _previewTexture);
                //Graphics.DrawTexture(rect, _previewTexture);
                //GUI.EndGroup();
            }

            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            using (var horizontalScope = new GUILayout.HorizontalScope("box"))
            {
                GUILayout.FlexibleSpace();
                const float sz = 70.0f;
                PreviewLeft = RTEditorGUI.FloatField("Left", PreviewLeft, GUILayout.Width(sz));
                PreviewRight = RTEditorGUI.FloatField("Right", PreviewRight, GUILayout.Width(sz));
                PreviewTop = RTEditorGUI.FloatField("Top", PreviewTop, GUILayout.Width(sz));
                PreviewBottom = RTEditorGUI.FloatField("Bottom", PreviewBottom, GUILayout.Width(sz));
                GUILayout.FlexibleSpace();
            }
            PreviewSeamless = RTEditorGUI.Toggle(PreviewSeamless, "Seamless");

#if UNITY_EDITOR
            Colorization = (NodeModuleBase.ColorMode)UnityEditor.EditorGUILayout.EnumPopup(new GUIContent("ColorMode", "Colorization method for preview"), Colorization);
#else
            GUILayout.Label(new GUIContent("Colorization: " + Colorization.ToString(), "Quality Mode for noise"));
#endif
        }
        
        public override bool Calculate()
        {
            LibNoise.ModuleBase noiseModule = null;

            var mnode = !Inputs[0].IsValueNull ? Inputs[0].GetValue<NodeModuleBase>() : null;
            if(mnode != null && mnode.DependenciesMet)
            {
                noiseModule = mnode.Module;
            }
            
            if (noiseModule != null)
            {
                if(_previewTexture == null)
                    _previewTexture = NodeModuleBase.GenerateDefaultTexture(_previewWidth, _previewHeight);

                ++_nextSequenceId;
                int asyncId = _nextSequenceId;

                AsyncWorkQueue.QueueBackgroundWork(GetType().Name, () =>
                {
                    // expensive stuff
                    LibNoise.Noise2D noise2d = new LibNoise.Noise2D(_previewWidth, _previewHeight, noiseModule);
                    noise2d.GeneratePlanar(PreviewLeft, PreviewRight, PreviewTop, PreviewBottom, PreviewSeamless);

                    // when we're done, queue the texture creation
                    AsyncWorkQueue.QueueMainThreadWork(() =>
                    {
                        if (asyncId >= _textureSequenceId)
                        {
                            _textureSequenceId = asyncId;
                            _previewTexture = NodeModuleBase.CreateNoiseTexture(noise2d, Colorization);

                            NodeEditorFramework.NodeEditor.RepaintClients();
                        }
                    });
                });                
            }
            else
            {
                _previewTexture = NodeModuleBase.GenerateDefaultTexture(_previewWidth, _previewHeight);
            }
            NodeEditorFramework.NodeEditor.RepaintClients();

            return true;
        }
    }
}

