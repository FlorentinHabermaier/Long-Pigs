using UnityEngine;

public class Building : MonoBehaviour
{
    public string Description => data.Description;
    public int Cost => data.Cost;
    
    private BuildingModel model;
    private BuildingData data;
    private float productionTimer;
    
    public void Setup(BuildingData data, float rotation)
    {
        this.data = data;
        model = Instantiate(data.Model, transform.position, Quaternion.identity, transform);
        model.SetRotation(rotation);
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
