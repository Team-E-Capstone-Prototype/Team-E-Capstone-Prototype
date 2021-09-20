using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ControllerInterface
{
    void Init();

    void UpdateControls();

    void SetFacingDirection(Vector3 direction);

    void AddLedgeDir(Vector3 ledgeDir);

    Vector3 GetControlRotation();

    Vector3 GetMoveInput();

    Vector3 GetLookInput();

    Vector3 GetAimTarget();

    bool IsJumping();

    bool IsFiring();

    bool IsAiming();

    bool ToggleCrouch();

}
