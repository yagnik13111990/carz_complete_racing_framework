using UnityEngine;

public class GlobalBootstrap : MonoBehaviour
{
    void Awake()
    {
        var locator = ServiceLocator.Instance;
       
        // Wallet Manager
        if (!locator.IsRegistered<WalletManager>())
            locator.RegisterIntoService(new WalletManager());

        // Inventory Manager
        if (!locator.IsRegistered<InventoryManager>())
            locator.RegisterIntoService(new InventoryManager());

        // Setting Manager
        if (!locator.IsRegistered<SettingManager>()) 
            locator.RegisterIntoService(new SettingManager());

        // Level Manager
        if(!locator.IsRegistered<LevelManager>())
            locator.RegisterIntoService(new LevelManager());

        // Race Manager
        if(!locator.IsRegistered<RaceManager>())
            locator.RegisterIntoService(new  RaceManager());    

    }
}
