using UnityEngine;

public class InputController : MonoBehaviour
{

    public CellType Mouse0 = CellType.Alive;
    public CellType Mouse1 = CellType.Dead;

    public CellType ShiftMouse0 = CellType.Alive;
    public CellType ShiftMouse1 = CellType.Dead;

    private Camera camera;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        camera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (Input.GetMouseButton(0))
            {
                PaintPixel(ShiftMouse0);
            }
            else if (Input.GetMouseButton(1))
            {
                PaintPixel(ShiftMouse1);
            }
        }
        else
        {
            if (Input.GetMouseButton(0))
            {
                PaintPixel(Mouse0);
            }
            else if (Input.GetMouseButton(1))
            {
                PaintPixel(Mouse1);
            }
        }
    }
    private void PaintPixel(CellType cellType)
    {
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 20f))
        {
            if (hit.collider.gameObject.TryGetComponent(out AutomataController automataController))
            {
                automataController.SetPixelToType(hit.point, cellType);
            }
        }
    }
}
