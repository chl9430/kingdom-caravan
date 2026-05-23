using UnityEngine;

public class PlayerExperience : MonoBehaviour
{
    [Header("Level")]
    public int currentLevel = 1;

    [Header("Experience")]
    public int currentExp = 0;
    public int expToNextLevel = 5;

    public void AddExperience(int amount)
    {
        currentExp += amount;

        Debug.Log(
            "EXP: " +
            currentExp +
            " / " +
            expToNextLevel
        );

        CheckLevelUp();
    }

    private void CheckLevelUp()
    {
        if (currentExp >= expToNextLevel)
        {
            LevelUp();
        }
    }

    private void LevelUp()
    {
        currentLevel++;

        currentExp -= expToNextLevel;

        expToNextLevel += 5;

        Debug.Log(
            "LEVEL UP! Current Level: " +
            currentLevel
        );
    }
}
