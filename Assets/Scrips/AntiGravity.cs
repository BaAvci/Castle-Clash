using UnityEngine;
using UnityEngine.VFX;

public class AntiGravity : MonoBehaviour
{
    [Header("General Values")] //Should be moved to an abstract class
    [SerializeField] private Vector3 vectorFieldSize = new(5, 0, 5);
    [SerializeField] private float accelerationDrag = 1;
    [SerializeField] private float vectorfieldHeight = 10;
    [SerializeField] private float upDraft = 5;

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
        diameter = (vectorFieldSize.x + vectorFieldSize.z) / 2;
        vfxAsset = transform.GetComponentInChildren<VisualEffect>();
        vfxAsset.SetFloat("Radius", diameter);
        vfxAsset.SetFloat("MaxHeight", diameter);
        vfxAsset.enabled = true;
        //vfxAsset.GetExposedProperties(VFXExposedProperty)
        vectorFieldBasePositiveCorner = transform.position + vectorFieldSize;
        vectorFieldBaseNegativeCorner = transform.position - vectorFieldSize;
        //vfxAsset.GetExposedProperties();
    }

    private void Update()
    {
        var xCheck = foreignObject.position.x <= vectorFieldBasePositiveCorner.x && foreignObject.position.x >= vectorFieldBaseNegativeCorner.x;
        var yCheck = foreignObject.position.y <= vectorfieldHeight;
        var zCheck = foreignObject.position.z <= vectorFieldBasePositiveCorner.z && foreignObject.position.z >= vectorFieldBaseNegativeCorner.z;
        if (xCheck && zCheck && yCheck)
        {
            acceleration = StraightVectorFieldOther(velocity);
            acceleration -= velocity * Time.deltaTime;
            velocity += acceleration * Time.deltaTime;
            foreignObject.position += velocity * Time.deltaTime;
        }
    }
    public Vector3 StraightVectorFieldOther(Vector3 velocity)
    {
        if (transform.position.y + vectorfieldHeight >= foreignObject.position.y)
        {
            Vector3 acceleration = new Vector3(0, upDraft, 0) * Time.deltaTime;
            acceleration -= velocity * Time.deltaTime;
            return acceleration;
        }
        return Vector3.zero;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawCube(transform.position, vectorFieldSize * 2);
    }
}
