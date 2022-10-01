using UnityEngine;
using UnityEngine.EventSystems;

/* TODO: 
 * Use same methode from PatrolWait script to generate random pos in radius from raycastHit.point
 * Check if random pos is still on Navmesh.
 * If not search for the nearest point on NavMesh
 * Maybe check distance between both to prevent random mouse clicks to move swarm
*/
public static class MousePosition
{
    public static Vector3? GetNavMeshPosition()
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