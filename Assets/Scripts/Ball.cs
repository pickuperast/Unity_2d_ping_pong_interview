using System.Collections;
using Photon.Pun;
using UnityEngine;
using Random = UnityEngine.Random;

public class Ball : MonoBehaviourPun, IPunObservable, ITargetColliderObject {
    [SerializeField] private float speedBase = 5f;
    [SerializeField] private ParticleSystem _fxDroplets;
    [SerializeField] private ParticleSystem _fxOrbGlow;
    [SerializeField] private ParticleSystem _fxPunch;
    [SerializeField] private AudioSource _audio;
    private float _timeToBallRespawn = 1f;
    private Rigidbody2D rigidbody;
    private PhotonView _thisPv;

    private void Awake() {
        _thisPv = GetComponent<PhotonView>();
        rigidbody = GetComponent<Rigidbody2D>();
    }
    
    public void OnEnable() => PhotonNetwork.AddCallbackTarget(this);

    private void Start() {
        OnResetPosition();//При старте игры мячик появляется в центре поля
        OnSetRandomDirection();// и начинает движение в случайном направлении
        SetHostColor();//Реализовать меню настроек в котором можно менять цвет мячика. Выбранные настройки сохраняются при перезапуске приложения.
    }

    public void OnDisable() => PhotonNetwork.RemoveCallbackTarget(this);

    private void OnTriggerEnter2D(Collider2D other) {
        ITargetTriggerObject triggerObject = other.gameObject.GetComponent<ITargetTriggerObject>();
        if (triggerObject == null)
            return;
        triggerObject.OnTriggerFire();//потенциал для масштабирования, например: powerups, coins
    }

    private void OnCollisionEnter2D(Collision2D other) => _thisPv.RPC("OnHitActions", RpcTarget.All);
    public void OnColliderHitPlatform() => ManagerScore.instance.AddScore();

    void SetHostColor() {
        if (_thisPv.IsMine)
            ChangeColor(MasterManager.Instance.BallColor[0], MasterManager.Instance.BallColor[1],MasterManager.Instance.BallColor[2]);
    }

    public void RespawnBall() => StartCoroutine(CorRespawnBall()); //При сбросе(рестарте) производить смену мячика, имеющего другие характеристики(скорость, размер и т.д.).

    IEnumerator CorRespawnBall() {
        yield return new WaitForSeconds(_timeToBallRespawn);
        OnResetPosition();
        OnSetRandomDirection();
        OnResetSpeed();
        OnSetRandomSize();
        OnChangeAppearance();
    }

    void OnResetPosition() {
        if (_thisPv.IsMine)
            transform.position = Vector3.zero;
    }

    void OnSetRandomDirection() {
        if (_thisPv.IsMine) {
            int sign = Random.Range(0, 1);
            if (sign == 0)
                ChangeDirection(Random.Range(.5f, 1f), Random.Range(.5f, 1f));
            else
                ChangeDirection(Random.Range(-1f,-.5f), Random.Range(-1f,-.5f));
        }
            
    }

    void OnResetSpeed() => _thisPv.RPC("ChangeBaseSpeed", RpcTarget.All, Random.Range(10f, 15f));
    void OnSetRandomSize() {
        float size = Random.Range(0.8f, 1.5f);
        _thisPv.RPC("ChangeSize", RpcTarget.All, size, size);
    }

    void OnChangeAppearance() => _thisPv.RPC("ChangeColor", RpcTarget.All, Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));

    #region PUN CALLBACKS
    
    [PunRPC]
    public void ChangeDirection(float x, float y) {
        Vector2 dir = new Vector2(x, y);
        rigidbody.velocity = dir.normalized * speedBase;
    }
    
    [PunRPC]
    public void ChangeColor(float x, float y, float z) {
        Color color = new Color(x, y, z);
        var main = _fxDroplets.main;
        main.startColor = color;
        main = _fxOrbGlow.main;
        main.startColor = color;
        //_sprite.color = new Color(x, y, z);
    }
    
    [PunRPC]
    public void ChangeSize(float x, float y) => transform.localScale = new Vector3(x, y, 1f);
    
    [PunRPC]
    public void ChangeBaseSpeed(float value) => speedBase = value;
    
    [PunRPC]
    public void OnHitActions() {
        _fxPunch.Play();
        _audio.Play();
    }
    
    #endregion
    
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
        if (stream.IsWriting)
            stream.SendNext(speedBase);
        else if (stream.IsReading)
            speedBase = (float) stream.ReceiveNext();
    }
}