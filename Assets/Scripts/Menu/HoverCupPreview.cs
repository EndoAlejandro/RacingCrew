using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using InGame;

namespace Menu
{
    public class HoverCupPreview : MonoBehaviour, IPointerEnterHandler, ISelectHandler
    {
        [Header("Assets")]
        [SerializeField] private CupSelectionAssets cupSelectionAssets;

        [Space(10)]
        [Header("User Interface")]
        [SerializeField] private Image[] racetracksImage = new Image[4];


        private Button _cupButton;
        private TextMeshProUGUI _buttonText;
        private TextMeshProUGUI _cupNameText;


        private void Awake()
        {
            _cupNameText = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            _buttonText = gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            _buttonText.text = cupSelectionAssets.CupName;

            _cupButton = GetComponent<Button>();

            UpdateCupInfoInScreen();
        }

        //En la selección de copas, actualiza la información de acuerdo a la copa seleccionada
        public void UpdateCupInfoInScreen()
        {
            _cupNameText.text = cupSelectionAssets.CupName;

            for (int i = 0; i < racetracksImage.Length; i++)
            {
                racetracksImage[i].sprite = cupSelectionAssets.TracksData[i].trackSprite;
            }
        }

        //Guardamos la información de la copa seleccionada
        public void SaveSelectedCupInMemory()
        {
            // GameManager.Instance.CurrentCup = cupSelectionAssets.cupID;
            GameManager.Instance.SetCurrentCup(cupSelectionAssets);
        }

        //Implementación de interfaces
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