using UnityEngine;

namespace Septyr.ScriptableObjectArchitecture
{
	[CreateAssetMenu(
	    fileName = "AnimationCurveCollection.asset",
	    menuName = SOArchitecture_Utility.ADVANCED_VARIABLE_COLLECTION + "AnimationCurve",
	    order = 120)]
	public class AnimationCurveCollection : Collection<AnimationCurve>
	{
	}
}