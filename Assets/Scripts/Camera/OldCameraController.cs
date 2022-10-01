using UnityEditor;
using UnityEngine;

public class OldCameraController : MonoBehaviour
{
    [Header("Cameraoptions")] [SerializeField]
    private bool allowMovement = true;

    [SerializeField] private float panSpeed = 30f;
    [SerializeField] private float panBorderThickness = 15f;
    [SerializeField] private float scrollSpeed = 5f;
    [SerializeField] private float minY = 20f;
    [SerializeField] private float maxY = 120f;


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            allowMovement = !allowMovement;
        }

        if (!allowMovement)
        {
            return;
        }

        if (Input.GetKey("w") || Input.mousePosition.y >= Screen.height - panBorderThickness)
        {
            transform.Translate(Vector3.forward * (panSpeed * Time.deltaTime), Space.World);
        }

        if (Input.GetKey("s") || Input.mousePosition.y <= panBorderThickness)
        {
            transform.Translate(Vector3.back * (panSpeed * Time.deltaTime), Space.World);
        }

        if (Input.GetKey("a") || Input.mousePosition.x <= panBorderThickness)
        {
            transform.Translate(Vector3.left * (panSpeed * Time.deltaTime), Space.World);
        }

        if (Input.GetKey("d") || Input.mousePosition.x >= Screen.width - panBorderThickness)
        {
            transform.Translate(Vector3.right * (panSpeed * Time.deltaTime), Space.World);
        }

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        Vector3 pos = transform.position;
        pos.y -= scroll * 500 * scrollSpeed * Time.deltaTime;
        pos.y = Mathf.Clamp(pos.y, minY, maxY);
        transform.position = pos;
    }
}