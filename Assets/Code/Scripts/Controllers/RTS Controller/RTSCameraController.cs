using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Make sure the camera rig is -45 degrees
//and the camera itself is +45 degrees
//Also, the camera itself should have the same y and z value,
//but with y as positive and z as negative
namespace Coots.Code.Controllers
{
    public class RTSCameraController : MonoBehaviour
    {
        public static RTSCameraController instance;
        private Transform followTransform;
        [Tooltip("The camera itself's transform, not the rig.")]
        public Transform cameraTransform;
        public Camera cameraComponent;

        public float normalSpeed = 0.5f;
        public float fastSpeed = 3f;
        public float movementSpeed { get; private set; }
        public float movementTime = 5f;
        public float rotationAmount = 1f;
        [Tooltip("Negative y, positive x, same magnitude for each.")]
        public Vector3 zoomAmount;

        public float maxZoom = 200f;
        public float minZoom = 40f;

        public float maxX = 50f;
        public float minX = -10f;
        public float maxZ = 50f;
        public float minZ = -10f;

        private Vector3 newPosition;
        private Quaternion newRotation;
        private Vector3 newZoom;

        private Vector3 dragStartPosition;
        private Vector3 dragCurrentPosition;
        private Vector3 rotateStartPosition;
        private Vector3 rotateCurrentPosition;

        public MouseState mouseState;

        private void Start()
        {
            instance = this;
            newPosition = transform.position;
            newRotation = transform.rotation;
            newZoom = cameraTransform.localPosition;
            mouseState.StateChanged += OnMouseStateChanged;
        }

        private void OnDestroy()
        {
            mouseState.StateChanged -= OnMouseStateChanged;
        }

        private void Update()
        {
            if (followTransform != null)
            {
                newPosition = followTransform.position;
            }

            HandleMouseInput();
            HandleMovementInput();

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                followTransform = null;
            }
        }

        public void SetFollowTransform(Transform tr)
        {
            followTransform = tr;
        }

        public void MoveToTransform(Transform tr)
        {
            if (tr == null)
            {
                return;
            }
            newPosition = tr.position;
        }

        private void LateUpdate()
        {
            UpdatePositionsAndRotations();
        }

        void HandleMouseInput()
        {

#if !UNITY_WEBGL
            if (Input.mouseScrollDelta.y != 0)
            {
                newZoom += Input.mouseScrollDelta.y * zoomAmount * 5f;
            }
#endif

            mouseState.UpdateState(Input.GetMouseButtonDown(0), Input.GetMouseButton(1));

            switch (mouseState.CurState)
            {
                case MouseState.State.DraggingPosition:

                    Plane plane = new Plane(Vector3.up, Vector3.zero);

                    Ray ray = cameraComponent.ScreenPointToRay(Input.mousePosition);

                    if (plane.Raycast(ray, out float entry))
                    {
                        dragCurrentPosition = ray.GetPoint(entry);

                        newPosition = transform.position + dragStartPosition - dragCurrentPosition;
                    }

                    break;
                case MouseState.State.DraggingRotation:

                    dragCurrentPosition = Input.mousePosition;

                    Vector3 difference = dragStartPosition - dragCurrentPosition;

                    dragStartPosition = dragCurrentPosition;

                    newRotation *= Quaternion.Euler(Vector3.up * (-difference.x / 5f));

                    break;
                case MouseState.State.DraggingZoom:

                    dragCurrentPosition = Input.mousePosition;

                    Vector3 diff = dragStartPosition - dragCurrentPosition;

                    dragStartPosition = dragCurrentPosition;

                    newZoom += -diff.y * zoomAmount;

                    break;
                default:
                    break;
            }

#if !UNITY_WEBGL
            //middle click to drag rotation
            if (Input.GetMouseButtonDown(2))
            {
                rotateStartPosition = Input.mousePosition;
            }
            if (Input.GetMouseButton(2))
            {
                rotateCurrentPosition = Input.mousePosition;

                Vector3 difference = rotateStartPosition - rotateCurrentPosition;

                rotateStartPosition = rotateCurrentPosition;

                newRotation *= Quaternion.Euler(Vector3.up * (-difference.x / 5f));
            }
        }
#endif

        void OnMouseStateChanged(MouseState.State newState)
        {
            //start drag
            switch (newState)
            {
                case MouseState.State.DraggingPosition:
                    Plane plane = new Plane(Vector3.up, Vector3.zero);

                    Ray ray = cameraComponent.ScreenPointToRay(Input.mousePosition);

                    if (plane.Raycast(ray, out float entry))
                    {
                        dragStartPosition = ray.GetPoint(entry);
                    }
                    break;
                case MouseState.State.DraggingRotation:
                    dragStartPosition = Input.mousePosition;
                    break;
                case MouseState.State.DraggingZoom:
                    dragStartPosition = Input.mousePosition;
                    break;
                default:
                    break;
            }
        }

        void HandleMovementInput()
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                movementSpeed = fastSpeed;
            }
            else
            {
                movementSpeed = normalSpeed;
            }


            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            {
                newPosition += (transform.forward * movementSpeed);
            }
            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            {
                newPosition += (transform.forward * -movementSpeed);
            }
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                newPosition += (transform.right * movementSpeed);
            }
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                newPosition += (transform.right * -movementSpeed);
            }

            if (Input.GetKey(KeyCode.Q))
            {
                newRotation *= Quaternion.Euler(Vector3.up * rotationAmount);
            }
            if (Input.GetKey(KeyCode.E))
            {
                newRotation *= Quaternion.Euler(Vector3.up * -rotationAmount);
            }

            if (Input.GetKey(KeyCode.R))
            {
                newZoom += zoomAmount;
            }
            if (Input.GetKey(KeyCode.F))
            {
                newZoom -= zoomAmount;
            }


        }

        public struct MouseState
        {
            public enum State
            {
                Idle,
                Selecting,
                DraggingPosition,
                DraggingRotation,
                DraggingZoom
            }

            public event Action<State> StateChanged;

            public State CurState { get; private set; }

            public void UpdateState(bool leftClickDown, bool rightClickHeld)
            {
                // I am so sorry, but this is game jam code so whatever works
                State prevState = CurState;
                //left click and right click on same frame
                if (leftClickDown && rightClickHeld)
                {
                    if (CurState == State.DraggingRotation || CurState == State.DraggingZoom)
                    {
                        CurState = State.DraggingZoom;
                    }
                    else
                    {
                        CurState = State.DraggingRotation;
                    }
                }
                else if (leftClickDown)
                {
                    //only left click is selected
                    CurState = State.Selecting;
                }
                else if (rightClickHeld)
                {
                    //left click not selected
                    if (CurState == State.Idle || CurState == State.Selecting)
                    {
                        CurState = State.DraggingPosition;
                    }
                        // if already dragging, keep current state
                }
                else
                {
                    CurState = State.Idle;
                }
                if (prevState != CurState) StateChanged?.Invoke(this.CurState);
            }

        }

        void UpdatePositionsAndRotations()
        {
            newZoom.y = Mathf.Clamp(newZoom.y, minZoom, maxZoom);
            newZoom.z = Mathf.Clamp(newZoom.z, -maxZoom, -minZoom);

            newPosition.x = Mathf.Clamp(newPosition.x, minX, maxX);
            newPosition.z = Mathf.Clamp(newPosition.z, minZ, maxZ);


            transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * movementTime);
            transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, Time.deltaTime * movementTime);
            cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, newZoom, Time.deltaTime * movementTime);
        }
    }
}