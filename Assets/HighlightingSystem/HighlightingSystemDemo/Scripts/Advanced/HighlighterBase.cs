using UnityEngine;
using System.Collections;
using HighlightingSystem;

public class HighlighterBase : MonoBehaviour
{
	public bool seeThrough = true;
	
	protected Highlighter highlighter;
	
	#region MonoBehaviour
	// 
	protected virtual void Awake()
	{
        highlighter = GetComponent<Highlighter>();
		if (highlighter == null)
        {
            highlighter = gameObject.AddComponent<Highlighter>();
           
        }
	}
	
	// 
	protected virtual void OnEnable()
	{
        highlighter.seeThrough = seeThrough;
	}
	
	// 
	protected virtual void Start()
	{
		
	}
	
	// 
	protected virtual void Update()
	{

	}
	
	// 
	protected virtual void OnValidate()
	{
		if (highlighter != null)
		{
            highlighter.seeThrough = seeThrough;
		}
	}
	#endregion
}