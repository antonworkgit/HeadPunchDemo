using Scripts.Helpers.Rendering;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace Scripts.Interactive.Painting
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Renderer), typeof(Rigidbody), typeof(MeshCollider))]
    public sealed class Paintable : MonoBehaviour, IPaintable
    {
        private Renderer _renderer;
        private Material _material;

        #region Graphic

        private const int TexSize = 1024;

        private const string SplatmaskShaderName = "Ink/Splatmask";
        private const string BlendShaderName = "Ink/Blend";

        private Material _splatmaskMat;
        private Material _blendMat;

        private RenderTexture _mainTexture;
        private RenderTexture _inkTexture;
        private RenderTexture _tmpTexture;

        private CommandBuffer _commandBuffer;

        #endregion

        private List<Color> _paintedColors;

        private void Start()
        {
            _renderer = GetComponent<Renderer>();
            _material = _renderer.material;

            if (_material.mainTexture == null)
            {
                _mainTexture = new RenderTexture(TexSize, TexSize, 0, RenderTextureFormat.ARGBFloat);
                _mainTexture.Create();

                Graphics.Blit(CommonTextures.WhitePixel, _mainTexture);
                _material.mainTexture = _mainTexture;
            }

            Shader splatmaskShader = Shader.Find(SplatmaskShaderName);
            Shader blendShader = Shader.Find(BlendShaderName);

            _splatmaskMat = new Material(splatmaskShader);
            _blendMat = new Material(blendShader);

            _inkTexture = new RenderTexture(TexSize, TexSize, 0, RenderTextureFormat.ARGBFloat);
            _inkTexture.Create();

            _tmpTexture = new RenderTexture(TexSize, TexSize, 0, RenderTextureFormat.ARGBFloat);
            _tmpTexture.Create();

            _renderer.material.SetTexture("_PaintTex", _inkTexture);

            _commandBuffer = new CommandBuffer();
            _paintedColors = new List<Color>();
        }

        private void OnDestroy()
        {
            if (_mainTexture != null)
                _mainTexture.Release();

            _inkTexture.Release();
            _tmpTexture.Release();
        }

        public Color GetMostColor()
        {
            if (_paintedColors.Count == 0)
            {
                Debug.LogWarning("No colors have been painted", gameObject);
                return Color.clear;
            }

            return Helpers.ColorMixing.ColorMixer.GetBlendedColor(_paintedColors);
        }

        // TODO: UV islands seamless
        public void Paint(Vector3 hitPosition, PaintParams paintParams)
        {
            _commandBuffer.Clear();

            _splatmaskMat.SetVector(Shader.PropertyToID("_SplatPos"), hitPosition);
            _splatmaskMat.SetVector(Shader.PropertyToID("_InkColor"), paintParams.inkColor);
            _splatmaskMat.SetFloat(Shader.PropertyToID("_Radius"), paintParams.radius);
            _splatmaskMat.SetFloat(Shader.PropertyToID("_Strength"), paintParams.strength);
            _splatmaskMat.SetFloat(Shader.PropertyToID("_Hardness"), paintParams.hardness);

            _commandBuffer.SetRenderTarget(_tmpTexture);
            _commandBuffer.DrawRenderer(_renderer, _splatmaskMat);

            _commandBuffer.SetRenderTarget(_inkTexture);
            _commandBuffer.Blit(_tmpTexture, _inkTexture, _blendMat);

            Graphics.ExecuteCommandBuffer(_commandBuffer);

            _paintedColors.Add(paintParams.inkColor);
        }
    }
}