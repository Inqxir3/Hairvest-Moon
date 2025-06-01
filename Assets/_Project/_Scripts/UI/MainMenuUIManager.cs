using System.Collections.Generic;
using UnityEngine;
using HairvestMoon.Core;
using HairvestMoon.Inventory;

public class MainMenuUIManager : MonoBehaviour
{
    public static MainMenuUIManager Instance { get; private set; }

    [Header("UI Panels")]
    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private GameObject cropLogPanel;
    [SerializeField] private GameObject mapPanel;
    [SerializeField] private GameObject questPanel;
    [SerializeField] private GameObject settingsPanel;

    [Header("Tab Buttons")]
    [SerializeField] private List<GameObject> tabButtons;

    private int currentTabIndex = 0;
    private GameObject[] allPanels;

    private void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;

        allPanels = new GameObject[] { inventoryPanel, cropLogPanel, mapPanel, questPanel, settingsPanel };
    }

    private void Start()
    {
        CloseMenu();
    }

    public void OpenMenu()
    {
        gameObject.SetActive(true);
        GameStateManager.Instance.SetState(GameState.Menu);
        OpenTab(0);
    }

    public void CloseMenu()
    {
        gameObject.SetActive(false);
        if (GameStateManager.Instance.CurrentState == GameState.Menu)
            GameStateManager.Instance.SetState(GameState.FreeRoam);
    }

    public void OpenTab(int index)
    {
        currentTabIndex = index;

        for (int i = 0; i < allPanels.Length; i++)
            allPanels[i].SetActive(i == currentTabIndex);

        // Optional: refresh inventory when tab is opened
        if (currentTabIndex == 1)
        {
            cropLogPanel.GetComponent<InventoryOverviewUI>().RefreshUI();
        }
    }

    public void NextTab()
    {
        int nextIndex = (currentTabIndex + 1) % allPanels.Length;
        OpenTab(nextIndex);
    }

    public void PreviousTab()
    {
        int prevIndex = (currentTabIndex - 1 + allPanels.Length) % allPanels.Length;
        OpenTab(prevIndex);
    }
}
