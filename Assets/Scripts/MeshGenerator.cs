using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(MeshFilter))]
public class MeshGenerator : MonoBehaviour
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

        StartCoroutine(CreateShape());
    }
    private void Update()
    {
        UpdateMesh();
    }
    private void UpdateMesh()
    {
        mesh.Clear();

        mesh.vertices = vertices;
        mesh.triangles = triangles;

        mesh.RecalculateNormals();
    }

    private IEnumerator CreateShape()
    {
        vertices = new Vector3[(xSize + 1) * (zSize + 1)];

        for (int i = 0, z = 0; z < zSize + 1; z++)
        {
            for (int x = 0; x < xSize + 1; x++)
            {
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

                yield return new WaitForSeconds(.1f);
            }
            vert++;
        }
    }

    private void OnDrawGizmos()
    {
        if (vertices == null)
            return;
        for (int i = 0; i < vertices.Length; i++)
        {
            Gizmos.DrawSphere(vertices[i], .1f);
        }
    }
}
