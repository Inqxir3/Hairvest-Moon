using HairvestMoon.Core;
using UnityEngine;
using UnityEngine.InputSystem;

namespace HairvestMoon.Tool
{

    /// <summary>
    /// Handles tool selection via keyboard hotkeys and input actions for next/previous.
    /// Updates ToolSystem and notifies UI.
    /// </summary>
    public class ToolSelector : MonoBehaviour
    {
        public static ToolSelector Instance { get; private set; }

        [SerializeField] private ToolHotbarUI toolHotbar;

        private ToolSystem.ToolType[] toolOrder = new[]
        {
        ToolSystem.ToolType.Hoe,
        ToolSystem.ToolType.WateringCan,
        ToolSystem.ToolType.Seed,
        ToolSystem.ToolType.Harvest
    };

        private int currentIndex = 0;

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }
        private void Start()
        {
            InputController.Instance.OnToolNext += HandleNext;
            InputController.Instance.OnToolPrevious += HandlePrevious;
            SetTool(toolOrder[currentIndex]);
        }

        private void OnDisable()
        {
            InputController.Instance.OnToolNext -= HandleNext;
            InputController.Instance.OnToolPrevious -= HandlePrevious;
        }

        private void Update()
        {
            // Number hotkeys (1–4)
            if (Keyboard.current.digit1Key.wasPressedThisFrame) SetToolByIndex(0);
            if (Keyboard.current.digit2Key.wasPressedThisFrame) SetToolByIndex(1);
            if (Keyboard.current.digit3Key.wasPressedThisFrame) SetToolByIndex(2);
            if (Keyboard.current.digit4Key.wasPressedThisFrame) SetToolByIndex(3);
        }

        private void HandleNext() => CycleTool(1);
        private void HandlePrevious() => CycleTool(-1);

        private void SetToolByIndex(int index)
        {
            currentIndex = Mathf.Clamp(index, 0, toolOrder.Length - 1);
            SetTool(toolOrder[currentIndex]);
        }

        private void CycleTool(int direction)
        {
            currentIndex = (currentIndex + direction + toolOrder.Length) % toolOrder.Length;
            SetTool(toolOrder[currentIndex]);
        }

        private void SetTool(ToolSystem.ToolType tool)
        {
            ToolSystem.Instance.SetTool(tool);
            toolHotbar?.HighlightTool(tool);
        }

        public void SelectToolExternally(ToolSystem.ToolType tool)
        {
            for (int i = 0; i < toolOrder.Length; i++)
            {
                if (toolOrder[i] == tool)
                {
                    currentIndex = i;
                    SetTool(toolOrder[currentIndex]);
                    return;
                }
            }
        }

    }
}