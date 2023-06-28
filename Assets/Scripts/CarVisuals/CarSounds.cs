using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Car Sounds", order = 5)]
public class CarSounds : ScriptableObject
{
	[Header("CAR AUDIO CLIPS")]
	public AudioClip carEngineClip;
	public AudioClip carImpactClip;
}
