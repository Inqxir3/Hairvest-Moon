using UnityEngine;
using UnityEngine.UI;
using TMPro;
using HairvestMoon.Tool;

namespace HairvestMoon.UI
{
    /// <summary>
    /// Displays water can capacity using both text and slider for real-time feedback.
    /// </summary>
    public class WaterCanUI : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private TextMeshProUGUI waterAmountText;
        [SerializeField] private Slider waterAmountSlider;

        [Header("Water System Reference")]
        [SerializeField] private ToolSystem toolSystem;

        private float maxWater => 100f;  // Or pull from ToolSystem later if this becomes dynamic

        private void Update()
        {
            float currentWater = toolSystem.waterCanCapacity;

            // Update text
            //waterAmountText.text = $"Water: {currentWater:0}";

            // Update slider normalized
            waterAmountSlider.value = Mathf.Clamp01(currentWater / maxWater);
        }
    }
}