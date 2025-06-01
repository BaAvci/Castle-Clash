using UnityEngine;
using UnityEngine.VFX;

public class AntiGravity : VectorField
{

    protected override void Start()
    {
        base.Start();
        diameter = (vectorFieldSize.x + vectorFieldSize.z) / 2;
        vfxAsset.SetFloat("Radius", diameter);
        vfxAsset.SetFloat("MaxHeight", diameter);
        vfxAsset.enabled = true;
    }

    private Vector3 StraightVectorFieldOther(Vector3 velocity)
    {
        if (transform.position.y + vectorfieldHeight >= foreignObject.position.y)
        {
            Vector3 acceleration = new Vector3(0, upDraft, 0) * Time.deltaTime;
            acceleration -= velocity * Time.deltaTime;
            return acceleration;
        }
        return Vector3.zero;
    }

    public override Vector3 VectorFieldAcceleration(Vector3 velocity)
    {
        return StraightVectorFieldOther(velocity);
    }
}
