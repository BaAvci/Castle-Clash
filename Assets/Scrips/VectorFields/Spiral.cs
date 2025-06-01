using UnityEngine;
using UnityEngine.VFX;

public class Spiral : VectorField
{
    private Vector3 SpiralVectorField( Vector3 velocity)
    {
        // F(x,y) = <y,-x> The basic function results in speed increase the further away it is
        // F(x,y) = <y * n , -x * n> results in a stronger start speed
        // F(x,y) = <y / x^2+y^2, -x / x^2+y^2> results in speed drop off the further away it is
        // F(x,y) = <y / sqrt(x^2+y^2), -x / sqrt(x^2+y^2)> results in a constant speed

        var relativX = foreignObject.position.x - transform.position.x;
        var relativZ = foreignObject.position.z - transform.position.z;

        float speed = SpeedCalculator(relativX, relativZ);
        float x = relativZ * vectorFieldStrength.z / speed;
        float z = -relativX * vectorFieldStrength.x / speed;

        Vector3 acceleration = new Vector3(x, 0, z) * Time.deltaTime;
        acceleration -= velocity * accelerationDrag * Time.deltaTime;
        return acceleration;
    }
    public override Vector3 VectorFieldAcceleration(Vector3 velocity)
    {
        return SpiralVectorField(velocity);
    }
}
