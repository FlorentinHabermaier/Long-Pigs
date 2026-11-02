using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BuildingModel : MonoBehaviour
{
    [SerializeField] private Transform wrapper;
    public float Rotation => wrapper.localEulerAngles.y;
    public float rotation => Rotation;
    private BuildingShapeUnit[] shapeUnits;
    
    private void Awake()
    {
        shapeUnits = GetComponentsInChildren<BuildingShapeUnit>();
    }

    public void Rotate(float rotationStep)
    {
        SetRotation(Rotation + rotationStep);
    }

    public void SetRotation(float angle)
    {
        wrapper.localRotation = Quaternion.Euler(0f, Mathf.Repeat(angle, 360f), 0f);
    }

    public List<Vector3> GetAllBuildingPositions()
    {
        return shapeUnits.Select(unit => unit.transform.position).ToList();
    }

}
