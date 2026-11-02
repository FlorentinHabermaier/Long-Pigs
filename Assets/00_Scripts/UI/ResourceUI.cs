using UnityEngine;
using TMPro;

public class ResourceUI : MonoBehaviour
{
    [SerializeField]
    private TMP_Text moneyText;

    [SerializeField]
    private TMP_Text goodsText;

    private ResourceManager resourceManager;

    private void Start()
    {
        resourceManager = ResourceManager.Instance;

        if(resourceManager == null || moneyText == null || goodsText == null)
        {
            Debug.Log("ResourceUI: ResourceManager, GoodsText oder MoneyText fehlt", this);
            return;
        }
        
        resourceManager.ResourceChanged += RefreshUI;
        RefreshUI(); 
    }

    private void RefreshUI()
    {
        moneyText.text = $"Geld: {resourceManager.Money}";
        goodsText.text = $"Waren: {resourceManager.Goods}";
    }

    private void OnDestroy()
    {
        if(resourceManager != null)
        {
            resourceManager.ResourceChanged -= RefreshUI;
        }
    }
}
