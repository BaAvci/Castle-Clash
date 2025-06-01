using UnityEngine;
using UnityEngine.VFX;

public class Blackhole : MonoBehaviour
{
    [Header("General Values")] //Should be moved to an abstract class
    [SerializeField] private Vector3 vectorFieldSize = new(5, 0, 5);
    [SerializeField] private float accelerationDrag = 50;
    [SerializeField] private float inwardPullStrenght = 0.1f;
    [SerializeField] private float spiralStrenght = 1;

    [Header("ForeignObject")]
    [SerializeField] private Transform foreignObject; // Temp until i figure out how to affect units in vectorfield
    private Vector3 acceleration;
    private Vector3 velocity;

    // Should be moved to the abstact class
    // Values to check if a unit is in the vectorfield space
    private Vector3 vectorFieldBasePositiveCorner;
    private Vector3 vectorFieldBaseNegativeCorner;

    //The attacked VFX to display the vectorfield
    private VisualEffect vfxAsset;
    private float diameter;

    private void Start()
    {
        diameter = vectorFieldSize.x + vectorFieldSize.z;
        vfxAsset = transform.GetComponentInChildren<VisualEffect>();
        vfxAsset.SetFloat("blackHoleSize", diameter);
        vfxAsset.enabled = true;
        //vfxAsset.GetExposedProperties(VFXExposedProperty)
        vectorFieldBasePositiveCorner = transform.position + vectorFieldSize;
        vectorFieldBaseNegativeCorner = transform.position - vectorFieldSize;
        //vfxAsset.GetExposedProperties();
    }

    private void Update()
    {
        var xCheck = foreignObject.position.x <= vectorFieldBasePositiveCorner.x && foreignObject.position.x >= vectorFieldBaseNegativeCorner.x;
        var zCheck = foreignObject.position.z <= vectorFieldBasePositiveCorner.z && foreignObject.position.z >= vectorFieldBaseNegativeCorner.z;
        if (xCheck && zCheck)
        {
            acceleration = BlackHole(velocity);
            acceleration -= velocity * 50 * Time.deltaTime;
            velocity += acceleration * Time.deltaTime;
            foreignObject.position += velocity * Time.deltaTime;
        }
    }

    private Vector3 BlackHole(Vector3 velocity)
    {
        // Fa(x,y) = <-x , -y> inward pull        
        // Fb(x,y) = <+y , -x> vortex
        // F = Fa + Fb
        // F = < -x +y , -y -x> OD < y -x , -x -y> results in the same vectorfield
        float noForceRadius = 0.5f;

        var relativeX = foreignObject.position.x - transform.position.x;
        var relativeZ = foreignObject.position.z - transform.position.z;

        if (relativeX * relativeX + relativeZ * relativeZ < noForceRadius * noForceRadius)
        {
            Debug.Log("Error");
            return transform.SpiralVectorField(foreignObject, new Vector3(50, 0, 50), velocity, accelerationDrag);
        }

        float speed = SpeedCalculator(relativeX, relativeZ, false);

        float x = (relativeZ * spiralStrenght - (relativeX * inwardPullStrenght)) / speed;
        float z = (-relativeX * spiralStrenght - (relativeZ * inwardPullStrenght)) / speed;
        Vector3 acceleration = new Vector3(x, 0, z) * Time.deltaTime;
        acceleration -= velocity * accelerationDrag * Time.deltaTime;
        return acceleration;
    }

    private float SpeedCalculator(float x, float y, bool sqrt = false)
    {
        float speed = x * x + y * y;
        if (sqrt)
        {
            speed = Mathf.Sqrt(speed);
        }
        return speed;
    }
    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawCube(transform.position, vectorFieldSize * 2);
    //}
}
