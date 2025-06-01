using HairvestMoon.Utility;
using UnityEngine;
using HairvestMoon.Farming;

#if UNITY_EDITOR
    using UnityEditor;
#endif


namespace HairvestMoon.Tool
{
    /// <summary>
    /// Core tool system management: handles current tool state and tool capacity.
    /// </summary>
    public class ToolSystem : MonoBehaviour
    {
        public static ToolSystem Instance { get; private set; }

        public enum ToolType
        {
            None,
            Hoe,
            WateringCan,
            Seed,
            Harvest
        }

        [Header("Watering Can Settings")]
        public float waterCanCapacity = 100f;
        public float waterPerUse = 1f;

        public ToolType CurrentTool { get; private set; } = ToolType.None;

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }

        public void SetTool(ToolType tool)
        {
            CurrentTool = tool;
            DebugUIOverlay.Instance.ShowLastAction($"Tool: {CurrentTool}");

            if (tool == ToolType.Seed)
                SeedSelectionUI.Instance.OpenSeedMenu();
            else
                SeedSelectionUI.Instance.CloseSeedMenu();

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
