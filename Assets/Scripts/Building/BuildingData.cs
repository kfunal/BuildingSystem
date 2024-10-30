using System.Collections.Generic;
using UnityEngine;

namespace BuildingSystem.Building
{
    [CreateAssetMenu(fileName = "BuildingData", menuName = "Scriptable Objects/BuildingData")]
    public class BuildingData : ScriptableObject
    {
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public Transform Prefab { get; private set; }

        [field: SerializeField] public int Width { get; private set; }
        [field: SerializeField] public int Height { get; private set; }

        private List<Vector2Int> gridPositionList = new List<Vector2Int>();

        public int GetRotationAngle(BuildDirection direction)
        {
            return direction switch
            {
                BuildDirection.Down => 0,
                BuildDirection.Left => 90,
                BuildDirection.Up => 180,
                BuildDirection.Right => 270,
                _ => 0
            };
        }

        public Vector2Int GetRotationOffset(BuildDirection direction)
        {
            return direction switch
            {
                BuildDirection.Down => new Vector2Int(0, 0),
                BuildDirection.Left => new Vector2Int(0, Width),
                BuildDirection.Up => new Vector2Int(Width, Height),
                BuildDirection.Right => new Vector2Int(Height, 0),
                _ => new Vector2Int(0, 0)
            };
        }

        public List<Vector2Int> GetGridPositions(Vector2Int offset, BuildDirection dir)
        {
            gridPositionList.Clear();

            if (dir == BuildDirection.Up || dir == BuildDirection.Down)
            {
                for (int x = 0; x < Width; x++)
                {
                    for (int y = 0; y < Height; y++)
                        gridPositionList.Add(offset + new Vector2Int(x, y));
                }
            }
            else
            {
                for (int x = 0; x < Height; x++)
                {
                    for (int y = 0; y < Width; y++)
                        gridPositionList.Add(offset + new Vector2Int(x, y));
                }
            }

            return gridPositionList;
        }

    }

    public enum BuildDirection
    {
        Up,
        Down,
        Left,
        Right,
    }
}
