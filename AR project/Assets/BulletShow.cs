using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BulletShow : MonoBehaviour
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

		if (player.currentAmmo < index)
		{
			img.enabled = false;
		}
		else
        {
			img.enabled = true;
        }
	}
}
