using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace SimpleTools {
	[CustomEditor (typeof(DevelopmentTracker))]
		public class SimpleTools_DevelopmentTracker : Editor {


		[MenuItem( "Simple Tools/Development Tracker", false, 1001 )]
		public static void createAsset(){
			GameObject[] objects = new GameObject[1];
			Object obj = AssetDatabase.LoadMainAssetAtPath ("Assets/Simple Tools/Development Tracker/DevelopmentAsset.asset");
			Selection.activeObject = obj;

		}

		int selectedHistory = -1;
		public override void OnInspectorGUI(){
			
			DevelopmentTracker devTracker = (DevelopmentTracker)target;
			EditorGUILayout.Space ();
			EditorGUILayout.LabelField ("Development Tracker");
			EditorGUILayout.Space ();

			EditorGUILayout.LabelField ("Version");
			devTracker.version = EditorGUILayout.TextField (devTracker.version);

			EditorGUILayout.Space ();
			EditorGUILayout.LabelField ("Build Number");
			EditorGUILayout.BeginHorizontal ();
			devTracker.build = EditorGUILayout.IntField (devTracker.build);
			if (GUILayout.Button ("Iterate")) {
				devTracker.build++;
			}

			EditorGUILayout.EndHorizontal ();

			EditorGUILayout.Space ();
			EditorGUILayout.LabelField ("Description");
			devTracker.description = EditorGUILayout.TextField (devTracker.description);

			EditorGUILayout.Space ();
			GUI.color = Color.green;
			if (GUILayout.Button ("Finalize Build")) {
				devTracker.history.Add(System.DateTime.Now.ToString() + "\n" + devTracker.version + "\nbuild " + devTracker.build.ToString() + "\n" + devTracker.description); 
				EditorUtility.SetDirty ((DevelopmentTracker)target);
			}
			GUI.color = Color.white;
			EditorGUILayout.Space ();
			for (int i = devTracker.history.Count -1; i >= 0; i--) {
				if (GUILayout.Button ("View Build " + i.ToString())) {
					if (selectedHistory != i) {
						selectedHistory = i;
					} else {
						selectedHistory = -1;
					}
				}
				if (selectedHistory == i) {
					string[] info = devTracker.history [i].Split ("\n" [0]);
					foreach (string ii in info) {
						EditorGUILayout.LabelField (ii);
					}

				}
			}

			GUI.color = Color.red;
			GUILayout.Space (30);
			if (GUILayout.Button ("Delete All Builds")) {
				devTracker.history.Clear ();
			}
		}
	}
}
