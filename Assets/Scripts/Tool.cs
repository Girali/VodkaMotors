using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tool : FurniturePiece
{
    public ToolType toolType;
}

public enum ToolType
{
    Hand,
    Glue,
    Screwdriver,
    Hexkey
}