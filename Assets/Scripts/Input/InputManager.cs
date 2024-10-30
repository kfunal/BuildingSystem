using System;
using BuildingSystem.Constants;
using UnityEngine;
using UnityEngine.InputSystem;

namespace BuildingSystem.Input
{
    public class InputManager : MonoBehaviour
    {
        public static InputManager Instance { get; private set; }

        public Action<Vector2> OnSpawnBuildingKeyPressed;
        public Action<Vector2> OnDestroyBuildingKeyPressed;
        public Action OnRotateBuildingKeyPressed;
        public Action OnResetCameraFOV;
        public Action OnResetCameraRotation;

        public Vector2 CameraMoveInput { get; private set; } = Vector2.zero;
        public Vector2 CameraZoom { get; private set; } = Vector2.zero;
        public bool RotateCamera { get; private set; } = false;

        private PlayerInput playerInput;
        private InputActionMap playerActionMap;

        private void Awake()
        {
            playerInput = GetComponent<PlayerInput>();
            playerActionMap = playerInput.actions.FindActionMap(InputConstants.PLAYER);
            CreateSingleton();
        }

        private void CreateSingleton()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void OnEnable() => EventSubscription(true);

        private void OnDisable() => EventSubscription(false);

        private void EventSubscription(bool subscribe)
        {
            AddOrRemoveForAllPhases(InputConstants.MOVE_CAMERA, subscribe, MoveCameraKeyPressed);
            AddOrRemoveForAllPhases(InputConstants.ZOOM_CAMERA, subscribe, ScrollDelta);
            AddOrRemoveForAllPhases(InputConstants.ROTATE_CAMERA, subscribe, OnRotateCamera);
            AddOrRemoveForStartPhase(InputConstants.MOUSE_LEFT_CLICK, subscribe, LeftMouseButtonClicked);
            AddOrRemoveForStartPhase(InputConstants.ROTATE_BUILDING, subscribe, RotateBuildingKeyPressed);
            AddOrRemoveForStartPhase(InputConstants.RESET_CAMERA_FOV, subscribe, ResetCameraFOV);
            AddOrRemoveForStartPhase(InputConstants.RESET_CAMERA_ROTATION, subscribe, ResetCameraRotation);
            AddOrRemoveForStartPhase(InputConstants.DESTROY_BUILDING, subscribe, DestroyBuilding);
        }

        public Vector2 MouseDelta() => Mouse.current.delta.ReadValue();
        public Vector2 MousePosition() => Mouse.current.position.ReadValue();

        private void ResetCameraFOV(InputAction.CallbackContext context) => OnResetCameraFOV?.Invoke();
        private void RotateBuildingKeyPressed(InputAction.CallbackContext context) => OnRotateBuildingKeyPressed?.Invoke();
        private void MoveCameraKeyPressed(InputAction.CallbackContext context) => CameraMoveInput = context.ReadValue<Vector2>();
        private void LeftMouseButtonClicked(InputAction.CallbackContext context) => OnSpawnBuildingKeyPressed?.Invoke(MousePosition());
        private void ScrollDelta(InputAction.CallbackContext context) => CameraZoom = context.ReadValue<Vector2>();
        private void OnRotateCamera(InputAction.CallbackContext context) => RotateCamera = context.started || context.performed;
        private void ResetCameraRotation(InputAction.CallbackContext context) => OnResetCameraRotation?.Invoke();
        private void DestroyBuilding(InputAction.CallbackContext context) => OnDestroyBuildingKeyPressed?.Invoke(MousePosition());

        private void AddOrRemoveForAllPhases(string actionName, bool add, Action<InputAction.CallbackContext> action)
        {
            InputAction inputAction = playerActionMap.FindAction(actionName);

            if (add)
            {
                inputAction.started += action;
                inputAction.performed += action;
                inputAction.canceled += action;
            }
            else
            {
                inputAction.started -= action;
                inputAction.performed -= action;
                inputAction.canceled -= action;
            }
        }

        private void AddOrRemoveForStartPhase(string actionName, bool add, Action<InputAction.CallbackContext> action)
        {
            InputAction inputAction = playerActionMap.FindAction(actionName);

            if (add)
                inputAction.started += action;
            else
                inputAction.started -= action;

        }
    }
}