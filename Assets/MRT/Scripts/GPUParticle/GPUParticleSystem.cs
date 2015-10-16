using UnityEngine;

using System.Collections;

public class GPUParticleSystem : MonoBehaviour {

    public GameObject System { get { return system; } }
    public MRT ReadMRT { get { return mrts[readIndex]; } }
    public MRT WriteMRT { get { return mrts[writeIndex]; } }

    [SerializeField] int vertexCount = 65000;
    [SerializeField] Material particleDisplayMat;
    [SerializeField] Material particleUpdateMat;

    [SerializeField] MRT[] mrts;
    int readIndex = 0;
    int writeIndex = 1;

    GameObject system;

    const int VERTEXLIMIT = 65000;

    GameObject Build(int vertCount, out int bufSize) {
        System.Type[] objectType = new System.Type[2];
        objectType[0] = typeof(MeshFilter);
        objectType[1] = typeof(MeshRenderer);

        GameObject go = new GameObject("ParticleMesh", objectType);

        Mesh particleMesh = new Mesh();
        particleMesh.name = vertCount.ToString();

        int vc = Mathf.Min(VERTEXLIMIT, vertCount);
        bufSize = Mathf.CeilToInt(Mathf.Sqrt(vertCount * 1.0f));

        Vector3[] verts = new Vector3[vc];
        Vector2[] texcoords = new Vector2[vc];

        int[] indices = new int[vc];

        for (int i = 0; i < vc; i++) {
            int k = i;

            float tx = 1f * (k % bufSize) / bufSize;
            float ty = 1f * (k / bufSize) / bufSize;

            verts[i] = Random.insideUnitSphere;
            texcoords[i] = new Vector2(tx, ty);
            indices[i] = i;
        }

        particleMesh.vertices = verts;
        particleMesh.uv = texcoords;
        particleMesh.uv2 = texcoords;

        particleMesh.SetIndices(indices, MeshTopology.Points, 0);
        particleMesh.RecalculateBounds();

        go.GetComponent<MeshRenderer>().material = particleDisplayMat;
        go.GetComponent<MeshFilter>().sharedMesh = particleMesh;

        return go;
    }

    void Start() {
        int bufSize;
        system = Build(vertexCount, out bufSize);
        system.transform.parent = transform;

        mrts = new MRT[2];
        for(int i = 0, n = mrts.Length; i < n; i++) {
            mrts[i] = new MRT(bufSize, bufSize);
        }

        ReadMRT.Render(particleUpdateMat, 0); // init

    }

    void Update() {
        var buffers = ReadMRT.RenderTextures;

        particleUpdateMat.SetTexture("_PosTex", buffers[0]);
        particleUpdateMat.SetTexture("_VelTex", buffers[1]);
        particleUpdateMat.SetTexture("_AccTex", buffers[2]);
        particleUpdateMat.SetVector("_TrackingTo", 2f * new Vector3(Mathf.Cos(Time.timeSinceLevelLoad), Mathf.Sin(Time.timeSinceLevelLoad), Mathf.Cos(Time.timeSinceLevelLoad)));

        WriteMRT.Render(particleUpdateMat, 1); // update

        Swap();

        particleDisplayMat.SetTexture("_PosTex", ReadMRT.RenderTextures[0]);
    }

    void Swap() {
        var tmp = readIndex;
        readIndex = writeIndex;
        writeIndex = tmp;
    }

    void OnDestroy() {
        for(int i = 0, n = mrts.Length; i < n; i++) {
            mrts[i].Release();
        }
    }

}
