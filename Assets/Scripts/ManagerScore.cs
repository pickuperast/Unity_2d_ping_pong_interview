using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class ManagerScore : MonoBehaviour {
    [SerializeField] private Text _textScore;
    [SerializeField] private Text _textBestScore;
    private int _currentScore;
    private const int DEFAULT_SCORE_STEP = 5;
    public static ManagerScore instance { get; private set; }
    
    void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }

    void Start() => OnBestScoreTextUpdate(MasterManager.Instance.BestScore);
    

    public void AddScore() {
        _currentScore += DEFAULT_SCORE_STEP;
        _textScore.text = "Score: " + _currentScore;
        if (_currentScore > MasterManager.Instance.BestScore) {
            SaveLoadManager.instance.SetBestScore(_currentScore);
            OnBestScoreTextUpdate(_currentScore);
        }
    }

    public void SubScore(int playerId) {
        if (playerId == 0)
            if (PhotonNetwork.IsMasterClient) {
                _currentScore -= DEFAULT_SCORE_STEP;
                _textScore.text = "Score: " + _currentScore;
            }
        else
            if (!PhotonNetwork.IsMasterClient) {
                _currentScore -= DEFAULT_SCORE_STEP;
                _textScore.text = "Score: " + _currentScore;
            }  
    }

    void OnBestScoreTextUpdate(int value) => _textBestScore.text = "Best Score: " + value;
}
