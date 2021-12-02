using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGate : MonoBehaviour, ITargetTriggerObject {
	[SerializeField] private int playerId;
	[SerializeField] private Ball _ball;
	
	public void OnTriggerFire() {//При вылете мяча за пределы горизонтальных граней игрового поля он снова появляется в центре поля и опять начинает движение в случайном направлении.
		_ball.RespawnBall();
		if (!ManagerGame.instance.isSinglePlayer)
			ManagerScore.instance.SubScore(1);
		ManagerScore.instance.SubScore(0);
	}
}
