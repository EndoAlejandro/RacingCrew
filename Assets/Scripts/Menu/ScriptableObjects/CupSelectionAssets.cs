using System;
using UnityEngine;

namespace Menu.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Cup Selection Assets", order = 1)]
    public class CupSelectionAssets : ScriptableObject
    {
        [SerializeField] private string cupName;
        [SerializeField] private TrackData[] tracksData;
        public TrackData[] TracksData => tracksData;
        public string CupName => cupName;
    }

    [Serializable]
    public struct TrackData
    {
        public string sceneName;
        public string displayName;
        public Sprite trackSprite;
    }
}