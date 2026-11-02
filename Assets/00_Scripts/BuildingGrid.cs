using System.Collections.Generic;
using UnityEngine;

public class BuildingGrid : MonoBehaviour
{
    [SerializeField] private int width;
    [SerializeField] private int height;

    private BuildingGridCell[,] grid;

    private void Awake()
    {
        grid = new BuildingGridCell[width, height];
        for(int x = 0; x < grid.GetLength(0); x++)
        {
            for(int y = 0; y < grid.GetLength(1); y++)
            {
                grid[x, y] = new();
            }
        }
    }

    public void SetBuilding(Building building, List<Vector3> allBuildingPositions)
    {
        TrySetBuilding(building, allBuildingPositions);
    }

    public bool TrySetBuilding(Building building, List<Vector3> allBuildingPositions)
    {
        // Validate the entire footprint before modifying any cell.
        if (building == null || !CanBuild(allBuildingPositions)) return false;
        foreach(var p in allBuildingPositions)
        {
            (int x, int y) = WorldToGridPosition(p);
            grid[x, y].SetBuilding(building);
        }
        return true;
    }

    public bool CanBuild(List<Vector3> allBuildingPositions)
    {
        if (grid == null || allBuildingPositions == null || allBuildingPositions.Count == 0) return false;
        foreach(var p in allBuildingPositions)
        {
            (int x, int y) = WorldToGridPosition(p);
            if(x < 0 || x >= width || y < 0 || y >= height) return false;
            if(!grid[x, y].IsEmpty()) return false;
        }
        return true;
    }

    public Vector3 GetCellCenter(Vector3 worldPosition)
    {
        (int x, int y) = WorldToGridPosition(worldPosition);
        return transform.position + new Vector3(
            (x + 0.5f) * BuildingSystem.CellSize, 0,
            (y + 0.5f) * BuildingSystem.CellSize);
    }

    private (int x, int y) WorldToGridPosition(Vector3 worldPosition)
    {
        int x = Mathf.FloorToInt((worldPosition - transform.position).x / BuildingSystem.CellSize);
        int y = Mathf.FloorToInt((worldPosition - transform.position).z / BuildingSystem.CellSize);
        return (x, y);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        if(BuildingSystem.CellSize <= 0 || width <= 0 || height <= 0) return;
        Vector3 origin = transform.position;
        for (int y = 0; y <= height; y++)
        {
            Vector3 start = origin + new Vector3(0, 0.01f, y * BuildingSystem.CellSize);
            Vector3 end = origin + new Vector3(width *BuildingSystem.CellSize, 0.01f, y * BuildingSystem.CellSize);
            Gizmos.DrawLine(start, end);
        }
        for (int x = 0; x <= width; x++)
        {
            Vector3 start = origin + new Vector3(x * BuildingSystem.CellSize, 0.01f, 0);
            Vector3 end = origin + new Vector3(x * BuildingSystem.CellSize, 0.01f, height * BuildingSystem.CellSize);
            Gizmos.DrawLine(start, end);
        }
    }
}

public class BuildingGridCell
{
    private Building building;

    public void SetBuilding(Building building)
    {
        this.building = building;
    }
    
    public bool IsEmpty()
    {
        return building == null;
    }
}
