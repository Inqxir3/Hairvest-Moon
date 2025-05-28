using UnityEngine;

/// <summary>
/// Core tool system definitions and current tool tracking
/// </summary>
public static class ToolSystem
{
    public enum ToolType
    {
        None,
        Hoe,
        WateringCan,
        Seed,
        Harvest
    }

    public static ToolType CurrentTool { get; private set; } = ToolType.None;

    public static void SetTool(ToolType tool)
    {
        CurrentTool = tool;
        DebugUIOverlay.Instance.ShowLastAction($"Tool: {CurrentTool}");
    }
}