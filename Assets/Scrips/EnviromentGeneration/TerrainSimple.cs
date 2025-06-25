using UnityEngine;

public class TerrainSimple : TerrainBase
{
    [SerializeField]
    private TerrainLayer _bottomLayer;
    [SerializeField]
    private TerrainLayer _topLayer;

    protected override void GenerateTexture(float minHeight, float maxHeight)
    {
        Texture2D texture = new Texture2D(textureResolution, textureResolution);
        texture.filterMode = FilterMode.Bilinear;
        Color32[] colors = new Color32[textureResolution * textureResolution];

        for (int y = 0; y < textureResolution; y++)
        {
            for (int x = 0; x < textureResolution; x++)
            {
                float height = heightMap[x, y];

                float normalizedHeight = height.Map(minHeight, maxHeight, 0f, 1f);
                normalizedHeight = Mathf.Clamp01(normalizedHeight); //clamp 0-1

                Color32 color = new Color(normalizedHeight, 0, 0);

                colors[y * textureResolution + x] = color;
            }
        }

        texture.SetPixels32(colors);
        texture.Apply();

        _meshRenderer.sharedMaterial.SetTexture("_HeightMap", texture);
        _meshRenderer.sharedMaterial.SetFloat("_Threshold", _bottomLayer.HeighThreshold);
    }

    private void OnValidate()
    {
        _meshRenderer.sharedMaterial.SetFloat("_Threshold", _bottomLayer.HeighThreshold);
    }
}
