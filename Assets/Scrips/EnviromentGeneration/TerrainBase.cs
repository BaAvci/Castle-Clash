using System;
using System.Threading;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public struct TerrainLayer
{
    [Range(0f, 1f)]
    public float HeighThreshold;
    public Texture2D Texture;
    public float Tiling;
}

[RequireComponent(typeof(MeshRenderer), typeof(MeshFilter))]
public abstract class TerrainBase : MonoBehaviour
{
    [SerializeField]
    protected int resolution = 512;
    [SerializeField]
    protected int textureResolution = 512;

    [SerializeField]
    protected float noiseScale = 0.3f;
    [SerializeField]
    protected int noiseOctaves = 4;

    protected float noiseHeight;
    protected MeshFilter meshFilter;
    protected MeshRenderer meshRenderer;
    protected MeshCollider meshCollider;
    protected Vector2Int tileGridEndPosition;
    protected int meshCenter;

    protected float[,] heightMap;

    protected virtual void Awake()
    {
        meshFilter = GetComponent<MeshFilter>();
        meshRenderer = GetComponent<MeshRenderer>();
    }

    public void CreateEnviroment(Vector2Int tileGridSize, float noiseHeight, int randomSeed)
    {
        if (meshRenderer == null)
        {
            meshRenderer = GetComponent<MeshRenderer>();
        }
        if (meshFilter == null)
        {
            meshFilter = GetComponent<MeshFilter>();
        }
        if (meshCollider == null)
        {
            meshCollider = GetComponent<MeshCollider>();
        }
        this.noiseHeight = noiseHeight;
        CalculateTileGridPosition(tileGridSize);
        GenerateHeightMap(out float minHeight, out float maxHeight, randomSeed);

        GenerateTexture(minHeight, maxHeight);

        GenerateMesh();
        meshCollider.sharedMesh = meshFilter.sharedMesh;
    }

    protected virtual void GenerateHeightMap(out float minHeight, out float maxHeight, int randomSeed)
    {
        maxHeight = float.MinValue;
        minHeight = float.MaxValue;

        System.Random rand = new(randomSeed);
        float offset = (float)rand.NextDouble();

        heightMap = new float[textureResolution, textureResolution];
        float noiseMultiplier = 1f / (noiseScale * textureResolution);   //Maybe textureResolution?

        for (int y = 0; y < textureResolution; y++)
        {
            int clampedY = Mathf.Clamp(y, meshCenter - 20, tileGridEndPosition.y + 20);
            for (int x = 0; x < textureResolution; x++)
            {
                int clampedX = Mathf.Clamp(x, meshCenter - 20, tileGridEndPosition.x + 20);
                float xDist = Mathf.Abs(clampedX - x);
                float yDist = Mathf.Abs(clampedY - y);
                float noiseMultiplierModificator = (xDist + yDist) / (float)textureResolution;

                float height = 0;
                for (int o = 1; o <= noiseOctaves; o++)
                {
                    float xCoord = x * o * noiseMultiplier;
                    float yCoord = y * o * noiseMultiplier;
                    float noiseValue = Mathf.PerlinNoise(xCoord + offset, yCoord + offset); //Gets perlin noise value
                    noiseValue *= noiseHeight; //Before: 0-1. Now: 0 - noiseHeight
                    noiseValue /= o;            //Makes the noise value less impactful with each octave

                    height += noiseValue;
                }
                height *= noiseMultiplierModificator;
                height -= 0.001f; // sets the Mesh position back to the normal hight

                heightMap[x, y] = height;

                if (height > maxHeight)
                {
                    maxHeight = height;
                }
                if (height < minHeight)
                {
                    minHeight = height;
                }
            }
        }
    }
    protected abstract void GenerateTexture(float minHeight, float maxHeight);

    protected virtual void GenerateMesh()
    {
        Mesh mesh = new Mesh();

        mesh.name = "Terrain Mesh";
        mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;

        Vector3[] vertices = new Vector3[resolution * resolution];
        Vector2[] uv = new Vector2[resolution * resolution];
        int index = 0;
        int resolutionFactor = textureResolution / resolution;

        for (int y = 0; y < resolution; y++)
        {
            for (int x = 0; x < resolution; x++)
            {
                vertices[index] = new Vector3(x, heightMap[x * resolutionFactor, y * resolutionFactor], y);
                uv[index] = new Vector2((float)x / (resolution - 1), (float)y / (resolution - 1));

                index++;
            }
        }

        int indicesSideLength = resolution - 1;
        int[] indices = new int[indicesSideLength * indicesSideLength * 6];
        int triangle = 0;

        for (int y = 0; y < indicesSideLength; y++)
        {
            for (int x = 0; x < indicesSideLength; x++)
            {
                int bottomLeft = y * resolution + x;
                int bottomRight = bottomLeft + 1;
                int topLeft = bottomLeft + resolution;
                int topRight = topLeft + 1;

                //Triangle 1
                indices[triangle++] = bottomLeft;   //triangle++ means we take triangle first, THEN increment it
                indices[triangle++] = topLeft;
                indices[triangle++] = bottomRight;

                //triangle 2
                indices[triangle++] = topLeft;
                indices[triangle++] = topRight;
                indices[triangle++] = bottomRight;
            }
        }

        mesh.vertices = vertices;
        mesh.SetIndices(indices, MeshTopology.Triangles, 0);
        mesh.SetUVs(0, uv);

        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
        mesh.UploadMeshData(false);
        meshFilter.mesh = mesh;
        gameObject.transform.position = new Vector3(-(textureResolution / 2), 0, -(textureResolution / 2));
    }

    private void CalculateTileGridPosition(Vector2Int tileGridSize)
    {
        meshCenter = textureResolution / 2;
        tileGridEndPosition.x = meshCenter + tileGridSize.x;
        tileGridEndPosition.y = meshCenter + tileGridSize.y;
    }
}
