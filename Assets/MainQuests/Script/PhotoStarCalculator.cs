using UnityEngine;

public static class PhotoStarCalculator
{
    public static int CalculateStars(int score, StarThreshold thresholds)
    {
        if (score >= thresholds.scoreFor3Stars) return 3;
        if (score >= thresholds.scoreFor2Stars) return 2;
        if (score >= thresholds.scoreFor1Star) return 1;
        return 0;
    }
}
