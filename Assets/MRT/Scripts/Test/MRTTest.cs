using UnityEngine;
using System.Collections;

public class MRTTest : MonoBehaviour {

    MRT mrt;

    [SerializeField] Shader testShader;
    Material mat;

    void Start() {
        mrt = new MRT(Screen.width, Screen.height);
        mat = new Material(testShader);
    }

    void Update() {
        mrt.Render(mat);
    }

    void OnGUI() {
        var w = Screen.width * 0.5f;
        var h = Screen.width * 0.5f;
        var r00 = new Rect(0f, 0f, w, h);
        var r01 = new Rect(w, 0f, w, h);
        var r10 = new Rect(0f, h, w, h);

        var buffers = mrt.RenderTextures;
        GUI.DrawTexture(r00, buffers[0]);
        GUI.DrawTexture(r01, buffers[1]);
        GUI.DrawTexture(r10, buffers[2]);
    }

}
