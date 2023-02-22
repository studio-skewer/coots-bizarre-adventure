using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Coots.Code.Controllers
{
    public class MouseImageManager : MonoBehaviour
    {
        [SerializeField] RTSCameraController cam;

        [SerializeField] Texture2D idle;
        [SerializeField] Texture2D selecting;
        [SerializeField] Texture2D draggingPos;
        [SerializeField] Texture2D draggingRot;
        [SerializeField] Texture2D draggingZom;

        public CursorMode cursorMode = CursorMode.Auto;
        public Vector2 hotSpot = Vector2.zero;

        void Awake() => cam.mouseState.StateChanged += OnMouseStateChanged;

        void OnDestroy() => cam.mouseState.StateChanged -= OnMouseStateChanged;


        void OnMouseStateChanged(RTSCameraController.MouseState.State newState)
        {
            switch (newState)
            {
                case RTSCameraController.MouseState.State.Idle:
                    Cursor.SetCursor(idle, hotSpot, cursorMode);
                    break;
                case RTSCameraController.MouseState.State.Selecting:
                    Cursor.SetCursor(selecting, hotSpot, cursorMode);
                    break;
                case RTSCameraController.MouseState.State.DraggingPosition:
                    Cursor.SetCursor(draggingPos, hotSpot, cursorMode);
                    break;
                case RTSCameraController.MouseState.State.DraggingRotation:
                    Cursor.SetCursor(draggingRot, hotSpot, cursorMode);
                    break;
                case RTSCameraController.MouseState.State.DraggingZoom:
                    Cursor.SetCursor(draggingZom, hotSpot, cursorMode);
                    break;
                default:
                    break;
            }
        }
    }
}