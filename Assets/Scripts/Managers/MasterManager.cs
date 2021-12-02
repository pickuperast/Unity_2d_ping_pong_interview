using System;
using Packages.Rider.Editor.UnitTesting;
using UnityEngine;
[CreateAssetMenu(menuName = "Singletons/MasterManager")]
public class MasterManager : SingletonScriptableObject<MasterManager> {
    public string NAME_BEST_SCORE = "BestScore";
    public string[] COLOR_NAMES = new string[]{"BallColorR", "BallColorG", "BallColorB"};
    public float[] BallColor = new float[] {1f,1f,1f};
    public int PLAYER_MAX_LIVES = 3;
    public string PLAYER_LIVES = "PlayerLives";
    public string PLAYER_READY = "IsPlayerReady";
    public string PLAYER_LOADED_LEVEL = "PlayerLoadedLevel";
    public int BestScore;


    public Color GetColor(int colorChoice) {
        switch (colorChoice) {
            case 0: return Color.red;
            case 1: return Color.green;
            case 2: return Color.blue;
            case 3: return Color.yellow;
            case 4: return Color.cyan;
            case 5: return Color.grey;
            case 6: return Color.magenta;
            case 7: return Color.white;
        }

        return Color.black;
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void FirstInitialize() {
        Debug.Log("MasterManager Initialisation");
    }
}