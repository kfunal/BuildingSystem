using System;
using UnityEngine;

namespace BuildingSystem.GridSystem
{
    public class Grid<T>
    {
        private int width;
        private int height;
        private float cellSize;
        private Vector3 originPosition;
        private bool showDebug;
        private T[,] grid;

        public Grid(int width, int height, float cellSize, Vector3 originPosition, bool showDebug, Func<Grid<T>, int, int, T> createGridObject)
        {
            this.width = width;
            this.height = height;
            this.cellSize = cellSize;
            this.originPosition = originPosition;
            this.showDebug = showDebug;

            grid = new T[width, height];

            for (int x = 0; x < grid.GetLength(0); x++)
            {
                for (int z = 0; z < grid.GetLength(1); z++)
                {
                    grid[x, z] = createGridObject(this, x, z);
                }
            }

            HandleDebug();
        }

        public void SetGridObject(int x, int z, T element)
        {
            if (!InBounds(x, z)) return;
            grid[x, z] = element;
        }

        public T GetGridObject(int x, int z)
        {
            if (!InBounds(x, z)) return default;
            return grid[x, z];
        }

        public T GetGridObject(Vector3 worldPosition)
        {
            int x, z;
            GetIndexesFromPosition(worldPosition, out x, out z);
            return GetGridObject(x, z);
        }

        public void GetIndexesFromPosition(Vector3 _worldPosition, out int _x, out int _z)
        {
            _x = Mathf.FloorToInt((_worldPosition - originPosition).x / cellSize);
            _z = Mathf.FloorToInt((_worldPosition - originPosition).z / cellSize);
        }

        public int GetWidth() => width;
        public int GetHeight() => height;
        public float GetCellSize() => cellSize;
        public Vector3 GetWorldPosition(int x, int z) => new Vector3(x, 0, z) * cellSize + originPosition;

        private bool InBounds(int x, int z)
        {
            if (x < 0 || z < 0) return false;
            if (x >= width || z >= height) return false;

            return true;
        }

        private void HandleDebug()
        {
            if (!showDebug) return;

            for (int x = 0; x < grid.GetLength(0); x++)
            {
                for (int z = 0; z < grid.GetLength(1); z++)
                {
                    Debug.DrawLine(GetWorldPosition(x, z), GetWorldPosition(x, z + 1), Color.white, 100f);
                    Debug.DrawLine(GetWorldPosition(x, z), GetWorldPosition(x + 1, z), Color.white, 100f);

                }
            }

            Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.white, 100f);
            Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.white, 100f);
        }
    }
}
