using UnityEngine;

public class TerrainMask : MonoBehaviour
{
    public MeshFilter terrainMeshFilter;
    public float sphereRadiusInGameUnits = 5.0f; // Set your desired sphere radius in game units

    void Start()
    {
        DeleteVerticesInsideSphere();
    }

    void DeleteVerticesInsideSphere()
    {
        Mesh terrainMesh = terrainMeshFilter.mesh;
        Vector3[] originalVertices = terrainMesh.vertices;
        int[] originalTriangles = terrainMesh.triangles;
        Vector3[] originalNormals = terrainMesh.normals;
        Vector2[] originalUVs = terrainMesh.uv;

        // Get the center of the sphere (you might want to adjust this based on your needs)
        Vector3 sphereCenter = new Vector3(0.0f, 10.0f, 0.0f);

        // Create lists to store the new vertices, triangles, normals, and UVs
        var newVertices = new System.Collections.Generic.List<Vector3>();
        var newTriangles = new System.Collections.Generic.List<int>();
        var newNormals = new System.Collections.Generic.List<Vector3>();
        var newUVs = new System.Collections.Generic.List<Vector2>();

        // Iterate through the original triangles
        for (int i = 0; i < originalTriangles.Length; i += 3)
        {
            // Check each vertex of the triangle
            bool triangleOutsideSphere = true;

            for (int j = 0; j < 3; j++)
            {
                int vertexIndex = originalTriangles[i + j];

                // Calculate the distance from the vertex to the sphere center in world space
                float distanceToSphere = Vector3.Distance(originalVertices[vertexIndex], sphereCenter);

                // If any vertex of the triangle is inside the sphere, mark the whole triangle as inside
                if (distanceToSphere <= sphereRadiusInGameUnits)
                {
                    triangleOutsideSphere = false;
                    break;
                }
            }

            // If the entire triangle is outside the sphere, add its vertices, normals, and UVs to the new lists
            if (triangleOutsideSphere)
            {
                for (int j = 0; j < 3; j++)
                {
                    int vertexIndex = originalTriangles[i + j];
                    newVertices.Add(originalVertices[vertexIndex]);
                    newNormals.Add(originalNormals[vertexIndex]);
                    newUVs.Add(originalUVs[vertexIndex]);
                    newTriangles.Add(newVertices.Count - 1); // Add the new index
                }
            }
        }

        // Update the mesh with the new set of vertices, triangles, normals, and UVs
        terrainMesh.SetVertices(newVertices);
        terrainMesh.SetNormals(newNormals);
        terrainMesh.SetUVs(0, newUVs);
        terrainMesh.SetTriangles(newTriangles, 0);
        terrainMesh.RecalculateNormals();
    }
}
