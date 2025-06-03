using HairvestMoon.Farming;
using HairvestMoon.Inventory;
using HairvestMoon.Tool;
using HairvestMoon.UI;
using UnityEngine;

public class GameInitializer : MonoBehaviour
{
    [Header("Systems")]
    public InventorySystem inventorySystem;
    public BackpackInventorySystem backpackInventorySystem;
    public BackpackEquipSystem backpackEquipSystem;
    public BackpackEquipInstallManager backpackEquipInstallManager;
    public BackpackUpgradeManager backpackUpgradeManager;

    [Header("UI")]
    public BackpackInventoryUI backpackInventoryUI;
    public SeedSelectionUI seedSelectionUI;
    public WateringSelectionUI wateringSelectionUI;
    public HoeSelectionUI hoeSelectionUI;
    public HarvestSelectionUI harvestSelectionUI;
    public SelectionTooltipUI selectionTooltipUI;

    [Header("Gameplay")]
    public ToolSystem toolSystem;
    public FarmToolHandler farmToolHandler;

    private void Awake()
    {
        // PHASE 1: Data Systems
        inventorySystem.InitializeSingleton();
        backpackInventorySystem.InitializeSingleton();
        backpackEquipSystem.InitializeSingleton();
        backpackEquipInstallManager.InitializeSingleton();
        backpackUpgradeManager.InitializeSingleton();

        // PHASE 2: Apply Upgrade logic AFTER inventory systems are ready
        backpackUpgradeManager.Initialize();

        // PHASE 3: UI Systems (they can now safely subscribe to events)
        backpackInventoryUI.InitializeUI();
        seedSelectionUI.InitializeUI();
        wateringSelectionUI.InitializeUI();
        hoeSelectionUI.InitializeUI();
        harvestSelectionUI.InitializeUI();
        selectionTooltipUI.InitializeUI();

        // PHASE 4: Gameplay systems
        toolSystem.InitializeSingleton();
        farmToolHandler.Initialize();
    }
}

