using UnityEngine;

public class Attraction : VectorField
{
    private Vector3 AttractingVectorField( Vector3 velocity)
    {
        // F(x,y) = <-x , -y> The basic function results in speed increase the further away it is
        // F(x,y) = <-x * n , -y * n> results in a stronger start speed
        // F(x,y) = <-x / (x^2 + y^2), -y / (x^2 + y^2)> results in speed drop off the further away it is
        // F(x,y) = <-x / sqrt(x^2+y^2), -y / sqrt(x^2+y^2)> results in a constant speed
        var relativX = foreignObject.position.x - transform.position.x;
        var relativZ = foreignObject.position.z - transform.position.z;

        if (Mathf.Abs(relativX) <= 0.1f && Mathf.Abs(relativZ) <= 0.1f)
        {
            return Vector3.zero;
        }

        float speed = SpeedCalculator(relativX, relativZ, false);
        float x = -relativX * vectorFieldStrength.x / speed;
        float z = -relativZ * vectorFieldStrength.z / speed;

        Vector3 acceleration = new Vector3(x, 0, z) * Time.deltaTime;
        acceleration -= velocity * accelerationDrag * Time.deltaTime;
        return acceleration;
    }

    public override Vector3 VectorFieldAcceleration(Vector3 velocity)
    {
        return AttractingVectorField(velocity);
    }
}
