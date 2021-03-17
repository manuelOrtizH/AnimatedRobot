//EQUIPO BICHOTAS
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Robot : MonoBehaviour
{

    //rotation arms (shoulders)
    float dir = 1.0f;    // dir can only be 1 or -1
    float delta = 0.1f;// how much to change rotation on each frame
    float minAngle = -60.0f;  // minimum rotation angle in Z
    float maxAngle = -10.0f;// maximum rotation angle in Z
    float rotArm = -10.0f;

    //rotation arms (shoulders)

    float minAngleS = -90.0f;  // minimum rotation angle in Z
    float maxAngleS = -10.0f;// maximum rotation angle in Z
    float rotShoulder = -10.0f;



    //Array to  name all the game objects created and later on, found them by this name.
    string[] blocksNames = {"Hips", "Torso", "Neck", "Head", "ShoulderRight", "BicepRight","ElbowRight", "ForeArmRight", "HandRight"};

    Vector3[] ApplyTransformations(Matrix4x4 t, GameObject target)
    {
        Block b = target.GetComponent<Block>();
        Vector3[] newVerts = new Vector3[8];
        for (int v = 0; v < b.vertices.Length; v++)
        {
            Vector4 temp = b.vertices[v];
            temp.w = 1;
            newVerts[v] = t * temp;
        }
        b.myMesh.vertices = newVerts;
        return newVerts;
    }




    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < blocksNames.Length; i++)
        {
            GameObject go = new GameObject(blocksNames[i]);
            go.AddComponent<Block>();
        }
        
    }



    // Update is called once per frame
    void Update()
    {


        //HIPS
        Matrix4x4 hipsS = Transformations.ScaleM(1.0f,0.5f,0.5f);
        Matrix4x4 hipsM = hipsS;
        Matrix4x4 hipsI = Matrix4x4.identity; //inherit hips
        ApplyTransformations(hipsM, GameObject.Find("Hips"));
        
        //TORSO
        Matrix4x4 torsoT = Transformations.TranslateM(0,0.8f,0);
        Matrix4x4 torsoS = Transformations.ScaleM(1.0f,1.0f,0.5f);;
        Matrix4x4 torsoI = hipsI * torsoT; //inherit from torso
        Matrix4x4 torsoM = torsoI * torsoS;
        ApplyTransformations(torsoM, GameObject.Find("Torso"));

        //NECK

        Matrix4x4 neckT = Transformations.TranslateM(0,0.6f,0);
        Matrix4x4 neckS = Transformations.ScaleM(0.3f,0.3f,0.3f);
        Matrix4x4 neckI = torsoI * neckT ; //inherit from torso y rotacion si tiene
        Matrix4x4 neckM = neckI * neckS;
        ApplyTransformations(neckM, GameObject.Find("Neck"));

        //HEAD, does not pass property

        Matrix4x4 headT = Transformations.TranslateM(0,0.4f,0);
        Matrix4x4 headS = Transformations.ScaleM(0.5f,0.5f,0.5f);;
        Matrix4x4 headM = neckI * headT * headS;
        ApplyTransformations(headM, GameObject.Find("Head"));

        rotArm += dir * delta;
        if (rotArm > maxAngle || rotArm < minAngle) dir = -dir;

        //SHOULDER  
        Matrix4x4 shoulderRightT = Transformations.TranslateM(0.7f,0.35f,0);
        Matrix4x4 shoulderRightR = Transformations.RotateM(90, Transformations.AXIS.AX_Z);
        Matrix4x4 shoulderRightRMove = Transformations.RotateM(rotArm, Transformations.AXIS.AX_Y);
        Matrix4x4 shoulderRightS = Transformations.ScaleM(0.3f,0.3f,0.3f);
        Matrix4x4 shoulderRightI = torsoI * shoulderRightT * shoulderRightR * shoulderRightRMove;
        Matrix4x4 shoulderRightM = shoulderRightI * shoulderRightS;
        ApplyTransformations(shoulderRightM,GameObject.Find("ShoulderRight") ) ;

        //BICEP rigth 

        Matrix4x4  bicepRightT = Transformations.TranslateM(-0.4f,0.0f,0);
        Matrix4x4  bicepRightR = Transformations.RotateM(90, Transformations.AXIS.AX_Z);
        Matrix4x4  bicepRightS = Transformations.ScaleM(0.2f,0.4f,0.2f);
        Matrix4x4  bicepRightI = shoulderRightI * bicepRightT * bicepRightR;
        Matrix4x4  bicepRightM = bicepRightI * bicepRightS;
        ApplyTransformations(bicepRightM,GameObject.Find("BicepRight") ) ;

        //ELBOW right elbowRight

        rotShoulder += dir * delta;
        if (rotShoulder > maxAngle || rotShoulder < minAngle) dir = -dir;

        Matrix4x4  elbowRightT = Transformations.TranslateM(0.0f,0.3f,0);
        Matrix4x4   elbowRightR = Transformations.RotateM(rotShoulder, Transformations.AXIS.AX_X);
        Matrix4x4  elbowRightS = Transformations.ScaleM(0.2f,0.2f,0.2f);
        Matrix4x4  elbowRightI = bicepRightI * elbowRightT * elbowRightR ;
        Matrix4x4  elbowRightM = elbowRightI * elbowRightS;
        ApplyTransformations(elbowRightM,GameObject.Find("ElbowRight") ) ;

        //ForeArmRight

        Matrix4x4  foreArmRightT = Transformations.TranslateM(0.0f,0.3f,0);
        Matrix4x4  foreArmRightR = Transformations.RotateM(90, Transformations.AXIS.AX_Z);
        Matrix4x4  foreArmRightS = Transformations.ScaleM(0.4f,0.2f,0.2f);
        Matrix4x4  foreArmRightI = elbowRightI * foreArmRightT * foreArmRightR ;
        Matrix4x4  foreArmRightM = foreArmRightI * foreArmRightS;
        ApplyTransformations(foreArmRightM,GameObject.Find("ForeArmRight") ) ;

        //HandRight
        Matrix4x4  handRightT = Transformations.TranslateM(1.2f,0.0f,0);
        Matrix4x4  handRightS = Transformations.ScaleM(0.2f,0.2f,0.2f);
        Matrix4x4  handRightM =foreArmRightI  * handRightS * handRightT  ;
        ApplyTransformations(handRightM,GameObject.Find("HandRight") ) ;
    }
}