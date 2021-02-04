using UnityEngine;
using System.Collections;

public class GeneralsInfoController : MonoBehaviour {
	
	public IAController IACtrl;
	
	public ListController 	generalsList;
	
	public GameObject		information;
	public GIInformation 	generalInfo;
	public GIFormation 		formation;
	public GIArms 			arms;
	public GIMagic 			magic;
	public GIEquipment 		equipment;
	
	public MenuDisplayAnim 	arrow;
	public ImageButton 		leftArrow;
	public ImageButton 		rightArrow;
	
	private int state = 0;
	private int selectIdx = 0;
	private int selectGeneralIdx = 98;
	
	private float timeTick;
	
	// Use this for initialization
	void Start () {
		generalsList.SetSelectItemHandler(OnSelectGeneral);
		
		leftArrow.SetButtonClickHandler(OnLeftArrowClickHandler);
		rightArrow.SetButtonClickHandler(OnRightArrowClickHandler);
	}
	
	void OnEnable() {
		state = 0;
		
		information.SetActive(false);
		generalsList.gameObject.SetActive(true);
		generalsList.gameObject.GetComponent<MenuDisplayAnim>().SetAnim(MenuDisplayAnim.AnimType.InsertFromLeft);
		
		AddGeneralsList();
	}
	
	void OnDisable() {
		generalsList.Clear();
	}
	
	// Update is called once per frame
	void Update () {
		switch (state) {
		case 0:
			OnSelectGeneralModeHandler();
			break;
		case 1:
			OnGeneralInformationModeHandler();
			break;
		case 2:
			OnChangeToInformationModeHandler();
			break;
		case 3:
			OnChangeToGeneralsListModeHandler();
			break;
		case 4:
			OnReturnMainHandler();
			break;
		}
	}
	
	void OnSelectGeneralModeHandler() {
		if (Misc.GetBack()) {
			state = 4;
			generalsList.gameObject.GetComponent<MenuDisplayAnim>().SetAnim(MenuDisplayAnim.AnimType.OutToLeft);
			return;
		}
	}
	
	void OnGeneralInformationModeHandler() {
		if (Misc.GetBack()) {
			state = 3;
			
			generalInfo	.gameObject.GetComponent<MenuDisplayAnim>().SetAnim(MenuDisplayAnim.AnimType.OutToLeft);
			formation	.gameObject.GetComponent<MenuDisplayAnim>().SetAnim(MenuDisplayAnim.AnimType.OutToTop);
			arms		.gameObject.GetComponent<MenuDisplayAnim>().SetAnim(MenuDisplayAnim.AnimType.OutToRight);
			magic		.gameObject.GetComponent<MenuDisplayAnim>().SetAnim(MenuDisplayAnim.AnimType.OutToBottom);
			equipment	.gameObject.GetComponent<MenuDisplayAnim>().SetAnim(MenuDisplayAnim.AnimType.OutToRight);
			arrow.SetAnim(MenuDisplayAnim.AnimType.OutToLeft);
			
			return;
		}
	}
	
	void OnChangeToInformationModeHandler() {
		timeTick += Time.deltaTime;
		if (timeTick >= 0.2f) {
			timeTick = 0;
			state = 1;
			
			generalsList.gameObject.SetActive(false);
			information.SetActive(true);
			
			generalInfo	.gameObject.GetComponent<MenuDisplayAnim>().SetAnim(MenuDisplayAnim.AnimType.InsertFromLeft);
			formation	.gameObject.GetComponent<MenuDisplayAnim>().SetAnim(MenuDisplayAnim.AnimType.InsertFromTop);
			arms		.gameObject.GetComponent<MenuDisplayAnim>().SetAnim(MenuDisplayAnim.AnimType.InsertFromRight);
			magic		.gameObject.GetComponent<MenuDisplayAnim>().SetAnim(MenuDisplayAnim.AnimType.InsertFromBottom);
			equipment	.gameObject.GetComponent<MenuDisplayAnim>().SetAnim(MenuDisplayAnim.AnimType.InsertFromRight);
			arrow.SetAnim(MenuDisplayAnim.AnimType.InsertFromLeft);
		}
	}
	
	void OnChangeToGeneralsListModeHandler() {
		timeTick += Time.deltaTime;
		if (timeTick >= 0.2f) {
			timeTick = 0;
			state = 0;
			
			generalsList.gameObject.SetActive(true);
			generalsList.gameObject.GetComponent<MenuDisplayAnim>().SetAnim(MenuDisplayAnim.AnimType.InsertFromLeft);
			
			information.SetActive(false);
		}
	}
	
	void OnReturnMainHandler() {
		timeTick += Time.deltaTime;
		if (timeTick >= 0.2f) {
			timeTick = 0;
			gameObject.SetActive(false);
			
			IACtrl.OnReturnMain();
		}
	}
	
	void OnSelectGeneral() {
		if (state != 0) return;
		
		state = 2;
				
		selectIdx = generalsList.GetSelectIndex();
		selectGeneralIdx = (int)generalsList.GetSelectItem().GetItemData();
		
		generalsList.gameObject.GetComponent<MenuDisplayAnim>().SetAnim(MenuDisplayAnim.AnimType.OutToLeft);
		
		OnChangeGeneralInfo();
	}
	
	void OnLeftArrowClickHandler() {
		if (selectIdx > 0) {
			selectIdx--;
			selectGeneralIdx = (int)generalsList.GetListItem(selectIdx).GetItemData();
			
			OnChangeGeneralInfo();
		}
	}
	
	void OnRightArrowClickHandler() {
		if (selectIdx < generalsList.GetCount() - 1) {
			selectIdx++;
			selectGeneralIdx = (int)generalsList.GetListItem(selectIdx).GetItemData();
			
			OnChangeGeneralInfo();
		}
	}
	
	void AddGeneralsList() {
		
		KingInfo kInfo = Informations.Instance.GetKingInfo(Controller.kingIndex);
		
		int kingIdx = kInfo.generalIdx;
		string str = ZhongWen.Instance.GetGeneralName1(kingIdx);
		ListItem li = generalsList.AddItem(str);
		li.SetItemData(kingIdx);
		
		for (int i=0; i<kInfo.generals.Count; i++) {
			
			int gIdx = (int)kInfo.generals[i];
			
			if (gIdx != kingIdx) {
				li = generalsList.AddItem(ZhongWen.Instance.GetGeneralName1(gIdx));
				li.SetItemData(gIdx);
			}
		}
	}
	
	void OnChangeGeneralInfo() {
		generalInfo	.SetGeneral(selectGeneralIdx);
		formation	.SetGeneral(selectGeneralIdx);
		arms		.SetGeneral(selectGeneralIdx);
		magic		.SetGeneral(selectGeneralIdx);
		equipment	.SetGeneral(selectGeneralIdx);
	}
}
