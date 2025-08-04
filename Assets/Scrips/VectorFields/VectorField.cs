using UnityEngine;
using UnityEngine.VFX;
public abstract class VectorField : MonoBehaviour
{
    [Header("General")]
    [SerializeField] protected Vector3 vectorFieldSize = new(5, 0, 5);
    [SerializeField] protected Vector3 vectorFieldStrength = new(5, 0, 3);
    [SerializeField] protected float vectorfieldHeight = 10;
    [SerializeField] protected Transform foreignObject;
    [SerializeField] protected float accelerationDrag = 50;

    [Header("StraightField")]
    [SerializeField] protected float upDraft = 5;
    [SerializeField] protected float vectorFieldHeight = 10;

    [Header("Blackhole")]
    [SerializeField] protected float inwardPullStrenght;
    [SerializeField] protected float spiralStrenght;

    protected Vector3 vectorFieldBasePositiveCorner;
    protected Vector3 vectorFieldBaseNegativeCorner;
    [Header("ForeignObject")]
    protected Vector3 acceleration;
    protected Vector3 velocity;

    //The attacked VFX to display the vectorfield
    protected VisualEffect vfxAsset;
    protected float diameter;

    [SerializeField] private bool drawGizmo;

    protected virtual void Awake()
    {
        vfxAsset = transform.GetComponentInChildren<VisualEffect>();

        vectorFieldBasePositiveCorner = transform.position + vectorFieldSize;
        vectorFieldBaseNegativeCorner = transform.position - vectorFieldSize;
    }

    protected virtual void Update()
    {
        var xCheck = foreignObject.position.x <= vectorFieldBasePositiveCorner.x && foreignObject.position.x >= vectorFieldBaseNegativeCorner.x;
        var yCheck = foreignObject.position.y <= vectorfieldHeight;
        var zCheck = foreignObject.position.z <= vectorFieldBasePositiveCorner.z && foreignObject.position.z >= vectorFieldBaseNegativeCorner.z;
        if (xCheck && zCheck && yCheck)
        {
            acceleration = VectorFieldAcceleration(velocity);
            acceleration -= velocity * Time.deltaTime;
            velocity += acceleration * Time.deltaTime;
            foreignObject.position += velocity * Time.deltaTime;
        }
    }

    public abstract Vector3 VectorFieldAcceleration(Vector3 velocity);
    protected float SpeedCalculator(float x, float y, bool sqrt = false)
    {
        float speed = x * x + y * y;
        if (sqrt)
        {
            speed = Mathf.Sqrt(speed);
        }
        return speed;
    }

    private void OnDrawGizmos()
    {
        if (drawGizmo)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawCube(transform.position, vectorFieldSize * 2);
        }
    }
}
