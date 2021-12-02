using System;
using System.Collections;
using System.Collections.Generic;
using root;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SaveLoadManager : MonoBehaviour {
    [SerializeField] private Text _textBestScore;
    [SerializeField] private Slider[] _sliders;
    public static SaveLoadManager instance { get; private set; }
    
    void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }
    private void Start() {
        Scene scene = SceneManager.GetActiveScene();

        // Check if the name of the current Active Scene is your first Scene.
        if (scene.name == "BattleScene") {
            _textBestScore = GameObject.FindWithTag("TextBestScore").GetComponent<Text>();
        }
        LoadBestScore();
        LoadColor();
    }

    public void SetBestScore(int newVal) {
        PlayerPrefs.SetInt(MasterManager.Instance.NAME_BEST_SCORE, newVal);
        MasterManager.Instance.BestScore = newVal;
    }

    void LoadBestScore() {
        if (PlayerPrefs.HasKey(MasterManager.Instance.NAME_BEST_SCORE)) {
            MasterManager.Instance.BestScore = PlayerPrefs.GetInt(MasterManager.Instance.NAME_BEST_SCORE);
            _textBestScore.text = "Best Score: " + MasterManager.Instance.BestScore;
        }
    }
    
    public void SetColor() {
        for (int i = 0; i < _sliders.Length; i++)
            PlayerPrefs.SetFloat(MasterManager.Instance.COLOR_NAMES[i], _sliders[i].normalizedValue);
    }

    void LoadColor() {
        if (PlayerPrefs.HasKey(MasterManager.Instance.COLOR_NAMES[0]))
            for (int i = 0; i < _sliders.Length; i++) {
                float val = PlayerPrefs.GetFloat(MasterManager.Instance.COLOR_NAMES[i]);
                MasterManager.Instance.BallColor[i] = val;
                _sliders[i].value = val;
            }
    }
}
