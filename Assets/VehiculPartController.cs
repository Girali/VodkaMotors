using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehiculPartController : MonoBehaviour
{
    public InteractablePart engine;
    public InteractablePart wheel_FL;
    public InteractablePart wheel_FR;
    public InteractablePart wheel_BL;
    public InteractablePart wheel_BR;
    public InteractablePart seat;
    public InteractablePart roof;
    public InteractablePart armature;
    public InteractablePart stairingWheel;
    public InteractablePart back;
    public InteractablePart carrige;
    public InteractablePart spoiler;
    public InteractablePart sidebar;
    public InteractablePart bumper;
    public InteractablePart exaust;
    public InteractablePart bonnet;
    public InteractablePart doorLeft;
    public InteractablePart doorRight;

    private StairingWheelPart stairingWheelPart;
    private VehiculMotor vehiculMotor;


    public GameObject partsParent;

    public InteractablePart GetPart(VehiculParts vp, int index)
    {
        switch (vp)
        {
            case VehiculParts.None:
                break;
            case VehiculParts.Engine:
                return engine;
            case VehiculParts.Wheel:
                switch (index)
                {
                    case 0:
                        return wheel_FL;
                    case 1:
                        return wheel_FR;
                    case 2:
                        return wheel_BL;
                    case 3:
                        return wheel_BR;
                    default:
                        break;
                }
                break;
            case VehiculParts.Seat:
                return seat;
            case VehiculParts.Roof:
                return roof;
            case VehiculParts.Armature:
                return armature;
            case VehiculParts.SteeringWheel:
                return stairingWheel;
            case VehiculParts.Back:
                return back;
            case VehiculParts.Carrige:
                return carrige;
            case VehiculParts.Spoiler:
                return spoiler;
            case VehiculParts.Sidebar:
                return sidebar;
            case VehiculParts.Bumper:
                return bumper;
            case VehiculParts.Exhaust:
                return exaust;
            case VehiculParts.Bonnet:
                return bonnet;
            case VehiculParts.Door:
                switch (index)
                {
                    case 0:
                        return doorLeft;
                    case 1:
                        return doorRight;
                    default:
                        break;
                }
                break;
            default:
                break;
        }

        return null;
    }

    public VehiculPartObject ReplacePiece(VehiculPartObject vpo, int index)
    {
        InteractablePart ip = null;

        switch (vpo.part)
        {
            case VehiculParts.None:
                break;
            case VehiculParts.Engine:
                ip = engine;
                break;
            case VehiculParts.Wheel:
                switch (index)
                {
                    case 0:
                        ip = wheel_FL;
                        break;
                    case 1:
                        ip = wheel_FR;
                        break;
                    case 2:
                        ip = wheel_BL;
                        break;
                    case 3:
                        ip = wheel_BR;
                        break;
                    default:
                        break;
                }
                break;
            case VehiculParts.Seat:
                ip = seat;
                break;
            case VehiculParts.Roof:
                ip = roof;
                break;
            case VehiculParts.Armature:
                ip = armature;
                break;
            case VehiculParts.SteeringWheel:
                ip = stairingWheel;
                break;
            case VehiculParts.Back:
                ip = back;
                break;
            case VehiculParts.Carrige:
                ip = carrige;
                break;
            case VehiculParts.Spoiler:
                ip = spoiler;
                break;
            case VehiculParts.Sidebar:
                ip = sidebar;
                break;
            case VehiculParts.Bumper:
                ip = bumper;
                break;
            case VehiculParts.Exhaust:
                ip = exaust;
                break;
            case VehiculParts.Bonnet:
                ip = bonnet;
                break;
            case VehiculParts.Door:
                switch (index)
                {
                    case 0:
                        ip = doorLeft;
                        break;
                    case 1:
                        ip = doorRight;
                        break;
                    default:
                        break;
                }
                break;
            default:
                break;
        }

        VehiculPartObject v = ip ? ip.vehiculPartObject : null;

        if(ip != null)
            Destroy(ip.gameObject);

        if (vpo.prefab != null)
        {
            GameObject g = Instantiate(vpo.prefab, partsParent.transform);
            g.transform.localPosition = Vector3.zero;
            g.transform.localRotation = Quaternion.identity;

            if (vpo.part == VehiculParts.Wheel)
            {
                FramePart framePart = (FramePart)carrige;
                switch (index)
                {
                    case 0:
                        g.transform.position = framePart.frontLeft.position;
                        g.transform.rotation = framePart.frontLeft.rotation;
                        break;
                    case 1:
                        g.transform.position = framePart.frontRight.position;
                        g.transform.rotation = framePart.frontRight.rotation;
                        break;
                    case 2:
                        g.transform.position = framePart.backLeft.position;
                        g.transform.rotation = framePart.backLeft.rotation;
                        break;
                    case 3:
                        g.transform.position = framePart.backRight.position;
                        g.transform.rotation = framePart.backRight.rotation;
                        break;
                    default:
                        break;
                }
            }


            if (vpo.part == VehiculParts.Door)
                if (index == 0)
                    g.transform.localScale = new Vector3(-1, 1, 1);

            ip = g.GetComponent<InteractablePart>();
        }
        else
        {
            ip = null;
        }

        switch (vpo.part)
            {
                case VehiculParts.None:
                    break;
                case VehiculParts.Engine:
                    engine = ip;
                    break;
                case VehiculParts.Wheel:
                    switch (index)
                    {
                        case 0:
                            wheel_FL= ip;
                            break;
                        case 1:
                            wheel_FR = ip;
                            break;
                        case 2:
                            wheel_BL = ip;
                            break;
                        case 3:
                            wheel_BR = ip;
                            break;
                        default:
                            break;
                    }
                    break;
                case VehiculParts.Seat:
                    seat = ip;
                    break;
                case VehiculParts.Roof:
                    roof = ip;
                    break;
                case VehiculParts.Armature:
                    armature = ip;
                    break;
                case VehiculParts.SteeringWheel:
                    stairingWheel = ip;
                    break;
                case VehiculParts.Back:
                    back = ip;
                    break;
                case VehiculParts.Carrige:
                    carrige = ip;
                    break;
                case VehiculParts.Spoiler:
                    spoiler = ip;
                    break;
                case VehiculParts.Sidebar:
                    sidebar = ip;
                    break;
                case VehiculParts.Bumper:
                    bumper = ip;
                    break;
                case VehiculParts.Exhaust:
                    exaust = ip;
                    break;
                case VehiculParts.Bonnet:
                    bonnet = ip;
                    break;
                case VehiculParts.Door:
                    switch (index)
                    {
                        case 0:
                            doorLeft = ip;
                            break;
                        case 1:
                            doorRight = ip;
                            break;
                        default:
                            break;
                    }
                    break;
                default:
                    break;
            }

        UpdateVehiculPart();

        return v;
    }

    private void Start()
    {
        vehiculMotor = GetComponent<VehiculMotor>();
        UpdateVehiculPart();
    }

    public void UpdateView(float angle)
    {
        stairingWheelPart.UpdateView(angle);
    }

    public void UpdateVehiculPart()
    {
        if (engine)
        {
            engine.StopInteraction();
            engine.Init(vehiculMotor);
        }

        if (wheel_FL)
        {
            wheel_FL.StopInteraction();
            wheel_FL.Init(vehiculMotor);
        }

        if (wheel_FR)
        {
            wheel_FR.StopInteraction();
            wheel_FR.Init(vehiculMotor);
        }

        if (wheel_BL)
        {
            wheel_BL.StopInteraction();
            wheel_BL.Init(vehiculMotor);
        }

        if (wheel_BR)
        {
            wheel_BR.StopInteraction();
            wheel_BR.Init(vehiculMotor);
        }

        if (seat)
        {
            seat.StopInteraction();
            seat.Init(vehiculMotor);
        }

        if (roof)
        {
            roof.StopInteraction();
            roof.Init(vehiculMotor);
        }

        if (armature)
        {
            armature.StopInteraction();
            armature.Init(vehiculMotor);
        }

        if (stairingWheel)
        {
            stairingWheel.StopInteraction();
            stairingWheelPart = (StairingWheelPart)stairingWheel;
            stairingWheel.Init(vehiculMotor);
        }

        if (back)
        {
            back.StopInteraction();
            back.Init(vehiculMotor);
        }

        if (carrige)
        {
            carrige.StopInteraction();
            FramePart framePart = (FramePart)carrige;

            wheel_FL.transform.position = framePart.frontLeft.position;
            wheel_FL.transform.rotation = framePart.frontLeft.rotation;

            wheel_FR.transform.position = framePart.frontRight.position;
            wheel_FR.transform.rotation = framePart.frontRight.rotation;

            wheel_BL.transform.position = framePart.backLeft.position;
            wheel_BL.transform.rotation = framePart.backLeft.rotation;

            wheel_BR.transform.position = framePart.backRight.position;
            wheel_BR.transform.rotation = framePart.backRight.rotation;
            carrige.Init(vehiculMotor);
        }

        if (spoiler)
        {
            spoiler.StopInteraction();
            spoiler.Init(vehiculMotor);
        }

        if (sidebar)
        {
            sidebar.StopInteraction();
            sidebar.Init(vehiculMotor);
        }

        if (bumper)
        {
            bumper.StopInteraction();
            bumper.Init(vehiculMotor);
        }

        if (exaust)
        {
            exaust.StopInteraction();
            exaust.Init(vehiculMotor);
        }

        if (bonnet)
        {
            bonnet.StopInteraction();
            bonnet.Init(vehiculMotor);
        }

        if (doorLeft)
        {
            doorLeft.StopInteraction();
            doorLeft.Init(vehiculMotor);
        }

        if (doorRight)
        {
            doorRight.StopInteraction();
            doorRight.Init(vehiculMotor);
        }


        vehiculMotor.frontWheels[0] = wheel_FL.GetComponent<WheelCollider>();
        vehiculMotor.frontWheels[1] = wheel_FR.GetComponent<WheelCollider>();
        vehiculMotor.rearWheels[0] = wheel_BL.GetComponent<WheelCollider>();
        vehiculMotor.rearWheels[1] = wheel_BR.GetComponent<WheelCollider>();

        vehiculMotor.Init();
    }
}
