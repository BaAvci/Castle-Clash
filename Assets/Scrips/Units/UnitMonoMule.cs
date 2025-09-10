using UnityEditor;
using UnityEngine;

public class UnitMonoMule : MonoBehaviour
{
    public static UnitMonoMule Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public static void DestroyGameObject(GameObject gameObject)
    {
        Destroy(gameObject);
    }
}
