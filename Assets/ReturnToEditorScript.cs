using UnityEngine;
using System.Collections;

public class ReturnToEditorScript : MonoBehaviour 
{
	public delegate void CallbackFunction();
	private CallbackFunction m_callBack;

	public void SetCallback(CallbackFunction function)
	{
		m_callBack = function;	
	}
	public void Update()
	{
		if(Input.GetKeyDown(KeyCode.F5))
		{
			m_callBack();
		}		
	}
}
