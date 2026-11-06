using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class BuildingSystem : MonoBehaviour
{
    public const float CellSize = 1f;

    [SerializeField] private BuildingData buildingData1;
    [SerializeField] private BuildingData buildingData2;
    [SerializeField] private BuildingPreview previewPrefab;
    [SerializeField] private Building buildingPrefab;
    [SerializeField] private BuildingGrid grid;

    private BuildingPreview preview;
    private int lastPlacementEndFrame = -1;

    public bool isPlacing =>
        preview != null || lastPlacementEndFrame == Time.frameCount;

    private void Update()
    {
        Keyboard keyboard = Keyboard.current;
        Mouse mouse = Mouse.current;
        if (grid == null || mouse == null) return;

        if ((keyboard != null && keyboard.escapeKey.wasPressedThisFrame)
            || mouse.rightButton.wasPressedThisFrame)
        {
            CancelPreview();
            return;
        }

        if (!TryGetMouseWorldPosition(mouse.position.ReadValue(), out Vector3 position))
        {
            if (preview != null) preview.gameObject.SetActive(false);
            return;
        }

        if (keyboard != null)
        {
            if (keyboard.digit1Key.wasPressedThisFrame) SelectBuilding(buildingData1, position);
            else if (keyboard.digit2Key.wasPressedThisFrame) SelectBuilding(buildingData2, position);
        }

        if (preview == null) return;
        preview.gameObject.SetActive(true);
        if (keyboard != null && keyboard.rKey.wasPressedThisFrame) preview.Rotate(90);

        // Align one footprint cell; all other cells retain their rotated offsets.
        preview.transform.position = position;
        List<Vector3> positions = preview.BuildingModel.GetAllBuildingPositions();
        if (positions.Count > 0)
        {
            Vector3 offset = grid.GetCellCenter(positions[0]) - positions[0];
            offset.y = 0;
            preview.transform.position += offset;
            positions = preview.BuildingModel.GetAllBuildingPositions();
        }

        bool overUI = EventSystem.current != null && EventSystem.current.IsPointerOverGameObject();
        bool canBuild = !overUI && grid.CanBuild(positions);
        preview.ChangeState(canBuild
            ? BuildingPreview.BuildingPreviewState.POSITIVE
            : BuildingPreview.BuildingPreviewState.NEGATIVE);

        if (canBuild && mouse.leftButton.wasPressedThisFrame) PlaceBuilding(positions);
    }

    private void PlaceBuilding(List<Vector3> positions)
    {
        if (preview == null || !grid.CanBuild(positions)) return;

        Building building = Instantiate(buildingPrefab, preview.transform.position, Quaternion.identity);
        building.Setup(preview.Data, preview.BuildingModel.Rotation);
        if (!grid.TrySetBuilding(building, positions))
        {
            Destroy(building.gameObject);
            return;
        }
        CancelPreview();
    }

    private bool TryGetMouseWorldPosition(Vector2 screenPosition, out Vector3 position)
    {
        position = default;
        Camera camera = Camera.main;
        if (camera == null) return false;
        Ray ray = camera.ScreenPointToRay(screenPosition);
        Plane ground = new Plane(Vector3.up, grid.transform.position);
        if (!ground.Raycast(ray, out float distance)) return false;
        position = ray.GetPoint(distance);
        return true;
    }

    private void SelectBuilding(BuildingData data, Vector3 position)
    {
        if (data == null || data.Model == null || previewPrefab == null || buildingPrefab == null)
        {
            Debug.LogWarning("BuildingSystem: Building data, model and prefabs must be assigned.", this);
            return;
        }
        CancelPreview();
        preview = Instantiate(previewPrefab, position, Quaternion.identity);
        preview.Setup(data);
    }

    private void CancelPreview()
    {
        if (preview == null) return;
        lastPlacementEndFrame = Time.frameCount;
        preview.gameObject.SetActive(false);
        Destroy(preview.gameObject);
        preview = null;
        
    }

    private void OnDisable()
    {
        CancelPreview();
    }
}
