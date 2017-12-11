using UnityEngine;
using System.Collections;

public class HighlighterConstant : HighlighterInteractive
{
	public Color color = Color.cyan;

	#region MonoBehaviour
	// 
	protected override void OnEnable()
	{
		base.OnEnable();
        highlighter.ConstantOnImmediate(color);
	}

	// 
	protected override void OnValidate()
	{
		base.OnValidate();
		if (highlighter != null)
		{
            highlighter.ConstantOnImmediate(color);
		}
	}
	#endregion
}