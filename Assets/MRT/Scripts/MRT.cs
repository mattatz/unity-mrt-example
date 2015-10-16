using UnityEngine;

using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class MRT {

    public RenderTexture[] RenderTextures { get { return RTs; } }
    public RenderBuffer[] ColorBuffers { get { return RTs.Select(rt => rt.colorBuffer).ToArray(); } }
    public RenderBuffer DepthBuffer { get { return RTs[0].depthBuffer; } }

    [SerializeField] RenderTexture[] RTs;

    public MRT(int width, int height, RenderTextureFormat format = RenderTextureFormat.ARGBFloat, FilterMode filterMode = FilterMode.Point, TextureWrapMode wrapMode = TextureWrapMode.Repeat) {
        RTs = new RenderTexture[3];
        for(int i = 0, n = RTs.Length; i < n; i++) {
            int depth = (i == 0) ? 24 : 0;
            RTs[i] = new RenderTexture(width, height, depth);
			RTs[i].hideFlags = HideFlags.DontSave;
            RTs[i].format = format;
			RTs[i].filterMode = filterMode;
			RTs[i].wrapMode = wrapMode;
            RTs[i].Create();
        }
    }

    public void Render (Material mat, int pass = 0) {
        Graphics.SetRenderTarget(ColorBuffers, DepthBuffer);

        GL.PushMatrix();

        mat.SetPass(pass);

        GL.LoadOrtho();

        GL.Begin(GL.QUADS);

        GL.TexCoord(new Vector3(0, 0, 0));
        GL.Vertex3(0, 0, 0);

        GL.TexCoord(new Vector3(1, 0, 0));
        GL.Vertex3(1, 0, 0);

        GL.TexCoord(new Vector3(1, 1, 0));
        GL.Vertex3(1, 1, 0);

        GL.TexCoord(new Vector3(0, 1, 0));
        GL.Vertex3(0, 1, 0);

        GL.End();

        GL.PopMatrix();
    }

    public void Release () {
        for(int i = 0, n = RTs.Length; i < n; i++) {
            RTs[i].Release();
        }
    }

}
