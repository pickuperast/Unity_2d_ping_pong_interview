using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlatformController : MonoBehaviourPun {
    private InputActions controls;
    private Vector2 _moveAxis;
    private List<Rigidbody2D> _controllablePlatformRigidbody = new List<Rigidbody2D>();
    private PhotonView _thisPhotonView;
    public PhotonView PV => _thisPhotonView;
    private float _startingY;
    [SerializeField] private float speed = 10f;
    
    void Awake() {
        _thisPhotonView = photonView;
        _startingY = transform.position.y;
    }

    void FixedUpdate() {
        _moveAxis = controls.Player.Move.ReadValue<Vector2>();
        if (_moveAxis.x > float.Epsilon || _moveAxis.x < float.Epsilon)
            foreach (Rigidbody2D rb in _controllablePlatformRigidbody)
                rb.velocity = new Vector2(_moveAxis.x * speed, _startingY);
    }
    
    private void OnEnable()
    {
        controls = new InputActions();
        controls.Enable();
    }

    public void OnGivePlatform(Player p, Platform platformToControl) {
        platformToControl.SetOwnerTo(this);
        _controllablePlatformRigidbody.Add(platformToControl.GetComponent<Rigidbody2D>());
    }
}