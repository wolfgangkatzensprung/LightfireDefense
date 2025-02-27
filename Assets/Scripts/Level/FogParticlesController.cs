using UnityEngine;

public class FogParticlesController : MonoBehaviour
{
    public Material fogParticlesMaterial;


    // -> alte Methode mit Mesh Calculation
    public ParticleSystem particleSystem;
    private ParticleSystem.ShapeModule shapeModule;
    public Mesh originalMesh;
    private Mesh currentMesh;

    private const float TERRAIN_SCALE = 100;
    internal float currentSphereRadius = 25f;
    // <-

    private void Update()
    {
        fogParticlesMaterial.SetVector("_PlayerPosition", GlobalInfo.Instance.playerTrans.position);

    }

}

//    void Start()
//    {
//        shapeModule = particleSystem.shape;
//        currentMesh = new Mesh();
//        currentMesh.vertices = originalMesh.vertices.Clone() as Vector3[];
//        currentMesh.triangles = originalMesh.triangles.Clone() as int[];
//        currentMesh.normals = originalMesh.normals.Clone() as Vector3[];

//        UpdateExclusionMesh();
//    }

//    public void UpdateExclusionMesh()
//    {
//        currentSphereRadius = LighthouseManager.Instance.lighthouseRange;
//        Mesh updatedMesh = GetSphereMesh(currentSphereRadius / TERRAIN_SCALE);
//        shapeModule.mesh = updatedMesh;
//    }

//    public Mesh GetSphereMesh(float radius)
//    {
//        Mesh mesh = new Mesh();

//        int resolution = 20; // Adjust the resolution as needed

//        // Vertices
//        Vector3[] vertices = new Vector3[(resolution + 1) * (resolution + 1)];
//        for (int i = 0, y = 0; y <= resolution; y++)
//        {
//            for (int x = 0; x <= resolution; x++, i++)
//            {
//                float u = x / (float)resolution;
//                float v = y / (float)resolution;

//                float theta = u * 2f * Mathf.PI;
//                float phi = v * Mathf.PI;

//                float xPos = radius * Mathf.Sin(phi) * Mathf.Cos(theta);
//                float yPos = radius * Mathf.Cos(phi);
//                float zPos = radius * Mathf.Sin(phi) * Mathf.Sin(theta);

//                vertices[i] = new Vector3(xPos, yPos, zPos);
//            }
//        }

//        // Triangles
//        int[] triangles = new int[resolution * resolution * 6];
//        for (int ti = 0, vi = 0, y = 0; y < resolution; y++, vi++)
//        {
//            for (int x = 0; x < resolution; x++, ti += 6, vi++)
//            {
//                triangles[ti] = vi;
//                triangles[ti + 3] = triangles[ti + 2] = vi + 1;
//                triangles[ti + 4] = triangles[ti + 1] = vi + resolution + 1;
//                triangles[ti + 5] = vi + resolution + 2;
//            }
//        }

//        // Assigning vertices and triangles to the mesh
//        mesh.vertices = vertices;
//        mesh.triangles = triangles;

//        // Recalculate normals for lighting
//        mesh.RecalculateNormals();

//        return mesh;
//    }
//}
