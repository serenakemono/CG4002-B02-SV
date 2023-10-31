using UnityEngine;
using UnityEngine.UI;

public class GrenadeShow : MonoBehaviour
{
	public int index;
	public Player player;
	public Image img;

	void Start()
	{

		img.enabled = true;
	}

	void Update()
	{

		if (player.currentGrenade < index)
		{
			img.enabled = false;
		}
		else
		{
			img.enabled = true;
		}
	}
}
