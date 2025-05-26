using UnityEngine;

public class Blackhole : MonoBehaviour
{

    public Vector3 BlackHole( Transform foreignTransform, Vector3 velocity, float accelerationDrag = 50, float inwardPullStrenght = 0.1f, float spiralStrenght = 1)
    {
        // Fa(x,y) = <-x , -y> inward pull        
        // Fb(x,y) = <+y , -x> vortex
        // F = Fa + Fb
        // F = < -x +y , -y -x> OD < y -x , -x -y> results in the same vectorfield
        float noForceRadius = 0.5f;

        var relativeX = foreignTransform.position.x - transform.position.x;
        var relativeZ = foreignTransform.position.z - transform.position.z;

        if (relativeX * relativeX + relativeZ * relativeZ < noForceRadius * noForceRadius)
        {
            return transform.SpiralVectorField(foreignTransform, new Vector3(50, 0, 50), velocity, accelerationDrag);
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
}
