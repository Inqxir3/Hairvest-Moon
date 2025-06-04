using HairvestMoon.Utility;
using UnityEngine;
using HairvestMoon.Farming;
using HairvestMoon.UI;
using HairvestMoon.Core;



#if UNITY_EDITOR
using UnityEditor;
#endif


namespace HairvestMoon.Tool
{
    /// <summary>
    /// Core tool system management: handles current tool state and tool capacity.
    /// </summary>
    public partial class ToolSystem : MonoBehaviour
    {
        [Header("Watering Can Settings")]
        public float waterCanCapacity = 100f;
        public float waterPerUse = 1f;

        public ToolType CurrentTool { get; private set; } = ToolType.None;

        public void SetTool(ToolType tool)
        {
            CurrentTool = tool;
            ServiceLocator.Get<DebugUIOverlay>().ShowLastAction($"Tool: {CurrentTool}");

            // Close all selection UIs first
            ServiceLocator.Get<SeedSelectionUI>()?.CloseSeedMenu();
            ServiceLocator.Get<WateringSelectionUI>()?.CloseWateringMenu();
            ServiceLocator.Get<HoeSelectionUI>()?.CloseHoeMenu();
            ServiceLocator.Get<HarvestSelectionUI>()?.CloseHarvestMenu();

            // Open only the active tool's selection UI
            switch (tool)
            {
                case ToolType.Seed:
                    ServiceLocator.Get<SeedSelectionUI>().OpenSeedMenu();
                    break;
                case ToolType.WateringCan:
                    ServiceLocator.Get<WateringSelectionUI>().OpenWateringMenu();
                    break;
                case ToolType.Hoe:
                    ServiceLocator.Get<HoeSelectionUI>().OpenHoeMenu();
                    break;
                case ToolType.Harvest:
                    ServiceLocator.Get<HarvestSelectionUI>().OpenHarvestMenu();
                    break;
            }
        }


        public void ConsumeWaterFromCan()
        {
            waterCanCapacity -= waterPerUse;
            waterCanCapacity = Mathf.Max(0f, waterCanCapacity);
            ServiceLocator.Get<DebugUIOverlay>().ShowLastAction($"Water Remaining: {waterCanCapacity}");
        }

        public void RefillWaterCan(float refillAmount)
        {
            waterCanCapacity += refillAmount;
            // Optional: Clamp to some max value if you want limits
        }

        public void RefillWaterToFull()
        {
            waterCanCapacity = 100f;  // we need to make this a constant or configurable value, consider modifications to max capacity later.
        }



#if UNITY_EDITOR
        [CustomEditor(typeof(ToolSystem))]
        public class ToolSystemEditor : Editor
        {
            public override void OnInspectorGUI()
            {
                DrawDefaultInspector();

                ToolSystem toolSystem = (ToolSystem)target;
                if (GUILayout.Button("Refill Water (Dev Only)"))
                {
                    toolSystem.RefillWaterToFull();
                }
            }
        }
        #endif

    }
}
