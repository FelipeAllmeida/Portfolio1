using System.Collections.Generic;
using UnityEngine;

namespace Vox.Internal
{
    public class InputRayManager : MonoBehaviour
    {
        public float rayMaxDistance;
        public List<string> listIgnoredLayers = new List<string>();

        public RayInfo CreateRayInfo(Camera p_camera, Vector2 p_point)
        {
            RaycastHit __raycastHit = new RaycastHit();

            Ray __ray = p_camera.ScreenPointToRay(p_point);

            int __layerMask = Physics.DefaultRaycastLayers;

            if (listIgnoredLayers.Count > 0)
            {
                __layerMask = 1 << LayerMask.NameToLayer(listIgnoredLayers[0]);
                listIgnoredLayers.ForEach(x => __layerMask |= (1 << LayerMask.NameToLayer(x)));
                __layerMask = ~__layerMask;
            }
           
            if (Physics.Raycast(__ray, out __raycastHit, __layerMask))
            {
                Debug.DrawRay(p_camera.transform.position, __ray.direction * rayMaxDistance, Color.green);

                return new RayInfo
                {
                    WorldPoint = __raycastHit.point,
                    Hit = __raycastHit.collider.gameObject
                };
            }
            else
            {
                Debug.DrawRay(p_camera.transform.position, __ray.direction * rayMaxDistance, Color.red);
            }

            return null;
        }
    }
}
