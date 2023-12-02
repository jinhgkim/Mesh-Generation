using System;
using UnityEngine;

public class Water : MonoBehaviour
{
    Mesh mesh;
    Vector3[] vertices;
    int[] triangles;

    public int xSize = 20;
    public int zSize = 20;

    Wave w;
    [SerializeField] float amplitude = .5f;
    [SerializeField] float waveLength = 2 * (float)Math.PI;
    [SerializeField] float frequency = .5f;

    struct Wave
    {
        public float amplitude;
        public float frequency;
        public float phase;
        public float k_waveNumber;

        public Wave(float amplitude, float waveLength, float speed, float phase = .0f)
        {
            this.amplitude = amplitude;
            this.k_waveNumber = 2 * (float)Math.PI / waveLength;
            this.frequency = 2 * (float)Math.PI * speed;
            this.phase = phase;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        mesh = new();
        GetComponent<MeshFilter>().mesh = mesh;

        CreateWater();
        UpdateMesh();

        w = new Wave(amplitude, waveLength, frequency);
    }

    private void Update()
    {
        if (vertices != null)
        {
            for (int i = 0; i < vertices.Length; i++)
            {
                Vector3 v = vertices[i];
                v.y = w.amplitude * Mathf.Sin(w.k_waveNumber * v.x + w.frequency * Time.time + w.phase);
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
