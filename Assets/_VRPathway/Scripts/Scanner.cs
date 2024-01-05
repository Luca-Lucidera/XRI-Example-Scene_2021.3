using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using TMPro;
using System;

public class Scanner : XRGrabInteractable
{
    [Header("Scanner Data")]
    public Animator animator;
    public LineRenderer laserRenderer;
    public TextMeshProUGUI targetName;
    public TextMeshProUGUI targetPosition;
    private bool firstSelect = true;
    protected override void Awake()
    {
        base.Awake();
        ScannerActivated(false);
    }

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);
        animator.SetBool("Opened", true);
        if (firstSelect)
        {
            targetName.gameObject.SetActive(true);
            targetPosition.gameObject.SetActive(true);
            targetName.SetText("Ready to scan");
            targetPosition.SetText("Ready to scan");
            firstSelect = false;
        }
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);
        animator.SetBool("Opened", false);
    }

    public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase)
    {
        base.ProcessInteractable(updatePhase);
        if (laserRenderer.gameObject.activeSelf)
            ScanForObjects();
    }

    protected override void OnActivated(ActivateEventArgs args)
    {
        base.OnActivated(args);
        ScannerActivated(true);
    }

    protected override void OnDeactivated(DeactivateEventArgs args)
    {
        base.OnDeactivated(args);
        ScannerActivated(false);
    }

    //INTERNAL 
    private void ScannerActivated(bool isActive)
    {
        laserRenderer.gameObject.SetActive(isActive);
        targetName.gameObject.SetActive(isActive);
        targetPosition.gameObject.SetActive(isActive);
    }
    private void ScanForObjects()
    {
        Vector3 laserPosition = laserRenderer.transform.position;
        Vector3 laserDirection = laserRenderer.transform.forward;
        Vector3 worldHit = laserPosition + laserDirection * 1000.0f;

        if (Physics.Raycast(laserPosition, laserDirection, out RaycastHit hit))
        {
            //Se il lase collide allora imposto che la lunghezza del lase deve essere massimo quella
            worldHit = hit.point;
            targetName.SetText(hit.collider.name);
            targetPosition.SetText(hit.transform.position.ToString());
        }

        //Imposta la lunghezza del laser molto lunga
        laserRenderer.SetPosition(1, laserRenderer.transform.InverseTransformPoint(worldHit));
    }
}
