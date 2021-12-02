using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class ManagerGame : MonoBehaviour {
    [SerializeField] private Platform[] _platforms;
    [SerializeField] private PlatformController[] platformController;
    [SerializeField] private GameObject PREFAB_PHOTON_NETWORK_PLAYER;
    public bool isSinglePlayer = true;
    public static ManagerGame instance { get; private set; }
    
    void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }

    private void Start() {
        int lenOfPlayers = PhotonNetwork.PlayerList.Length;
        platformController = new PlatformController[lenOfPlayers];

        if (lenOfPlayers == 1) {
            PlatformController hostPlatform = CreatePlayer();
            foreach (Platform platform in _platforms)
                hostPlatform.OnGivePlatform(PhotonNetwork.PlayerList[0], platform);
        }
        else {//Каждый игрок вызывая эту часть создает себе контроллер
            if (PhotonNetwork.IsMasterClient) {
                platformController[0] = CreatePlayer();
                platformController[0].OnGivePlatform(PhotonNetwork.PlayerList[0], _platforms[0]);
            }
            else {
                platformController[1] = CreatePlayer();
                platformController[1].OnGivePlatform(PhotonNetwork.PlayerList[1], _platforms[1]);
            }
        }
    }

    private PlatformController CreatePlayer() {
        GameObject platformGo = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", PREFAB_PHOTON_NETWORK_PLAYER.name), Vector3.zero, Quaternion.identity, 0);
        return platformGo.GetComponent<PlatformController>();
    }
}
