using UnityEngine;
using System.Collections;

public class PowerUp : MonoBehaviour {

	public enum PU
	{
		pointNormal,
		pointArea,
		invert,
		crossClear
	}

	public PU powerUpType;

	public int quantity;

	public Sprite onSprite;
	public Sprite offSprite;
	// what type of tile this powerup places
	public GameObject tile;

	private bool active;

	public PowerUpManager pum;

	void Update()
	{
		if (powerUpType == PU.pointNormal)
			quantity = ScoreManager.instance.PU_PointNormal;
		else if (powerUpType == PU.pointArea)
			quantity = ScoreManager.instance.PU_PointArea;
		else if (powerUpType == PU.invert)
			quantity = ScoreManager.instance.PU_Invert;
		else if (powerUpType == PU.crossClear)
			quantity = ScoreManager.instance.PU_CrossClear;

		if (quantity <= 0)
		{
			GetComponent<SpriteRenderer>().color = new Color(0.9f, 0.7f, 0.7f);
		}
	}

	public void decrementQuantity()
	{
		if (powerUpType == PU.pointNormal)
			ScoreManager.instance.PU_PointNormal --;
		else if (powerUpType == PU.pointArea)
			ScoreManager.instance.PU_PointArea --;
		else if (powerUpType == PU.invert)
			ScoreManager.instance.PU_Invert --;
		else if (powerUpType == PU.crossClear)
			ScoreManager.instance.PU_CrossClear --;
	}

	public void setActive(bool act)
	{
		active = act;
		this.GetComponent<SpriteRenderer>().sprite = act ? onSprite : offSprite;
	}

	public bool isActive()
	{return active;}

	void OnMouseDown()
	{
		if (quantity > 0)
		{
			if (!isActive())
			{
				setActive(true);
				pum.ClearOtherActivePowerUps (this);
			}
			else
			{
				setActive (false);
			}
		}
	}
}
