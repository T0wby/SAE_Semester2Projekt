using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Tabs
{
	Preset,
	CurrentTree,
	Settings
}

public class TabDrawer
{
	#region Fields
	private List<Tabs> _allCategories;
	private List<string> _allCategorylabels;
	private Tabs _currentTab;
	#endregion

	#region Properties
	public Tabs CurrentTab { get => _currentTab; }
	#endregion


	#region Constructor
	public TabDrawer()
	{
		InitTabs();
	}
    #endregion


    #region Methods

    /// <summary>
    /// Creates Toolbar with all Elements from Tabs
    /// </summary>
    public void DrawTabs()
	{
		int index = (int)_currentTab;
		index = GUILayout.Toolbar(index, _allCategorylabels.ToArray());
		_currentTab = _allCategories[index];
	}

	/// <summary>
	/// Converts all Tabs Elements to a tabs list and then convert that into strings
	/// </summary>
	private void InitTabs()
	{
		_allCategories = new List<Tabs>();

		Tabs[] enums = (Tabs[])System.Enum.GetValues(typeof(Tabs));
		for (int i = 0; i < enums.Length; i++)
		{
			_allCategories.Add(enums[i]);
		}

		_allCategorylabels = _allCategories.ConvertAll(x => x.ToString());

	} 
	#endregion
}
