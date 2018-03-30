using UnityEngine;
using LibNoise.Operator;
using NodeEditorFramework;
using NodeEditorFramework.Utilities;

namespace LibNoiseNodes
{
    [System.Serializable]
    [Node(false, "Noise/Cellular Automata")]
    public class NodeModuleCellularAutomata : Node
    {
        public override string GetID { get { return typeof(NodeModuleCellularAutomata).Name.ToLower(); } }

        public float Cutoff = 0.0f;
        public int SmoothIterations = 3;

        public float PreviewLeft = -5.0f;
        public float PreviewRight = 5.0f;
        public float PreviewTop = -5.0f;
        public float PreviewBottom = 5.0f;
        public bool PreviewSeamless = false;
        
        public bool DependenciesMet { get; protected set; }

        const int _previewWidth = 512;
        const int _previewHeight = 512;
        protected Texture2D _previewTexture = null;
        
        int _nextSequenceId = -1;
        int _textureSequenceId = -1;

        public NodeModuleCellularAutomata()
        {
        }

        public override Node Create(Vector2 pos)
        {
            NodeModuleCellularAutomata node = CreateInstance<NodeModuleCellularAutomata>();
            node.rect = new Rect(pos.x, pos.y, _previewWidth + 20.0f, _previewHeight + 150.0f);
            node.name = "Cellular Automata";

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

            Cutoff = RTEditorGUI.FloatField("Cutoff", Cutoff);
            SmoothIterations = RTEditorGUI.IntField("Smooth Iterations", SmoothIterations);

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
        }

        static int CountNeighbors(bool[,] map, int gridX, int gridY)
        {
            int wallCount = 0;
            for (int neighbourX = gridX - 1; neighbourX <= gridX + 1; neighbourX++)
            {
                for (int neighbourY = gridY - 1; neighbourY <= gridY + 1; neighbourY++)
                {
                    if (neighbourX >= 0 && neighbourX < map.GetLength(0) && neighbourY >= 0 && neighbourY < map.GetLength(1))
                    {
                        if (neighbourX != gridX || neighbourY != gridY)
                        {
                            wallCount += map[neighbourX, neighbourY] ? 1 : 0;
                        }
                    }
                    else
                    {
                        wallCount++;
                    }
                }
            }

            return wallCount;
        }

        static void SmoothMap(bool[,] src, bool[,] dst)
        {
            for (int x = 0; x < src.GetLength(0); x++)
            {
                for (int y = 0; y < src.GetLength(1); y++)
                {
                    int neighbors = CountNeighbors(src, x, y);

                    if (neighbors > 4)
                        dst[x, y] = true;
                    else if (neighbors < 4)
                        dst[x, y] = false;
                }
            }
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
                    LibNoise.Noise2D noise2d = new LibNoise.Noise2D(_previewWidth, _previewHeight, noiseModule);
                    noise2d.GeneratePlanar(PreviewLeft, PreviewRight, PreviewTop, PreviewBottom, PreviewSeamless);

                    float[,] noiseData = noise2d.GetData();

                    bool[,] cellular = new bool[noiseData.GetLength(0), noiseData.GetLength(1)];

                    for (var x = 0; x < noiseData.GetLength(0); x++)
                    {
                        for (var y = 0; y < noiseData.GetLength(1); y++)
                        {
                            cellular[x, y] = noiseData[x, y] > Cutoff;
                        }
                    }

                    bool[,] dstMap = new bool[noiseData.GetLength(0), noiseData.GetLength(1)];
                    for (int i = 0; i < SmoothIterations; ++i)
                    {
                        SmoothMap(cellular, dstMap);

                        // swap the array references
                        bool[,] tmp = cellular;
                        cellular = dstMap;
                        dstMap = tmp;
                    }

                    // when we're done, queue the texture creation
                    AsyncWorkQueue.QueueMainThreadWork(() =>
                    {
                        if (asyncId >= _textureSequenceId)
                        {
                            _textureSequenceId = asyncId;

                            var pixels = new Color[cellular.GetLength(0) * cellular.GetLength(1)];

                            _previewTexture = new Texture2D(cellular.GetLength(0), cellular.GetLength(1));
                            _previewTexture.wrapMode = TextureWrapMode.Clamp;
                            _previewTexture.alphaIsTransparency = true;

                            for (var x = 0; x < noiseData.GetLength(0); x++)
                            {
                                for (var y = 0; y < noiseData.GetLength(1); y++)
                                {
                                    pixels[x + y * noiseData.GetLength(0)] = cellular[x, y] ? Color.black : Color.white;
                                }
                            }

                            _previewTexture.SetPixels(pixels);
                            _previewTexture.Apply();

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

