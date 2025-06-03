using HairvestMoon.Core;
using HairvestMoon.Farming;
using HairvestMoon.Interaction;
using HairvestMoon.Inventory;
using HairvestMoon.Player;
using HairvestMoon.Tool;
using HairvestMoon.UI;
using UnityEngine;

public class GameInitializer : MonoBehaviour
{
    [Header("Core Game Systems")]
    public GameManager gameManager;
    public GameStateManager gameStateManager;
    public GameTimeManager gameTimeManager;
    public InputController inputController;
    public PlayerStateController playerStateController;
    public Player_Controller playerController;
    public PlayerFacingController playerFacingController;
    public TileTargetingSystem tileTargetingSystem;

    [Header("Databases")]
    public SeedDatabase seedDatabase;

    [Header("Farming Core Systems")]
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

    [Header("Gameplay Systems")]
    public ToolSystem toolSystem;
    public ToolSelector toolSelector;
    public FarmToolHandler farmToolHandler;

    private void Awake()
    {
        // PHASE 1 — Core Game Systems
        gameStateManager.InitializeSingleton();
        gameTimeManager.InitializeSingleton();
        inputController.InitializeSingleton();
        gameManager.InitializeSingleton();
        playerStateController.InitializeSingleton();
        playerController.InitializeSingleton();
        playerFacingController.InitializeSingleton();
        tileTargetingSystem.InitializeSingleton();

        // PHASE 2 — Databases
        seedDatabase.InitializeSingleton();

        // PHASE 3 — Farming Systems
        inventorySystem.InitializeSingleton();
        backpackInventorySystem.InitializeSingleton();
        backpackEquipSystem.InitializeSingleton();
        backpackEquipInstallManager.InitializeSingleton();
        backpackUpgradeManager.InitializeSingleton();

        // PHASE 4 — Apply Backpack Upgrades AFTER inventory live
        backpackUpgradeManager.Initialize();

        // PHASE 5 — UI Systems
        backpackInventoryUI.InitializeUI();
        seedSelectionUI.InitializeUI();
        wateringSelectionUI.InitializeUI();
        hoeSelectionUI.InitializeUI();
        harvestSelectionUI.InitializeUI();
        selectionTooltipUI.InitializeUI();

        // Disable all selection UIs on boot
        seedSelectionUI.CloseSeedMenu();
        wateringSelectionUI.CloseWateringMenu();
        hoeSelectionUI.CloseHoeMenu();
        harvestSelectionUI.CloseHarvestMenu();

        // PHASE 6 — Gameplay Systems
        toolSystem.InitializeSingleton();
        toolSelector.InitializeSingleton();
        farmToolHandler.Initialize();
    }
}
