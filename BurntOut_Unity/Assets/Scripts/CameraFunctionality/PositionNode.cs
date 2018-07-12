using UnityEngine;

namespace Narrative
{
    public class PositionNode : MonoBehaviour
    {
        public CameraLookHere CameraLook;
        public Transform CameraPosition;

        public void MoveTo()
        {
            var cameraLooks = FindObjectsOfType<CameraLookHere>();
            foreach (var cameraLook in cameraLooks)
                cameraLook.enabled = false;


            var positionNodes = FindObjectsOfType<PositionNode>();
            foreach (var positionNode in positionNodes)
                positionNode.gameObject.SetActive(false);

            PlayerMovement.Instance.MoveTo(CameraPosition.transform, CameraLook.transform);

            gameObject.SetActive(true);
        }

        public void FadeTo(ActorObject actor)
        {
            var cameraLooks = FindObjectsOfType<CameraLookHere>();
            foreach (var cameraLook in cameraLooks)
                cameraLook.enabled = false;

            PlayerMovement.Instance.FadeTo(actor.transform, 2, this);

            gameObject.SetActive(false);
        }
    }
}