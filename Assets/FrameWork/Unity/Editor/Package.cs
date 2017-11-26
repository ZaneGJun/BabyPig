using System;
using UnityEngine;
using UnityEditor;

public class Package : Editor
{
	[MenuItem("Custom/Build Win32 Package")]
	static void PackageAssetBundle()
	{
		string str = Application.dataPath + "/Output";
		BuildPipeline.BuildAssetBundles (str, BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows);
	}
}


