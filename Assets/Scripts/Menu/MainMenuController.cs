using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public GameObject panelSaveSlots;

    public void Play() => SceneManager.LoadScene("GameScene");
    public void OpenSave()
    {
        var ctrl = panelSaveSlots.GetComponent<SaveSlotsPanelController>();
        ctrl.panelMode = SaveSlotsPanelController.Mode.Load;
        panelSaveSlots.SetActive(true);
    }
    public void Quit() => Application.Quit();
}