using UnityEngine;

public class MatChanger : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private GameObject gameObject;
    private Material mat;
    void Start()
    {
        mat = gameObject.GetComponent<MeshRenderer>().material;
        mat.color = Color.cyan;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
