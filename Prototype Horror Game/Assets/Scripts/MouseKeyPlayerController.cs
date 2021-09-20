using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseKeyPlayerController : ControllerInterface
{
    public void Init()
    {

    }

    public MouseKeyPlayerController()
    {

    }

    public void UpdateControls()
    {

    }

    public void SetFacingDirection(Vector3 direction)
    {

    }

    public void AddLedgeDir(Vector3 ledgeDir)
    {

    }

    public Vector3 GetControlRotation()
    {
        return Vector3.zero;
    }

    public Vector3 GetMoveInput()
    {
        return new Vector3(
            Input.GetAxis("Horizontal"),
            0.0f,
            Input.GetAxis("Vertical")
            );
    }

    public Vector3 GetLookInput()
    {
        return new Vector3(
            Input.GetAxis("Mouse X"),
            Input.GetAxis("Mouse Y"),
            0.0f
            );
    }

    public Vector3 GetAimTarget()
    {
        return Vector3.zero;
    }

    public bool IsJumping()
    {
        return Input.GetButton("Jump");
    }

    public bool IsFiring()
    {

        return Input.GetButton("Fire1");
    }

    public bool IsAiming()
    {
        return Input.GetButton("Aim");
    }

    public bool ToggleCrouch()
    {
        return Input.GetKeyDown(KeyCode.C);
    }
}
