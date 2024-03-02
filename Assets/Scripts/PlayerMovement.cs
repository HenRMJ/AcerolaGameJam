using Cinemachine;
using Unity.Netcode;
using UnityEngine;

public class PlayerMovement : NetworkBehaviour
{
    [SerializeField] private CinemachineVirtualCamera vCam;
    [SerializeField] private float speed;
    [SerializeField] private float rotateSpeed;

    public override void OnNetworkSpawn()
    {
        if (!IsOwner) return;

        vCam.Priority = 20;
    }

    private void Update()
    {
        if (!IsOwner) return;

        Vector2 inputVector = GameInput.Instance.GetNormalizedInput();
        Vector3 movementVector = new Vector3(inputVector.x, 0, inputVector.y);

        transform.position += movementVector * Time.deltaTime * speed;

        if (movementVector != Vector3.zero)
        {
            transform.forward = Vector3.Lerp(transform.forward, movementVector, Time.deltaTime * rotateSpeed);
        }
    }
}
