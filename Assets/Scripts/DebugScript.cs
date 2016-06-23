using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class DebugScript : MonoBehaviour {

	public Text debugText;
	public int maxMessages = 20;
	static DebugScript instance;
	static List<string> lines;

	void Awake ()
	{
		instance = this;
		lines = new List<string>();
	}

	public static void Log (string msg)
	{
		if (instance == null)
			return;
		else if (!instance.gameObject.activeSelf)
			return;

		// add latest message to first line
		lines.Insert(0, msg);

		// remove older messages
		while (lines.Count > instance.maxMessages)
			lines.RemoveAt(lines.Count-1);

		// print messages
		string fullStr = "";
		for (int i = 0; i < lines.Count; i++)
			fullStr += lines[i] + "\n";
		instance.debugText.text = fullStr;
	}
}
