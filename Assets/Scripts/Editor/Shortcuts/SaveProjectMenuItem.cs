using UnityEditor;

public class SaveProjectMenuItem
{
	[MenuItem("Window/Custom/SaveEverything %#&s")]
	public static void Save()
	{
		EditorApplication.ExecuteMenuItem("File/Save Scene");
		EditorApplication.ExecuteMenuItem("File/Save Project");
	}
}
