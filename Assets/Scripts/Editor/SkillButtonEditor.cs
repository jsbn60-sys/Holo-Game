using UnityEditor;
using UnityEditor.UI;
using UnityEngine.Rendering;


/// <summary>
/// This custom editor script is a workaround to make
/// the SerializeField skill of the class SkillButton appear in the editor,
/// so that the skills can be inserted there. The field does not appear on its
/// own because the button class is a unity lib class and has a custom editor
/// so that new SerializeFields won't appear.
/// PROBLEM: This causes many other fields of the
/// button class to appear, which usually were supposed to be hidden.
/// </summary>
[CustomEditor(typeof(SkillButton))]
public class MenuButtonEditor : ButtonEditor
{
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();
	}
}


