using UnityEngine;

[CreateAssetMenu(menuName = "Farming/Crop Data")]
public class CropData : ScriptableObject
{
    public string cropName;
    public Sprite[] growthStages;
    public int growthDuration;
}

