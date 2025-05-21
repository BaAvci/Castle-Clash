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
    [SerializeField] private Vector3 vectorFieldSize = new(5, 0, 5);
    [SerializeField] private Vector3 vectorFieldStrength = new(5, 0, 3);
    [SerializeField] private Transform foreignObject;
    [SerializeField] private VectorFieldTyp vectorFieldTyp;
    [SerializeField] private float accelerationDrag = 50;

    [Header("StraightField")]
    [SerializeField] private float updraft = 5;
    [SerializeField] private float vectorFieldHeight = 10;

    [Header("Blackhole")]
    [SerializeField] private float inwardPullStr;
    [SerializeField] private float spiralStr;

    private Vector3 vectorFieldBasePositiveCorner;
    private Vector3 vectorFieldBaseNegativeCorner;
    [Header("ForeignObject")]
    private Vector3 acceleration;
    private Vector3 velocity;
    private bool reachedMiddle = false;

    private void Start()
    {
        vectorFieldBasePositiveCorner = transform.position + vectorFieldSize;
        vectorFieldBaseNegativeCorner = transform.position - vectorFieldSize;

    }
    // Update is called once per frame
    void Update()
    {
        var xCheck = foreignObject.position.x <= vectorFieldBasePositiveCorner.x && foreignObject.position.x >= vectorFieldBaseNegativeCorner.x;
        var yCheck = foreignObject.position.y <= vectorFieldHeight;
        var zCheck = foreignObject.position.z <= vectorFieldBasePositiveCorner.z && foreignObject.position.z >= vectorFieldBaseNegativeCorner.z;
        if (xCheck && yCheck && zCheck)
        {
            switch (vectorFieldTyp)
            {
                case VectorFieldTyp.Linear:
                    acceleration = transform.StraightVectorFieldOther(foreignObject, vectorFieldHeight, updraft);
                    break;
                case VectorFieldTyp.Circular:
                    acceleration = transform.SpiralVectorField(foreignObject, vectorFieldStrength, velocity, accelerationDrag);
                    break;
                case VectorFieldTyp.Attraction:
                    acceleration = transform.AttractingVectorField(foreignObject, vectorFieldStrength);
                    break;
                case VectorFieldTyp.Repulsion:
                    acceleration = transform.RepulsionVectorField(foreignObject, vectorFieldStrength);
                    break;
                case VectorFieldTyp.Blackhole:
                    acceleration = transform.BlackHole(foreignObject, ref reachedMiddle, velocity, accelerationDrag, inwardPullStr, spiralStr);
                    break;
            }
            acceleration -= velocity * 50 * Time.deltaTime;
            velocity += acceleration * Time.deltaTime;
            foreignObject.position += velocity * Time.deltaTime;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawCube(transform.position, vectorFieldSize * 2);
    }
}
