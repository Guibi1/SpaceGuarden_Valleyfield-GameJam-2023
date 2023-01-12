using System.Collections;
using System.Collections.Generic;
using Shapes;
using UnityEngine;

[ExecuteInEditMode]
[ExecuteAlways] 
public class PlantAoEDrawer : ImmediateModeShapeDrawer
{

    public Color DrawerColor;
    public float size;
    
    public override void DrawShapes( Camera cam ){

        using( Draw.Command( cam ) ){

            // set up static parameters. these are used for all following Draw.Line calls
            Draw.LineGeometry = LineGeometry.Flat2D;
		    Draw.ThicknessSpace = ThicknessSpace.Meters;
            Draw.Thickness = 3; // 4px wide

            // set static parameter to draw in the local space of this object
            Draw.Matrix = transform.localToWorldMatrix;
            
            Draw.Disc(transform.localPosition, new Vector3(0,90,0), size,DrawerColor);
        }

    }

}
