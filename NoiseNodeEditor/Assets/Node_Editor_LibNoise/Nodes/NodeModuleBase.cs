using System;
using System.Collections.Generic;
using UnityEngine;
using NodeEditorFramework;
using NodeEditorFramework.Utilities;

namespace LibNoiseNodes
{
    public abstract class NodeModuleBase : Node
    {
        public override bool AllowRecursion { get { return false; } }
        public override bool ContinueCalculation { get { return true; } }

        public enum ColorMode
        {
            Terrain,
            Greyscale,
            GreyscaleOpaque,
            RGB,
            RGBA,
        }
        
        public float PreviewLeft = -1.0f;
        public float PreviewRight = 1.0f;
        public float PreviewTop = -1.0f;
        public float PreviewBottom = 1.0f;
        public bool PreviewSeamless = false;
        public ColorMode Colorization = ColorMode.Terrain;

        public float NodeWidth { get; protected set; }
        public float NodeHeight { get; protected set; }

        public LibNoise.ModuleBase Module { get; protected set; }
        
        const int _previewWidth = 256;
        const int _previewHeight = 256;
        protected Texture2D _previewTexture = null;

        int _nextSequenceId = -1;
        int _textureSequenceId = -1;

        protected void InitModule(LibNoise.ModuleBase mod)
        {
            Module = mod;                      
        }

        protected void DrawPreview()
        {
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
            Colorization = (ColorMode)UnityEditor.EditorGUILayout.EnumPopup(new GUIContent("ColorMode", "Colorization method for preview"), Colorization);
#else
            GUILayout.Label (new GUIContent ("Colorization: " + Colorization.ToString (), "Quality Mode for noise"));
#endif

        }

        protected virtual void NodeGUIProperties()
        {
        }

        protected override void NodeGUI()
        {
            GUILayout.BeginHorizontal();
            GUILayout.BeginVertical();

            for( int i = 0; i < Inputs.Count; ++i )
            {
                GUILayout.Label(Inputs[i].name);
                InputKnob(i);
            }
           
            GUILayout.EndVertical();
            GUILayout.BeginVertical();

            for (int i = 0; i < Outputs.Count; ++i)
            {
                Outputs[i].DisplayLayout();
            }

            GUILayout.EndVertical();
            GUILayout.EndHorizontal();

            NodeGUIProperties();

            DrawPreview();

            if (GUI.changed)
                NodeEditor.RecalculateFrom(this);
        }

        protected virtual void PreGenerate()
        {
        }

        public bool DependenciesMet
        {
            get
            {
                bool met = true;

                for (int m = 0; m < Module.SourceModuleCount; ++m)
                {
                    bool SrcDependenciesMet = true;

                    if (Inputs[m].IsValueNull)
                    {
                        Module[m] = null;
                        return false;
                    }
                    else
                    {
                        var mnode = Inputs[m].GetValue<NodeModuleBase>();
                        SrcDependenciesMet = mnode.DependenciesMet;
                        Module[m] = mnode.Module;
                    }

                    met &= Module[m] != null && SrcDependenciesMet;
                }

                return met;
            }
        }
        
        public static Texture2D GenerateDefaultTexture(int width, int height)
        {
            // generate a default
            Texture2D tex = new Texture2D(width, height);
            var pixels = new Color[width * height];
            for (int i = 0; i < pixels.Length; ++i)
                pixels[i] = Color.magenta;
            tex.SetPixels(pixels);
            tex.Apply();
            return tex;
        }
        
        public static Texture2D CreateNoiseTexture(LibNoise.Noise2D noise, ColorMode colormode)
        {
            Texture2D tex = null;
            switch (colormode)
            {
                case ColorMode.Terrain:
                    tex = noise.GetTexture(LibNoise.GradientPresets.Terrain);
                    break;
                case ColorMode.Greyscale:
                    tex = noise.GetTexture(LibNoise.GradientPresets.Grayscale);
                    break;
                case ColorMode.GreyscaleOpaque:
                    tex = noise.GetTexture(LibNoise.GradientPresets.GrayscaleOpaque);
                    break;
                case ColorMode.RGB:
                    tex = noise.GetTexture(LibNoise.GradientPresets.RGB);
                    break;
                case ColorMode.RGBA:
                    tex = noise.GetTexture(LibNoise.GradientPresets.RGBA);
                    break;
            }
            return tex;
        }

        public override bool Calculate()
        {
            if (DependenciesMet)
            {
                PreGenerate();

                if (_previewTexture == null)
                    _previewTexture = GenerateDefaultTexture(_previewWidth, _previewHeight);

                ++_nextSequenceId;
                int asyncId = _nextSequenceId;

                AsyncWorkQueue.QueueBackgroundWork(GetType().Name, ()=>
                {
                    // expensive stuff
                    LibNoise.Noise2D noise2d = new LibNoise.Noise2D(_previewWidth, _previewHeight, Module);
                    noise2d.GeneratePlanar(PreviewLeft, PreviewRight, PreviewTop, PreviewBottom, PreviewSeamless);

                    // when we're done, queue the texture creation
                    AsyncWorkQueue.QueueMainThreadWork(() =>
                    {
                        if (asyncId >= _textureSequenceId)
                        {
                            _textureSequenceId = asyncId;
                            _previewTexture = CreateNoiseTexture(noise2d, Colorization);

                            NodeEditorFramework.NodeEditor.RepaintClients();
                        }
                    });
                });
            }
            else
            {
                _previewTexture = GenerateDefaultTexture(_previewWidth, _previewHeight);
            }
            NodeEditorFramework.NodeEditor.RepaintClients();

            for(int i = 0; i < Outputs.Count; ++i)
            {
                Outputs[i].SetValue<NodeModuleBase>(this);
            }            
            return true;
        }
    }

    // Connection Type only for visual purposes
    public class ModuleBaseType : IConnectionTypeDeclaration
    {
        public string Identifier { get { return "Module"; } }
        public Type Type { get { return typeof(NodeModuleBase); } }
        public Color Color { get { return Color.green; } }
        public string InKnobTex { get { return "Textures/In_Knob.png"; } }
        public string OutKnobTex { get { return "Textures/Out_Knob.png"; } }
    }
}
