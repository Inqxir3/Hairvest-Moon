using System.Collections.Generic;
using UnityEngine;
using HairvestMoon.Core;
using HairvestMoon.Inventory;

namespace HairvestMoon.UI
{
    ///
    public class MainMenuUIManager : MonoBehaviour, IBusListener
    {
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

        public void InitializeUI()
        {
            allPanels = new GameObject[] { inventoryPanel, cropLogPanel, mapPanel, questPanel, settingsPanel };
            CloseMenu();
        }

        public void RegisterBusListeners()
        {
            var bus = ServiceLocator.Get<GameEventBus>();
            bus.MenuToggle += HandleMenuToggle;
        }

        public void OpenMenu()
        {
            gameObject.SetActive(true);
            ServiceLocator.Get<GameStateManager>().SetState(GameState.Menu);
            OpenTab(0);
        }

        public void CloseMenu()
        {
            gameObject.SetActive(false);
            if (ServiceLocator.Get<GameStateManager>().CurrentState == GameState.Menu)
                ServiceLocator.Get<GameStateManager>().SetState(GameState.FreeRoam);
        }

        private void HandleMenuToggle()
        {
            if (ServiceLocator.Get<GameStateManager>().CurrentState == GameState.Menu)
                CloseMenu();
            else
                OpenMenu();
        }

        public void OpenTab(int index)
        {
            currentTabIndex = index;

            for (int i = 0; i < allPanels.Length; i++)
                allPanels[i].SetActive(i == currentTabIndex);

            if (currentTabIndex == 0)
            {
               inventoryPanel.GetComponent<BackpackInventoryUI>().RefreshUI();
            }

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
}