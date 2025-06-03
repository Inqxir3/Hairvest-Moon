using HairvestMoon.Utility;
using UnityEngine;
using HairvestMoon.Farming;
using HairvestMoon.UI;


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
        public static ToolSystem Instance { get; private set; }

        [Header("Watering Can Settings")]
        public float waterCanCapacity = 100f;
        public float waterPerUse = 1f;

        public ToolType CurrentTool { get; private set; } = ToolType.None;

        public void InitializeSingleton()
        {
            Instance = this;
        }

        public void SetTool(ToolType tool)
        {
            CurrentTool = tool;
            DebugUIOverlay.Instance.ShowLastAction($"Tool: {CurrentTool}");

            // Close all selection UIs first
            SeedSelectionUI.Instance?.CloseSeedMenu();
            WateringSelectionUI.Instance?.CloseWateringMenu();
            HoeSelectionUI.Instance?.CloseHoeMenu();
            HarvestSelectionUI.Instance?.CloseHarvestMenu();

            // Open only the active tool's selection UI
            switch (tool)
            {
                case ToolType.Seed:
                    SeedSelectionUI.Instance.OpenSeedMenu();
                    break;
                case ToolType.WateringCan:
                    WateringSelectionUI.Instance.OpenWateringMenu();
                    break;
                case ToolType.Hoe:
                    HoeSelectionUI.Instance.OpenHoeMenu();
                    break;
                case ToolType.Harvest:
                    HarvestSelectionUI.Instance.OpenHarvestMenu();
                    break;
            }
        }


        public void ConsumeWaterFromCan()
        {
            waterCanCapacity -= waterPerUse;
            waterCanCapacity = Mathf.Max(0f, waterCanCapacity);
            DebugUIOverlay.Instance.ShowLastAction($"Water Remaining: {waterCanCapacity}");
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
