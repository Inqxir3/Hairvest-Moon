using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// A UI button representing a tool in the hotbar.
/// </summary>
public class ToolSlot : MonoBehaviour
{
    [SerializeField] private ToolSystem.ToolType tool;
    [SerializeField] private Image highlightImage;

    public ToolSystem.ToolType Tool => tool;

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(() =>
        {
            ToolSelector.Instance?.SelectToolExternally(tool);
            DebugUIOverlay.Instance.ShowLastAction($"Tool: {tool}");
        });
    }

    public void SetHighlighted(bool isHighlighted)
    {
        if (highlightImage != null)
            highlightImage.enabled = isHighlighted;
    }
}
