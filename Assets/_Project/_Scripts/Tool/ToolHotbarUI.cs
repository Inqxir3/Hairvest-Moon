using UnityEngine;

namespace HairvestMoon.Tool
{

    /// <summary>
    /// Manages the visual UI of tool slots and highlights the selected one.
    /// </summary>
    public class ToolHotbarUI : MonoBehaviour
    {
        [SerializeField] private ToolSlot[] slots;

        public void HighlightTool(ToolSystem.ToolType selected)
        {
            foreach (ToolSlot slot in slots)
            {
                slot.SetHighlighted(slot.Tool == selected);
            }
        }
    }
}