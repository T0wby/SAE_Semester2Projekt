using UnityEngine;
using UnityEngine.EventSystems;

/* 
 * Only used in Old scripts and not in current version.
*/
public static class MousePosition
{
    public static Vector3? GetMousePosition()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue))
            {
                return raycastHit.point;
            }
            else
            {
                return null;
            }
        }
        else
        {
            return null;
        }
    }
}