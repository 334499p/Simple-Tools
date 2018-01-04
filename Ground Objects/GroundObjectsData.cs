using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SimpleTools{
	public class GroundObjectsData : ScriptableObject {
		public float offset;
		public Vector3 rotationOffset;
		public bool randomizeRotation;
		public bool rotateTowardsNormal;
	}
}
