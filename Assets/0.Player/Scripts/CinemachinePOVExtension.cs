using UnityEngine;
using Cinemachine;

public class CinemachinePOVExtension : CinemachineExtension
{
    private InputManager inputManager;
    private Vector3 startingRotation;

    [SerializeField]
    private float speed = 10f;
    [SerializeField]
    private float clampAngle = 80f;

    private float lastX, lastY;

    protected override void Awake()
    {
        inputManager = InputManager.Instance;
        base.Awake();
    }

    protected override void PostPipelineStageCallback(CinemachineVirtualCameraBase vcam, CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
    {
        if (InventoryManager.Instance.isUi || OptionManager.Instance.isMenu)
        {
            state.RawOrientation = Quaternion.Euler(lastX, lastY, 0f);
            return;
        }

        if (vcam.Follow)
        {
            if(stage == CinemachineCore.Stage.Aim)
            {
                if (inputManager == null)
                    return;

                if (startingRotation == null)
                    startingRotation = transform.localRotation.eulerAngles;

                Vector2 delta = inputManager.GetMouseDelta();
                startingRotation.x += delta.x * speed * Time.deltaTime;
                startingRotation.y += delta.y * speed * Time.deltaTime;

                startingRotation.y = Mathf.Clamp(startingRotation.y, -clampAngle, clampAngle);
                state.RawOrientation = Quaternion.Euler(-startingRotation.y, startingRotation.x, 0f);
                lastX = Camera.main.transform.localEulerAngles.x;
                lastY = Camera.main.transform.localEulerAngles.y;
            }
        }
    }
}
