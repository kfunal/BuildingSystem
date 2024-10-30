using System;
using System.Collections.Generic;
using BuildingSystem.GridSystem;
using BuildingSystem.Input;
using UnityEngine;

namespace BuildingSystem.Building
{
    public class BuildingController : MonoBehaviour
    {
        [Header("Raycast")]
        [SerializeField] private Camera cam;
        [SerializeField] private LayerMask groundLayer;
        [SerializeField] private LayerMask buildingLayer;
        [SerializeField] private float raycastDistance;

        [Header("Building Data")]
        [SerializeField] private BuildingData[] buildings;

        [Header("Grid Parameters")]
        [SerializeField] private int gridWidth;
        [SerializeField] private int gridHeight;
        [SerializeField] private float gridCellSize;
        [SerializeField, Tooltip("Enable gizmos on game window to visualize debug")] private bool showDebug;

        [Header("References")]
        [SerializeField] private Transform ground;
        [SerializeField] private Transform buildingsParent;
        [SerializeField] private Material groundGridMaterial;

        private Vector3 gridOrigin;
        private Grid<GridObject<BuildingItem>> grid;
        private BuildingData selectedBuilding;
        private BuildingItem previewObject;
        private BuildDirection currentDirection = BuildDirection.Down;

        private void Awake()
        {
            gridOrigin = new Vector3(-gridWidth * gridCellSize / 2f, 0, -gridHeight * gridCellSize / 2f);
            grid = new Grid<GridObject<BuildingItem>>(gridWidth, gridHeight, gridCellSize, gridOrigin, showDebug, CreateGridObject);

            float groundScaleAmount = gridWidth * gridHeight / 2f;
            ground.transform.localScale = new Vector3(gridWidth, 1, gridHeight);

            groundGridMaterial.SetVector("_GridXY", new Vector2(gridWidth, gridHeight));
        }

        private void OnEnable()
        {
            InputManager.Instance.OnSpawnBuildingKeyPressed += SpawnBuilding;
            InputManager.Instance.OnRotateBuildingKeyPressed += RotateBuilding;
            InputManager.Instance.OnDestroyBuildingKeyPressed += DestroyBuilding;
        }

        private void OnDisable()
        {
            InputManager.Instance.OnSpawnBuildingKeyPressed -= SpawnBuilding;
            InputManager.Instance.OnRotateBuildingKeyPressed -= RotateBuilding;
            InputManager.Instance.OnDestroyBuildingKeyPressed -= DestroyBuilding;
        }

        private void Update()
        {
            if (previewObject == null) return;

            RayToGrid(InputManager.Instance.MousePosition(), groundLayer, (RaycastHit hit) =>
            {
                previewObject.transform.position = hit.point;
                previewObject.transform.rotation = Quaternion.Euler(0f, selectedBuilding.GetRotationAngle(currentDirection), 0f);

                previewObject.AreaAvailableColor(CanBuild(hit.point, out int x, out int z));
            });
        }

        private void SpawnBuilding(Vector2 mousePosition)
        {
            if (selectedBuilding == null) return;

            RayToGrid(mousePosition, groundLayer, (RaycastHit hit) =>
            {
                if (!CanBuild(hit.point, out int x, out int z)) return;

                List<Vector2Int> gridPositions = selectedBuilding.GetGridPositions(new Vector2Int(x, z), currentDirection);
                Vector2Int rotationOffset = selectedBuilding.GetRotationOffset(currentDirection);
                Vector3 buildingPosition = grid.GetWorldPosition(x, z) + new Vector3(rotationOffset.x, 0f, rotationOffset.y) * grid.GetCellSize();

                BuildingItem buildObject = Instantiate(selectedBuilding.Prefab, buildingPosition, Quaternion.Euler(0f, selectedBuilding.GetRotationAngle(currentDirection), 0), buildingsParent).GetComponent<BuildingItem>();
                buildObject.Place(new Vector2Int(x, z), currentDirection);

                for (int i = 0; i < gridPositions.Count; i++)
                    grid.GetGridObject(gridPositions[i].x, gridPositions[i].y).SetPlacedObject(buildObject);
            });
        }

        private void DestroyBuilding(Vector2 mousePosition)
        {
            RayToGrid(mousePosition, buildingLayer, (RaycastHit hit) =>
            {
                BuildingItem placedObject = hit.transform.GetComponent<BuildingItem>();
                List<Vector2Int> gridPositions = placedObject.GetGridPositionList();

                for (int i = 0; i < gridPositions.Count; i++)
                    grid.GetGridObject(gridPositions[i].x, gridPositions[i].y).ClearPlacedObject();

                placedObject.DestroySelf();
            });
        }

        private void RotateBuilding() => currentDirection = GetNextDirection(currentDirection);

        private void RayToGrid(Vector2 mousePosition, LayerMask layer, Action<RaycastHit> onHit)
        {
            Ray ray = cam.ScreenPointToRay(mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, raycastDistance, layer))
                onHit?.Invoke(hit);
        }

        private bool AreaAvailable(List<Vector2Int> positions)
        {
            for (int i = 0; i < positions.Count; i++)
            {
                Vector2Int position = positions[i];

                if (grid.GetGridObject(position.x, position.y) == null) return false;
                if (!grid.GetGridObject(position.x, position.y).Empty()) return false;
            }

            return true;
        }

        private bool CanBuild(Vector3 position, out int x, out int y)
        {
            grid.GetIndexesFromPosition(position, out int i, out int j);
            List<Vector2Int> gridPositions = selectedBuilding.GetGridPositions(new Vector2Int(i, j), currentDirection);
            x = i;
            y = j;

            if (!AreaAvailable(gridPositions)) return false;

            return true;
        }

        private GridObject<BuildingItem> CreateGridObject(Grid<GridObject<BuildingItem>> grid, int x, int z) => new GridObject<BuildingItem>(grid, x, z);

        public BuildDirection GetNextDirection(BuildDirection direction)
        {
            return direction switch
            {
                BuildDirection.Down => BuildDirection.Left,
                BuildDirection.Left => BuildDirection.Up,
                BuildDirection.Up => BuildDirection.Right,
                BuildDirection.Right => BuildDirection.Down,
                _ => BuildDirection.Down,
            };
        }

        public void OnBuildingButtonClicked(int index)
        {
            if (index < 0 || index >= buildings.Length) return;

            selectedBuilding = buildings[index];

            if (previewObject != null && selectedBuilding != null && previewObject.name.Contains(selectedBuilding.Name)) return;

            if (previewObject != null)
            {
                Destroy(previewObject.gameObject);
                previewObject = null;
            }

            if (index == 0) return;

            previewObject = Instantiate(selectedBuilding.Prefab, InputManager.Instance.MousePosition(), Quaternion.identity, buildingsParent).GetComponent<BuildingItem>();
            previewObject.OpenPreviewElements();
        }
    }
}
