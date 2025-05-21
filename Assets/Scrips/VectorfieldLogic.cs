using UnityEditor;
using UnityEngine;

public static class VectorfieldLogic
{
    // Creates a vectorfield that starts at point A and moves in a line
    public static void StraightVectorFieldSelf(this Transform transform, Vector3 start, Vector3 length)
    {
        // F(x) = 0
        // F(y) = -9.81
        if (start.y - transform.position.y <= length.y)
        {
            transform.position += new Vector3(0, -9.81f, 0) * Time.deltaTime;
        }
        else
        {
            transform.position = start;
        }
    }

    public static Vector3 StraightVectorFieldOther(this Transform vectorfieldPosition, Transform foreignTransform, float vectorfieldHeight, float updraft)
    {
        if (vectorfieldPosition.position.y + vectorfieldHeight >= foreignTransform.position.y)
        {
            return new Vector3(0, updraft, 0) * Time.deltaTime;
        }
        return Vector3.zero;
    }

    // Creates a vectorfield that circles around a given point.
    // No forces outwards or inwards applied
    public static Vector3 SpiralVectorField(this Transform transform, Transform foreignTransform, Vector3 strenght, Vector3 velocity, float accelerationDrag = 50)
    {
        // F(x,y) = <y,-x> The basic function results in speed increase the further away it is
        // F(x,y) = <y * n , -x * n> results in a stronger start speed
        // F(x,y) = <y / x^2+y^2, -x / x^2+y^2> results in speed drop off the further away it is
        // F(x,y) = <y / sqrt(x^2+y^2), -x / sqrt(x^2+y^2)> results in a constant speed

        var relativX = foreignTransform.position.x - transform.position.x;
        var relativZ = foreignTransform.position.z - transform.position.z;

        float speed = SpeedCalculator(relativX, relativZ);
        float x = relativZ * strenght.z / speed;
        float z = -relativX * strenght.x / speed;

        Vector3 acceleration = new Vector3(x, 0, z) * Time.deltaTime;
        acceleration -= velocity * accelerationDrag * Time.deltaTime;
        return acceleration;
    }

    // Creates a vectorfield that moves everything away from a given point
    public static Vector3 RepulsionVectorField(this Transform transform, Transform foreignTransform, Vector3 strenght, Vector3 velocity, float accelerationDrag = 50)
    {
        // F(x,y) = <x , y> The basic function results in speed increase the further away it is
        // F(x,y) = <x * n , y * n> results in a stronger start speed
        // F(x,y) = <x / (x^2 + y^2), y / (x^2 + y^2)> results in speed drop off the further away it is
        // F(x,y) = <x / sqrt(x^2+y^2), y / sqrt(x^2+y^2)> results in a constant speed
        var relativX = foreignTransform.position.x - transform.position.x;
        var relativZ = foreignTransform.position.z - transform.position.z;

        float speed = SpeedCalculator(relativX, relativZ);
        float x = relativX * strenght.x / speed;
        float z = relativZ * strenght.z / speed;

        Vector3 acceleration = new Vector3(x, 0, z) * Time.deltaTime;
        acceleration -= velocity * accelerationDrag * Time.deltaTime;
        return acceleration;
    }

    // Creates a vectorfield that moves everything towards the given point
    public static Vector3 AttractingVectorField(this Transform transform, Transform foreignTransform, Vector3 strenght, Vector3 velocity, float accelerationDrag = 50)
    {
        // F(x,y) = <-x , -y> The basic function results in speed increase the further away it is
        // F(x,y) = <-x * n , -y * n> results in a stronger start speed
        // F(x,y) = <-x / (x^2 + y^2), -y / (x^2 + y^2)> results in speed drop off the further away it is
        // F(x,y) = <-x / sqrt(x^2+y^2), -y / sqrt(x^2+y^2)> results in a constant speed
        var relativX = foreignTransform.position.x - transform.position.x;
        var relativZ = foreignTransform.position.z - transform.position.z;

        if (Mathf.Abs(relativX) <= 0.1f && Mathf.Abs(relativZ) <= 0.1f)
        {
            return Vector3.zero;
        }

        float speed = SpeedCalculator(relativX, relativZ, false);
        float x = -relativX * strenght.x / speed;
        float z = -relativZ * strenght.z / speed;

        Vector3 acceleration = new Vector3(x, 0, z) * Time.deltaTime;
        acceleration -= velocity * accelerationDrag * Time.deltaTime;
        return acceleration;
    }
    public static Vector3 BlackHole(this Transform transform, Transform foreignTransform, Vector3 velocity, float accelerationDrag = 50, float inwardPullStrenght = 0.1f, float spiralStrenght = 1)
    {
        // Fa(x,y) = <-x , -y> inward pull        
        // Fb(x,y) = <+y , -x> vortex
        // F = Fa + Fb
        // F = < -x +y , -y -x> OD < y -x , -x -y> results in the same vectorfield
        float noForceRadius = 0.5f;

        var relativX = foreignTransform.position.x - transform.position.x;
        var relativZ = foreignTransform.position.z - transform.position.z;

        if (relativX * relativX + relativZ * relativZ < noForceRadius * noForceRadius)
        {
            return transform.SpiralVectorField(foreignTransform, new Vector3(50, 0, 50), velocity, accelerationDrag);
        }

        float speed = SpeedCalculator(relativX, relativZ, false);

        float x = (relativZ * spiralStrenght - (relativX * inwardPullStrenght)) / speed;
        float z = (-relativX * spiralStrenght - (relativZ * inwardPullStrenght)) / speed;
        Vector3 acceleration = new Vector3(x, 0, z) * Time.deltaTime;
        acceleration -= velocity * accelerationDrag * Time.deltaTime;
        return acceleration;
    }
    private static float SpeedCalculator(float x, float y, bool sqrt = false)
    {
        float speed = x * x + y * y;
        if (sqrt)
        {
            speed = Mathf.Sqrt(speed);
        }
        return speed;
    }
}

