using HairvestMoon.Core;
using HairvestMoon.Farming;
using HairvestMoon.Interaction;
using HairvestMoon.Inventory;
using HairvestMoon.Player;
using HairvestMoon.Tool;
using HairvestMoon.UI;
using HairvestMoon.Utility;
using UnityEngine;

public class GameInitializer : MonoBehaviour
{
    [Header("Event Bus")]
    public GameEventBus gameEventBus;

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
    public InventoryOverviewUI inventoryOverviewUI;
    public BackpackCapacityBarUI backpackCapacityBarUI;
    public MainMenuUIManager mainMenuUIManager;

    [Header("Gameplay Systems")]
    public ToolSystem toolSystem;
    public ToolSelector toolSelector;
    public FarmToolHandler farmToolHandler;

    [Header("Debug Systems")]
    public DebugUIOverlay debugUIOverlay;


    private void Awake()
    {
        // Reset Service Locator
        ServiceLocator.Clear();

        // Core Systems
        ServiceLocator.Register(this);
        ServiceLocator.Register(gameManager);
        ServiceLocator.Register(gameStateManager);
        ServiceLocator.Register(gameTimeManager);
        ServiceLocator.Register(inputController);
        ServiceLocator.Register(playerStateController);
        ServiceLocator.Register(playerController);
        ServiceLocator.Register(playerFacingController);
        ServiceLocator.Register(tileTargetingSystem);

        // Databases
        ServiceLocator.Register(seedDatabase);

        // Farming Systems
        ServiceLocator.Register(inventorySystem);
        ServiceLocator.Register(backpackInventorySystem);
        ServiceLocator.Register(backpackEquipSystem);
        ServiceLocator.Register(backpackEquipInstallManager);
        ServiceLocator.Register(backpackUpgradeManager);

        // UI
        ServiceLocator.Register(mainMenuUIManager);
        ServiceLocator.Register(seedSelectionUI);
        ServiceLocator.Register(wateringSelectionUI);
        ServiceLocator.Register(hoeSelectionUI);
        ServiceLocator.Register(harvestSelectionUI);
        ServiceLocator.Register(selectionTooltipUI);
        ServiceLocator.Register(backpackInventoryUI);
        ServiceLocator.Register(inventoryOverviewUI);
        ServiceLocator.Register(backpackCapacityBarUI);

        // Gameplay
        ServiceLocator.Register(toolSystem);
        ServiceLocator.Register(toolSelector);
        ServiceLocator.Register(farmToolHandler);

        // Debug
        ServiceLocator.Register(debugUIOverlay);

        // Event Wiring
        ServiceLocator.Register(gameEventBus);
    }

    private void Start()
    {
        InitializeUI();

        InitializeGame();

        RegisterAllBusListeners();
    }

    private void InitializeUI()
    {
        // Phase 1 — UI initialization
        mainMenuUIManager.InitializeUI();
        backpackInventoryUI.InitializeUI();
        seedSelectionUI.InitializeUI();
        wateringSelectionUI.InitializeUI();
        hoeSelectionUI.InitializeUI();
        harvestSelectionUI.InitializeUI();
        selectionTooltipUI.InitializeUI();
        inventoryOverviewUI.InitializeUI();
        backpackCapacityBarUI.InitializeUI();

        // Disable all selection menus initially
        seedSelectionUI.CloseSeedMenu();
        wateringSelectionUI.CloseWateringMenu();
        hoeSelectionUI.CloseHoeMenu();
        harvestSelectionUI.CloseHarvestMenu();
    }

    private void InitializeGame()
    {
        // Phase 2 — Backpack Upgrades
        backpackUpgradeManager.Initialize();

        // Phase 3 — Gameplay systems prep
        farmToolHandler.Initialize();
        seedDatabase.InitializeSeedDatabase();
        toolSelector.InitialSetTool();
        inputController.InitInput();
        gameStateManager.InitializeGameState();
        playerStateController.InitializePlayerState();
    }

    private void RegisterAllBusListeners()
    {
        foreach (var listener in GetComponentsInChildren<IBusListener>(true))
        {
            listener.RegisterBusListeners();
        }
    }
}
