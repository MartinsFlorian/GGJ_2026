using UnityEngine;

public class S_CursorManager : MonoBehaviour
{
    //[Header("Settings")]

    //[Header("References")]

    [Header("Inputs")]
    [SerializeField] private RSE_OnShowCursor rse_OnShowCursor;

    //[Header("Outputs")]
    private void OnEnable()
    {
        rse_OnShowCursor.action += LockCursor;
    }
    private void OnDisable()
    {
        rse_OnShowCursor.action -= LockCursor;
    }
    private void Start()
    {
        LockCursor(true);
    }
    private void LockCursor(bool locked)
    {
        Cursor.lockState = locked ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !locked;
    }
}