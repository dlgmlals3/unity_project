using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;

public class MyNetworkPlayer : NetworkBehaviour
{
	[SerializeField] private TMP_Text displayNameText = null;
	[SerializeField] private Renderer displayColorRenderer = null;

	[SyncVar(hook = nameof(HandleDisplayName))]
	[SerializeField]
	private string displayName = "Missing Name";

	[SyncVar(hook = nameof(HandleDisplayColorUpdated))]
	[SerializeField]
	private Color displayColor = Color.black;

	#region Server
	[Server]
	public void SetDisplayName(string newDisplayName)
	{
		displayName = newDisplayName;
	}
	public void SetDisplayColor(Color color)
	{
		displayColor = color;
	}
	
	[Command] // for clients calling a method o`n the server
	private void CmdSetDisplayName(string newDisplayName)
	{
		if (newDisplayName.Length < 2 || newDisplayName.Length > 20) { return; }

		RpcLogNewName(newDisplayName);
		SetDisplayName(newDisplayName);
	}
	#endregion

	#region Client
	private void HandleDisplayColorUpdated(Color oldColor, Color newColor)
	{
		Debug.Log("HandleDisplayColorUpdated ");
		displayColorRenderer.material.SetColor("_BaseColor", newColor);
	}
	
	private void HandleDisplayName(string oldName, string newName)
	{
		displayNameText.text = newName;
	}

	[ContextMenu("Set My Name")]
	private void SetMyName()
	{
		CmdSetDisplayName("My New Name");
	}

	[ClientRpc] // For the server calling a method on clients
	private void RpcLogNewName(string newDisplayName)
	{
		Debug.Log(newDisplayName);
	}
	#endregion
}
