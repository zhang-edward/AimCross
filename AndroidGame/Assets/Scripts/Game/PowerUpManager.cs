using UnityEngine;
using System.Collections;

public class PowerUpManager : MonoBehaviour {

	public PowerUp[] powerUps;
	public PowerUp activePowerUp;

	void Awake()
	{
		foreach (PowerUp pu in powerUps)
			pu.pum = this;
	}

	public void ClearOtherActivePowerUps(PowerUp powerUp)
	{
		activePowerUp = powerUp;
		foreach (PowerUp pu in powerUps)
		{
			if (pu != powerUp)
				pu.setActive(false);
		}
	}

	public void UsedPowerUp()
	{
		activePowerUp.decrementQuantity();
	}

	public bool CheckActivePowerUps()
	{
		foreach (PowerUp pu in powerUps)
		{
			if (pu.isActive())
				return true;
		}
		return false;
	}
}
