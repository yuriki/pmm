using UnityEngine;
using UnityEditor;


public class BuildiOSAppSlices : MonoBehaviour
{
	[InitializeOnLoadMethod]
	static void SetupResourcesBuild()
	{
		UnityEditor.iOS.BuildPipeline.collectResources += CollectResources;
	}

	static UnityEditor.iOS.Resource[] CollectResources()
	{
		return new UnityEditor.iOS.Resource[]
		{
			new UnityEditor.iOS.Resource("textures").BindVariant( "Assets/ODR/textures.hd", "hd" )
									 .BindVariant( "Assets/ODR/textures.sd", "sd" )
					 .AddOnDemandResourceTags( "textures" ),
		};
	}

	[MenuItem("Bundle/Build iOS App Slices")]
	static void BuildAssetBundles()
	{
		var options = BuildAssetBundleOptions.None;
		BuildPipeline.BuildAssetBundles("Assets/ODR", options, EditorUserBuildSettings.activeBuildTarget);
	}

}
