using System;
using UnityEngine;

namespace Menu
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Cup Selection Assets", order = 1)]
    public class CupSelectionAssets : ScriptableObject
    {
        public int cupID;
        [SerializeField] private string cupName;
        /*public Sprite[] imageSpeedway = new Sprite[4];
        public string[] speedwayNames = new string[4];*/
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