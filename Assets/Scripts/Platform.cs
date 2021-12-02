using Photon.Pun;
using UnityEngine;

public class Platform : MonoBehaviour {
    private PlatformController _owner;
    private PhotonView _thisPv;
    public PhotonView PV => _thisPv;
    private Ball _ball;
    
    void Awake() {
        _thisPv = gameObject.GetPhotonView();
        _ball = GameObject.FindWithTag("Ball").GetComponent<Ball>();
    }

    private void OnCollisionEnter2D(Collision2D other) {
        ITargetColliderObject targetObject = other.gameObject.GetComponent<ITargetColliderObject>();
        if (targetObject == null)
            return;
        targetObject.OnColliderHitPlatform();//потенциал для масштабирования, например: дополнительные цели для отбивания
    }
    
    public void SetOwnerTo(PlatformController newOwner) {
        _owner = newOwner;
        _thisPv.TransferOwnership(newOwner.PV.Controller);
    }
}