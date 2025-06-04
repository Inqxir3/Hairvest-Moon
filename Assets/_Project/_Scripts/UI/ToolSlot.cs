using HairvestMoon.Utility;
using UnityEngine;
using HairvestMoon.Tool;
using UnityEngine.UI;
using HairvestMoon.Core;

namespace HairvestMoon.UI
{

    /// <summary>
    /// A UI button representing a tool in the hotbar.
    /// </summary>
    public class ToolSlot : MonoBehaviour
    {
        [SerializeField] private ToolType tool;
        [SerializeField] private Image highlightImage;

        public ToolType Tool => tool;

        private void Awake()
        {
            GetComponent<Button>().onClick.AddListener(() =>
            {
                ServiceLocator.Get<ToolSelector>().SelectToolExternally(tool);
                ServiceLocator.Get<DebugUIOverlay>().ShowLastAction($"Tool: {tool}");
            });
        }

        public void SetHighlighted(bool isHighlighted)
        {
            if (highlightImage != null)
                highlightImage.enabled = isHighlighted;
        }
    }
}
