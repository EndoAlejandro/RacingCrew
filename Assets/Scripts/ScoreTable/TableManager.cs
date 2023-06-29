using UnityEngine;
using CustomUtils;

public class TableManager : Singleton<TableManager>
{
	[Header("TABLE SETTINGS")]
	[SerializeField] private Transform parent;
	[SerializeField] private GameObject prefabCard;


	[ContextMenu("DO IT")]
	public void SetResults() {
		for (int i = 0;i < 8;i++) {
			GameObject card =  Instantiate(prefabCard,parent);
			card.GetComponent<SetInfoCard>().SetInfo((i+1).ToString(), "Racer " + CupManager.Instance.CupRacers[i].RacerIndex, CupManager.Instance.CupRacers[i].Score.ToString());
		}
	}
}
