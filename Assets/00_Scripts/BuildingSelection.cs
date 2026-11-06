using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class BuildingSelection : MonoBehaviour
{
    [SerializeField] private BuildingSystem buildingSystem;
    [SerializeField] private Camera viewCamera;
    
    public Building SelectedBuilding { get; private set; }

    private void LateUpdate()
    {
        if (buildingSystem == null || viewCamera == null)
        {
            return;
        }

        if(buildingSystem.isPlacing)
        {
            SelectedBuilding = null;
            return;
        }

        Mouse mouse = Mouse.current;

        if(EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        Ray ray = viewCamera.ScreenPointToRay(
            mouse.position.ReadValue()
        );

        Building clickedBuilding = null;

        if (Physics.Raycast(
            ray,
            out RaycastHit hit,
            Mathf.Infinity,
            Physics.DefaultRaycastLayers,
            QueryTriggerInteraction.Ignore))
        {
            clickedBuilding =
                hit.collider.GetComponentInParent<Building>();
        }

        SelectBuilding(clickedBuilding);
    }

    private void SelectBuilding(Building building)
    {
        if (SelectedBuilding == building)
            return;

        SelectedBuilding = building;

        if (SelectedBuilding != null)
        {
            Debug.Log(
                $"Ausgewählt: {SelectedBuilding.Description}",
                SelectedBuilding
            );
        }
        else
        {
            Debug.Log("Gebäudeauswahl aufgehoben.");
        }
    }
}
