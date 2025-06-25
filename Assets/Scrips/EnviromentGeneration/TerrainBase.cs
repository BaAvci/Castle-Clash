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
    protected int _resolution = 512;
    [SerializeField]
    protected int textureResolution = 512;

    [SerializeField]
    protected float _noiseScale = 0.3f;
    [SerializeField]
    protected int _noiseOctaves = 4;

    protected float noiseHeight;
    protected MeshFilter _meshFilter;
    protected MeshRenderer _meshRenderer;
    protected Vector2Int tileGridEndPosition;
    protected int meshCenter;

    protected float[,] heightMap;

    protected virtual void Awake()
    {
        _meshFilter = GetComponent<MeshFilter>();
        _meshRenderer = GetComponent<MeshRenderer>();
    }

    public void CreateEnviroment(Vector2Int tileGridSize, float noiseHeight, int randomSeed)
    {
        float minHeight, maxHeight;
        this.noiseHeight = noiseHeight;
        CalculateTileGridPosition(tileGridSize);
        GenerateHeightMap(out minHeight, out maxHeight, randomSeed);

        GenerateTexture(minHeight, maxHeight);

        GenerateMesh();
    }

    protected virtual void GenerateHeightMap(out float minHeight, out float maxHeight, int randomSeed)
    {
        maxHeight = float.MinValue;
        minHeight = float.MaxValue;

        System.Random rand = new(randomSeed);
        float offset = (float)rand.NextDouble();

        heightMap = new float[textureResolution, textureResolution];
        float noiseMultiplier = 1f / (_noiseScale * textureResolution);   //Maybe _textureResolution?

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
                for (int o = 1; o <= _noiseOctaves; o++)
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

        Vector3[] vertices = new Vector3[_resolution * _resolution];
        Vector2[] uv = new Vector2[_resolution * _resolution];
        int index = 0;
        int resolutionFactor = textureResolution / _resolution;

        for (int y = 0; y < _resolution; y++)
        {
            for (int x = 0; x < _resolution; x++)
            {
                vertices[index] = new Vector3(x, heightMap[x * resolutionFactor, y * resolutionFactor], y);
                uv[index] = new Vector2((float)x / (_resolution - 1), (float)y / (_resolution - 1));

                index++;
            }
        }

        int indicesSideLength = _resolution - 1;
        int[] indices = new int[indicesSideLength * indicesSideLength * 6];
        int triangle = 0;

        for (int y = 0; y < indicesSideLength; y++)
        {
            for (int x = 0; x < indicesSideLength; x++)
            {
                int bottomLeft = y * _resolution + x;
                int bottomRight = bottomLeft + 1;
                int topLeft = bottomLeft + _resolution;
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
        _meshFilter.mesh = mesh;
        gameObject.transform.position = new Vector3(-(textureResolution / 2), 0, -(textureResolution / 2));
    }

    private void CalculateTileGridPosition(Vector2Int tileGridSize)
    {
        meshCenter = textureResolution / 2;
        tileGridEndPosition.x = meshCenter + tileGridSize.x;
        tileGridEndPosition.y = meshCenter + tileGridSize.y;
    }
}
