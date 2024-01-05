using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class BalloonInflator : XRGrabInteractable
{
    [Header("Balloon Data")]
    public Transform attachPoint;
    public Balloon balloonPrefb;

    private Balloon m_BallonInstance;
    private XRBaseController m_Controller;

    public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase)
    {
        base.ProcessInteractable(updatePhase);
        if (isSelected && m_Controller != null)
        {
            m_BallonInstance.transform.localScale =
                Vector3.one * Mathf.Lerp(1f, 4f, m_Controller.activateInteractionState.value);
        }
    }

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);

        //Creo un nuovo palloncino
        m_BallonInstance = Instantiate(balloonPrefb, attachPoint);
        var controllerInteractor = args.interactorObject as XRBaseControllerInteractor;
        m_Controller = controllerInteractor.xrController;
        m_Controller.SendHapticImpulse(1, 0.5f);
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);

        //Distrugge il palloncino creato
        Destroy(m_BallonInstance);
    }
}
