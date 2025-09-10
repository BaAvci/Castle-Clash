using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

public class BlackholeVec : VectorField
{
    private Spiral spiral;
    protected override void Awake()
    {
        base.Awake();
        spiral = new Spiral();
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
            return spiral.VectorFieldAcceleration(velocity);
        }

        float speed = SpeedCalculator(relativeX, relativeZ, false);

        float x = (relativeZ * spiralStrenght - (relativeX * inwardPullStrenght)) / speed;
        float z = (-relativeX * spiralStrenght - (relativeZ * inwardPullStrenght)) / speed;
        Vector3 acceleration = new Vector3(x, 0, z) * Time.deltaTime;
        acceleration -= accelerationDrag * Time.deltaTime * velocity;
        return acceleration;
    }
    public override Vector3 VectorFieldAcceleration(Vector3 velocity)
    {
        return BlackHole(velocity);
    }
}
