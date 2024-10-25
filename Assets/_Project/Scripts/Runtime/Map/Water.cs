using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class Water : MonoBehaviour
{
    public int Dimensions = 10;
    protected MeshFilter MeshFilter;
    protected Mesh Mesh;
    public Octave[] Octaves;
    public float UVScale = 2f;

    #region UNITY METHODS

    private void Start()
    {
        Mesh = new Mesh();
        Mesh.name = gameObject.name;
        Mesh.vertices = GenerateVerts();
        Mesh.triangles = GenerateTries();
        Mesh.uv = GenerateUVs();
        Mesh.RecalculateBounds();
        Mesh.RecalculateNormals();
        MeshFilter = gameObject.AddComponent<MeshFilter>();
        MeshFilter.mesh = Mesh;
    }

    private void Update()
    {
        Vector3[] verts = Mesh.vertices;
        for (int x = 0; x <= Dimensions; x++)
        {
            for (int z = 0; z <= Dimensions; z++)
            {
                float y = 0;
                for (int o = 0; o < Octaves.Length; o++)
                {
                    if (Octaves[o].Alternate)
                    {
                        float perl = Mathf.PerlinNoise((x * Octaves[o].Scale.x) / Dimensions, (z * Octaves[o].Scale.y) / Dimensions) * Mathf.PI * 2f;
                        y += Mathf.Sin(perl + Time.time * Octaves[o].Speed.magnitude) * Octaves[o].Height;
                    }
                    else
                    {
                        float perl = Mathf.PerlinNoise((x * Octaves[o].Scale.x + Time.time * Octaves[o].Speed.x) / Dimensions, (z * Octaves[o].Scale.y + Time.time * Octaves[o].Speed.y) / Dimensions) * Mathf.PI * 2f;
                        y += Mathf.Sin(perl) * Octaves[o].Height;
                    }
                }
                verts[Index(x, z)] = new Vector3(x, y, z);
            }
        }
        Mesh.vertices = verts;
        //Mesh.RecalculateBounds();
        Mesh.RecalculateNormals();
    }

    #endregion UNITY METHODS

    #region METHODS

    private Vector3[] GenerateVerts()
    {
        Vector3[] verts = new Vector3[(Dimensions + 1) * (Dimensions + 1)];
        for (int x = 0; x <= Dimensions; x++)
        {
            for (int z = 0; z <= Dimensions; z++)
            {
                verts[Index(x, z)] = new Vector3(x, 0, z);
            }
        }
        return verts;
    }

    private int Index(int x, int z)
    {
        return x * (Dimensions + 1) + z;
    }

    private int[] GenerateTries()
    {
        //int[] triangles = new int[Dimensions * Dimensions * 6];
        int[] triangles = new int[Mesh.vertices.Length * 6];
        for (int x = 0; x < Dimensions; x++)
        {
            for (int z = 0; z < Dimensions; z++)
            {
                triangles[Index(x, z) * 6 + 0] = Index(x, z);
                triangles[Index(x, z) * 6 + 1] = Index(x + 1, z + 1);
                triangles[Index(x, z) * 6 + 2] = Index(x + 1, z);
                triangles[Index(x, z) * 6 + 3] = Index(x, z);
                triangles[Index(x, z) * 6 + 4] = Index(x, z + 1);
                triangles[Index(x, z) * 6 + 5] = Index(x + 1, z + 1);
            }
        }
        return triangles;
    }

    private Vector2[] GenerateUVs()
    {
        Vector2[] uvs = new Vector2[Mesh.vertices.Length];
        for (int x = 0; x <= Dimensions; x++)
        {
            for (int z = 0; z <= Dimensions; z++)
            {
                Vector2 vec = new Vector2((x / UVScale) % 2, (z / UVScale) % 2);
                uvs[Index(x, z)] = new Vector2(vec.x <= 1 ? vec.x : 2 - vec.x, vec.y <= 1 ? vec.y : 2 - vec.y);
            }
        }
        return uvs;
    }

    public float GetHeight(Vector3 position)
    {
        Vector3 scale = new Vector3(1 / transform.lossyScale.x, 0, 1 / transform.lossyScale.z);
        Vector3 localPos = Vector3.Scale((position - transform.position), scale);

        Vector3 p1 = new Vector3(Mathf.Floor(localPos.x), 0, Mathf.Floor(localPos.z));
        Vector3 p2 = new Vector3(Mathf.Floor(localPos.x), 0, Mathf.Ceil(localPos.z));
        Vector3 p3 = new Vector3(Mathf.Ceil(localPos.x), 0, Mathf.Floor(localPos.z));
        Vector3 p4 = new Vector3(Mathf.Ceil(localPos.x), 0, Mathf.Ceil(localPos.z));

        p1.x = Mathf.Clamp(p1.x, 0, Dimensions);
        p1.z = Mathf.Clamp(p1.z, 0, Dimensions);
        p2.x = Mathf.Clamp(p2.x, 0, Dimensions);
        p2.z = Mathf.Clamp(p2.z, 0, Dimensions);
        p3.x = Mathf.Clamp(p3.x, 0, Dimensions);
        p3.z = Mathf.Clamp(p3.z, 0, Dimensions);
        p4.x = Mathf.Clamp(p4.x, 0, Dimensions);
        p4.z = Mathf.Clamp(p4.z, 0, Dimensions);

        float max = Mathf.Max(Vector3.Distance(p1, localPos), Vector3.Distance(p2, localPos), Vector3.Distance(p3, localPos), Vector3.Distance(p4, localPos) + Mathf.Epsilon);
        float dist = (max - Vector3.Distance(p1, localPos))
            + (max - Vector3.Distance(p2, localPos))
            + (max - Vector3.Distance(p3, localPos))
            + (max - Vector3.Distance(p4, localPos) + Mathf.Epsilon);

        var height = Mesh.vertices[Index((int)p1.x, (int)p1.z)].y * (max - Vector3.Distance(p1, localPos))
            + Mesh.vertices[Index((int)p2.x, (int)p2.z)].y * (max - Vector3.Distance(p2, localPos))
            + Mesh.vertices[Index((int)p3.x, (int)p3.z)].y * (max - Vector3.Distance(p3, localPos))
            + Mesh.vertices[Index((int)p4.x, (int)p4.z)].y * (max - Vector3.Distance(p4, localPos));

        return height * transform.lossyScale.y / dist;
    }

    #endregion METHODS

    #region STRUCTS

    [Serializable]
    public struct Octave
    {
        public bool Alternate;
        public Vector2 Speed;
        public float Height;
        public Vector2 Scale;
    }

    public struct WaterVertex
    {
        public Vector3 Position;
        public Vector3 Normal;
        public Vector2 UV;
    }

    #endregion STRUCTS
}