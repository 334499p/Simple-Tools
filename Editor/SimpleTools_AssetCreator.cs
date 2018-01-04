using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
namespace SimpleTools{
	public class SimpleTools_AssetCreator : EditorWindow {

		// Use this for initialization
		void Start () {
			
		}

		[MenuItem( "Simple Tools/Asset Creator", false, 1001 )]
		public static void createAsset(){
			var window = EditorWindow.GetWindow<SimpleTools_AssetCreator>();
			window.titleContent = new GUIContent("Asset Creator");
		}

		string newAssetName = "New Asset";
		string newAssetScript = "CustomAsset";
		bool viewingInformation;

		void OnGUI(){
			GUILayout.Space (5);
			GUILayout.Label ("Create a New Asset");

			GUILayout.Space (5);
			GUILayout.Label ("Asset Name");
			newAssetName = GUILayout.TextArea (newAssetName);

			GUILayout.Space (5);
			GUILayout.Label ("Asset Script");
			newAssetScript = GUILayout.TextArea (newAssetScript);

			GUILayout.Space (10);
			if (GUILayout.Button ("Create")) {
				CreateAsset (newAssetName, "Assets/Simple Tools/MyAssets/"+newAssetName+".asset");
			}

			GUILayout.Space (10);
			if (GUILayout.Button ("View Instructions")) {
				viewingInformation = !viewingInformation;
			}
			if (viewingInformation) {
				GUILayout.Label ("Enter the name of the asset under Asset Name.");
				GUILayout.Label ("");
				GUILayout.Label ("Enter the name of the script from which the new asset will be derived.");
				GUILayout.Label ("You must create that script (use CustomAsset.cs for reference).");
				GUILayout.Label ("");
				GUILayout.Label ("Pressing 'Create' will create a new asset under MyAssets/");

			}
		}

		public Object CreateAsset(string name, string assetFile){
			Object obj = ScriptableObject.CreateInstance(newAssetScript);
			obj.name = name;
			AssetDatabase.CreateAsset(obj, assetFile);
			return obj;
		}
	}
}
