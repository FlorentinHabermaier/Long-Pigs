using TMPro;
using UnityEngine;

[DefaultExecutionOrder(100)]
public class BuildingSelectionUI : MonoBehaviour
{
    [SerializeField] private BuildingSelection selection;
    [SerializeField] private Camera viewCamera;
    [SerializeField] private RectTransform canvasRect;
    [SerializeField] private RectTransform panel;
    [SerializeField] private TMP_Text infoText;

    [SerializeField] private Vector3 worldOffset =
        new Vector3(0f, 3f, 0f);

    private void Start()
    {
        panel.gameObject.SetActive(false);
    }

    private void LateUpdate()
    {
        Building building = selection.SelectedBuilding;

        if (building == null)
        {
            panel.gameObject.SetActive(false);
            return;
        }

        Vector3 worldPosition =
            building.GetUIAnchorPosition() + worldOffset;

        Vector3 screenPosition =
            viewCamera.WorldToScreenPoint(worldPosition);

        // Gebäude hinter der Kamera: Panel ausblenden.
        if (screenPosition.z <= 0f)
        {
            panel.gameObject.SetActive(false);
            return;
        }

        panel.gameObject.SetActive(true);

        infoText.text =
            $"{building.Description} · Level {building.Level}";

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRect,
            screenPosition,
            null,
            out Vector2 localPosition
        );

        panel.localPosition = new Vector3(
            localPosition.x,
            localPosition.y,
            0f
        );
    }
}