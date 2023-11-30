using UnityEngine;

public class Water : MonoBehaviour
{
    Mesh mesh;
    Vector3[] vertices;
    int[] triangles;

    public int xSize = 20;
    public int zSize = 20;

    // Start is called before the first frame update
    void Start()
    {
        mesh = new();
        GetComponent<MeshFilter>().mesh = mesh;

        CreateWater();
        UpdateMesh();
    }

    private void Update()
    {
        if (vertices != null)
        {
            for (int i = 0; i < vertices.Length; i++)
            {
                Vector3 v = vertices[i];
                v.y = Mathf.Sin(0.5f * v.x + Time.time) * 0.5f;
                vertices[i] = v;
            }

            mesh.vertices = vertices;
            mesh.RecalculateNormals();
        }
    }

    private void UpdateMesh()
    {
        mesh.Clear();

        mesh.vertices = vertices;
        mesh.triangles = triangles;

        mesh.RecalculateNormals();
    }
    private void CreateWater()
    {
        vertices = new Vector3[(xSize + 1) * (zSize + 1)];

        for (int i = 0, z = 0; z < zSize + 1; z++)
        {
            for (int x = 0; x < xSize + 1; x++)
            {
                //float y = Mathf.PerlinNoise(x * .3f, z * .3f) * 2f;
                vertices[i] = new Vector3(x, 0, z);
                i++;
            }
        }

        triangles = new int[6 * xSize * zSize];
        int tries = 0;
        int vert = 0;
        for (int z = 0; z < zSize; z++)
        {
            for (int x = 0; x < xSize; x++)
            {
                // Draw one quad
                triangles[0 + tries] = vert + 0;
                triangles[1 + tries] = vert + xSize + 1;
                triangles[2 + tries] = vert + 1;
                triangles[3 + tries] = vert + 1;
                triangles[4 + tries] = vert + xSize + 1;
                triangles[5 + tries] = vert + xSize + 2;

                tries += 6;
                vert++;
            }
            vert++;
        }
    }

}
