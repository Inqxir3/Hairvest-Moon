using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// UI representation of a tool slot that can be selected
/// </summary>
public class ToolSlot : MonoBehaviour
{
    [SerializeField] private ToolSystem.ToolType toolType;
    [SerializeField] private Image selectionHighlight;

    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(SelectTool);
    }

    private void Update()
    {
        selectionHighlight.enabled = (ToolSystem.CurrentTool == toolType);
    }

    private void SelectTool()
    {
        ToolSystem.SetTool(toolType);
    }
}