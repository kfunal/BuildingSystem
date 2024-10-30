using UnityEngine;

namespace BuildingSystem.GridSystem
{
    public class GridObject<T>
    {
        private int x;
        private int z;

        private T placedObject;
        private Grid<GridObject<T>> grid;

        public GridObject(Grid<GridObject<T>> grid, int x, int z)
        {
            this.grid = grid;
            this.x = x;
            this.z = z;
        }

        public void SetPlacedObject(T obj) => placedObject = obj;
        public void ClearPlacedObject() => placedObject = default;
        public T GetPlacedObject() => placedObject;
        public bool Empty() => placedObject == null;
    }
}
