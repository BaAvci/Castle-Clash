using UnityEngine;

public enum VectorFieldTyp
{
    Linear,
    Circular,
    Attraction,
    Repulsion,
    Blackhole,
}

public class VectorfieldCreator : MonoBehaviour
{
    [Header("General")]
    [SerializeField] private Vector3 vectorfieldSize = new(5, 0, 5);
    [SerializeField] private Vector3 vectorfieldStrength = new(5, 0, 3);
    [SerializeField] private Transform foreigneObject;
    [SerializeField] private VectorFieldTyp vectorFieldTyp;

    [Header("StraightField")]
    [SerializeField] private float updraft = 5;
    [SerializeField] private float vectorfieldHeight = 10;

    [Header("Blackhole")]
    [SerializeField] private float inwardPullStr;
    [SerializeField] private float spiralStr;

    private Vector3 vectorfieldBasePositivCorner;
    private Vector3 vectorfieldBaseNegativCorner;
    private void Start()
    {
        vectorfieldBasePositivCorner = transform.position + vectorfieldSize;
        vectorfieldBaseNegativCorner = transform.position - vectorfieldSize;

    }
    // Update is called once per frame
    void Update()
    {
        var xCheck = foreigneObject.position.x <= vectorfieldBasePositivCorner.x && foreigneObject.position.x >= vectorfieldBaseNegativCorner.x;
        var yCheck = foreigneObject.position.y <= vectorfieldHeight;
        var zCheck = foreigneObject.position.z <= vectorfieldBasePositivCorner.z && foreigneObject.position.z >= vectorfieldBaseNegativCorner.z;
        if (xCheck && yCheck && zCheck)
        {
            switch (vectorFieldTyp)
            {
                case VectorFieldTyp.Linear:
                    transform.StraightVectorFieldOther(foreigneObject, vectorfieldHeight, updraft);
                    break;
                case VectorFieldTyp.Circular:
                    transform.SpiralVectorField(foreigneObject, vectorfieldStrength);
                    break;
                case VectorFieldTyp.Attraction:
                    transform.AttractingVectorField(foreigneObject, vectorfieldStrength);
                    break;
                case VectorFieldTyp.Repulsion:
                    transform.RepulsionVectorField(foreigneObject, vectorfieldStrength);
                    break;
                case VectorFieldTyp.Blackhole:
                    transform.BlackHole(foreigneObject, inwardPullStr, spiralStr);
                    break;
            }
        }

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawCube(transform.position, vectorfieldSize * 2);
    }
}
