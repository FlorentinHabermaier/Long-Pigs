using UnityEngine;

public class Building : MonoBehaviour
{
    public string Description => data.Description;
    public int Cost => data.Cost;

    public int Level {get; private set;} = 1;
    
    private BuildingModel model;
    private BuildingData data;
    private float productionTimer;
    
    public void Setup(BuildingData data, float rotation)
    {
        this.data = data;
        model = Instantiate(data.Model, transform.position, Quaternion.identity, transform);
        model.SetRotation(rotation);
    }

    public Vector3 GetUIAnchorPosition()
    {
        if (model == null)
            return transform.position;

        Renderer[] renderers = model.GetComponentsInChildren<Renderer>();

        if (renderers.Length == 0)
            return transform.position;

        Bounds bounds = renderers[0].bounds;

        foreach (Renderer renderer in renderers)
        {
            bounds.Encapsulate(renderer.bounds);
        }

        return new Vector3(
            bounds.center.x,
            bounds.max.y,
            bounds.center.z
        );
    }

    private void Update()
    {
        if(data == null)
        {
            return;
        }

        if(data.ProductionAmount <= 0 || data.ProductionInterval <= 0f)
        {
            return;
        }

        ResourceManager resources = ResourceManager.Instance;

        if(resources == null)
        {
            return;
        }

        productionTimer += Time.deltaTime;

        while(productionTimer >= data.ProductionInterval)
        {
            productionTimer -= data.ProductionInterval;
            resources.AddGoods(data.ProductionAmount);
        }
    }
}
