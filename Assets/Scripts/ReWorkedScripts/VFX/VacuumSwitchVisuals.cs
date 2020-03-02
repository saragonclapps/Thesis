using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VacuumSwitchVisuals : MonoBehaviour {

    VacuumSwitch sw;
    public Image progressBar;
    public Text text;
    public TextMeshProUGUI textTMP;

	void Start () {
        sw = GetComponent<VacuumSwitch>();
        UpdatesManager.instance.AddUpdate(UpdateType.UPDATE, Execute);
	}
	
	
	void Execute ()
    {
        var prog = sw.GetCurrentProgressPercent();
        progressBar.fillAmount = prog;
        // Deprecate because using Text Mesh Pro
        // text.text = (int)(prog *100) + "%";
        if (textTMP) textTMP.SetText((int)(prog *100) + "%");
    }

    private void OnDestroy()
    {
        UpdatesManager.instance.RemoveUpdate(UpdateType.UPDATE, Execute);
    }
}
