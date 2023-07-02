using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using Menu.ScriptableObjects;

namespace Menu
{
    public class HoverCupPreview : MonoBehaviour, IPointerEnterHandler, ISelectHandler
    {
        [SerializeField] private CupSelectionAssets cupSelectionAssets;
        [SerializeField] private Image[] racetracksImage = new Image[4];

        private TMP_Text[] racetracksName = new TMP_Text[4];
        private Button _cupButton;
        private TextMeshProUGUI _buttonText;
        private TextMeshProUGUI _cupNameText;

        private void Awake()
        {
            _cupNameText = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            _buttonText = gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            _buttonText.text = cupSelectionAssets.CupName;

            _cupButton = GetComponent<Button>();

            for (int i = 0; i < racetracksImage.Length; i++)
            {
                racetracksName[i] = racetracksImage[i].GetComponentInChildren<TMP_Text>();
            }

            UpdateCupInfoInScreen();
        }

        private void UpdateCupInfoInScreen()
        {
            _cupNameText.text = cupSelectionAssets.CupName;

            for (int i = 0; i < racetracksImage.Length; i++)
            {
                racetracksImage[i].sprite = cupSelectionAssets.TracksData[i].trackSprite;
                racetracksName[i].text = cupSelectionAssets.TracksData[i].displayName;
            }
        }

        public void SaveSelectedCupInMemory() => GameManager.Instance.SetCurrentCup(cupSelectionAssets);

        public void OnPointerEnter(PointerEventData eventData)
        {
            _cupButton.Select();
            UpdateCupInfoInScreen();
        }

        public void OnSelect(BaseEventData eventData)
        {
            UpdateCupInfoInScreen();
            SaveSelectedCupInMemory();
        }
    }
}