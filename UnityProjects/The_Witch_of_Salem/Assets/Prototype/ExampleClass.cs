using UnityEngine;
using System.Collections;

public class ExampleClass : MonoBehaviour
{
    public Vector3[] vertices;
    public Color[] colors;

    void Start()
    {
        Mesh mesh = GetComponent<MeshFilter>().mesh;
        vertices = mesh.vertices;

        // create new colors array where the colors will be created.
        colors = new Color[vertices.Length];

        for (int i = 0; i < vertices.Length; i++)
            colors[i] = Color.Lerp(Color.blue, Color.green, vertices[i].y);

        // assign the array of colors to the Mesh.
        mesh.colors = colors;
    }
}
