using UnityEngine;
using System.IO;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Pass : MonoBehaviour {

	public GameObject passPanel;
	public GameObject forParentsButton;
	public InputField passField;
	public Text placeholder;
	string typedPass = "";
	string tmpPass = "";
	string passState = "";
	string pass_FileName = "Pass.txt";
	public StringData appPath;

	public void ShowPassPanel () 
	{	
		passPanel.SetActive (true);
		forParentsButton.SetActive (false);
		CheckPassExistance (pass_FileName);
	}


	void CheckPassExistance (string fileName)
	{

		if (!File.Exists (appPath.Data + fileName))
		{
			placeholder.text = Lean.Localization.LeanLocalization.GetTranslationText("Create new password");
			passState = "newPass";
		}
		else
		{
			placeholder.text = Lean.Localization.LeanLocalization.GetTranslationText("Enter pass");
			passState = "checkPass";
		}
	}
		

	public void ReadTypedPass (string typed)
	{
		typedPass = typed;


	}
		

	//Сравниваю введённый пароль с реальным паролем..
	public void CheckPassword () 
	{
		//cleaning passField after reading pin TODO Problem here: I cleaned but didn't show anything
		if (passField.text != "")
		{
			placeholder.text = "";
			passField.text = "";
		}


		if (typedPass != "")
		{
			switch (passState)
			{
				case "checkPass":
					string realPass = File.ReadAllText (appPath.Data + pass_FileName);
					if (typedPass == realPass) //passes the same..
					{
						placeholder.text = "OK";
						SceneManager.LoadScene (1); //..load admin Level
					}
					else
					{
						placeholder.text = Lean.Localization.LeanLocalization.GetTranslationText("Wrong pass. Try again");
					}
					typedPass = "";
					break;

				case "newPassRepeat":
					if (typedPass != tmpPass)
					{
						passState = "newPass";
						passField.text = "";
						placeholder.text = Lean.Localization.LeanLocalization.GetTranslationText("Passwords mismatched. Type new pass again");
						tmpPass = "";
					}
					else
					{
						placeholder.text = Lean.Localization.LeanLocalization.GetTranslationText("Saved");
						File.WriteAllText (appPath.Data + pass_FileName, typedPass);
						tmpPass = "";
						SceneManager.LoadScene (1);
					}
					typedPass = "";
					break;

				case "newPass":
					tmpPass = typedPass;
					passState = "newPassRepeat";
					passField.text = "";
					placeholder.text = Lean.Localization.LeanLocalization.GetTranslationText("Repeat password");
					typedPass = "";
					break;
			}
		}
	}
}


