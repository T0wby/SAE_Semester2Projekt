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
	private List<Tabs> _allCategories;
	private List<string> _allCategorylabels;
	private Tabs _currentTab;

    public Tabs CurrentTab { get => _currentTab; }


    public TabDrawer()
	{
		InitTabs();

    }


    public void DrawTabs()
	{
		int index = (int)_currentTab;
		index = GUILayout.Toolbar(index, _allCategorylabels.ToArray());
        _currentTab = _allCategories[index];
    }

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
}
