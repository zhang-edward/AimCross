using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ShopUI : MonoBehaviour {

	// points is the in-game currency
	public int pointsIncrementer;
	public Text points;

	// texts that display the amounts of each power up
	public Text pointNormal;
	public Text pointArea;
	public Text invert;
	public Text crossClearNormal;

	public ScoreManager sm;

	void Start()
	{
		sm = ScoreManager.instance;
		pointsIncrementer = sm.Points;
	}

	void Update()
	{
		if (pointsIncrementer > sm.Points)
			pointsIncrementer --;
		points.text = "-" + pointsIncrementer.ToString() + "-";

		pointNormal.text = sm.PU_PointNormal.ToString();
		pointArea.text = sm.PU_PointArea.ToString();
		invert.text = sm.PU_Invert.ToString();
		crossClearNormal.text = sm.PU_CrossClear.ToString();
	}

	public void Back()
	{
		Application.LoadLevel ("MainMenu");
	}

	public void PointNormal()
	{sm.BuyPointNormal();}

	public void PointArea()
	{sm.BuyPointArea();}

	public void Invert()
	{sm.BuyInvert();}

	public void CrossClear()
	{sm.BuyCrossClear();}
}
