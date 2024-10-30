using BuildingSystem.Input;
using UnityEngine;

namespace BuildingSystem.CameraManager
{
    public class CameraManager : MonoBehaviour
    {
        [Header("Parameters")]
        [SerializeField] private float moveSpeed;
        [SerializeField] private float zoomSpeed;
        [SerializeField] private float zoomSmoothness;
        [SerializeField] private float rotationSpeed;

        [SerializeField] private float minFOV = 30;
        [SerializeField] private float maxFOV = 100;

        [Header("References")]
        [SerializeField] private Camera cam;

        private float currentFOV;
        private float defaultFov;
        private Quaternion defaultRotation;

        private void Awake()
        {
            currentFOV = cam.fieldOfView;
            defaultFov = currentFOV;
            defaultRotation = transform.rotation;
        }

        private void OnEnable()
        {
            InputManager.Instance.OnResetCameraFOV += ResetFOV;
            InputManager.Instance.OnResetCameraRotation += ResetRotation;

        }

        private void OnDisable()
        {
            InputManager.Instance.OnResetCameraFOV -= ResetFOV;
            InputManager.Instance.OnResetCameraRotation -= ResetRotation;
        }

        private void Update()
        {
            Move();
            Zoom();
            Rotate();
        }

        private void Move()
        {
            Vector2 input = InputManager.Instance.CameraMoveInput;

            if (input == Vector2.zero) return;

            transform.position += Quaternion.Euler(0f, cam.transform.eulerAngles.y, 0f) * new Vector3(input.x, 0f, input.y) * moveSpeed * Time.deltaTime;
        }

        private void Zoom()
        {
            Vector2 input = InputManager.Instance.CameraZoom;

            currentFOV = Mathf.Clamp(currentFOV - input.y * zoomSpeed, minFOV, maxFOV);
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, currentFOV, zoomSmoothness * Time.deltaTime);
        }

        private void Rotate()
        {
            if (!InputManager.Instance.RotateCamera) return;
            transform.Rotate(Vector3.up, InputManager.Instance.MouseDelta().x * rotationSpeed * Time.deltaTime);
        }

        private void ResetFOV() => currentFOV = defaultFov;
        private void ResetRotation() => transform.rotation = defaultRotation;
    }
}
