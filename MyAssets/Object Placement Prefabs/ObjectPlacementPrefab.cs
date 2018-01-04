using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleTools {
	public class ObjectPlacementPrefab : ScriptableObject {
		public GameObject prefab;
		[HideInInspector]
		[Range(.5f,10)]
		public float toolRadius;
		[HideInInspector]
		[Range(0,1)]
		public float placementDensity;
		[Range(0,100)]
		[HideInInspector]
		public float placementAddition;
		[HideInInspector]
		public float normalOffset;
		[HideInInspector]
		public Vector3 rotationOffset;
		[HideInInspector]
		public bool randomizeNormalAxisRotation;
	}
}
