using System.Collections.Generic;
using UnityEngine;

namespace BuildingSystem.Building
{
    public class BuildingItem : MonoBehaviour
    {
        [SerializeField] private BuildingData data;
        [SerializeField] private GameObject anchor;
        [SerializeField] private GameObject area;
        [SerializeField] private MeshRenderer areaRenderer;
        [SerializeField] private Color availableAreaColor;
        [SerializeField] private Color notAvailableAreaColor;

        private Vector2Int origin;
        private BuildDirection dir;

        private MaterialPropertyBlock materialPropertyBlock;

        private void Awake()
        {
            materialPropertyBlock = new MaterialPropertyBlock();
            areaRenderer.GetPropertyBlock(materialPropertyBlock);
        }

        public void Place(Vector2Int origin, BuildDirection dir)
        {
            this.origin = origin;
            this.dir = dir;
        }

        public void AreaAvailableColor(bool available)
        {
            Color currentColor = available ? availableAreaColor : notAvailableAreaColor;

            if (materialPropertyBlock.GetColor("_BaseColor") == currentColor) return;

            materialPropertyBlock.SetColor("_BaseColor", available ? availableAreaColor : notAvailableAreaColor);
            areaRenderer.SetPropertyBlock(materialPropertyBlock);
        }

        public void OpenPreviewElements()
        {
            anchor.gameObject.SetActive(true);
            area.gameObject.SetActive(true);
        }

        public void DestroySelf() => Destroy(gameObject);

        public List<Vector2Int> GetGridPositionList()
        {
            return data.GetGridPositions(origin, dir);
        }
    }
}
