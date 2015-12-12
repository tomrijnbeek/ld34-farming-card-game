using UnityEngine;
using System.Collections;

public class GrowthRateInfluence {

    public float amount;
    public string description;
    public System.Func<float> getDuration;

    public string GetString() {
        var displayPercentage = Mathf.RoundToInt(amount * 100);
        return description + ": " + (amount >= 0 ? "+" : "") + displayPercentage + "%" +
            (getDuration != null ? string.Format(" ({0} turns)", getDuration()) : "");
    }
}
