using UnityEngine;
public abstract class VectorField : MonoBehaviour
{
    public abstract Vector3 GetAcceleration(GameObject gameObject);
}
