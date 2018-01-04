using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleTools;

public class AssetUsageExample : MonoBehaviour {
	public WeaponAsset[] weapons;

	void Start(){
		foreach (WeaponAsset weapon in weapons) {
			Debug.Log (weapon.name + " - " + weapon.damage + " damage");
		}
	}
}
