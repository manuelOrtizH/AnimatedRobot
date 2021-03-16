//EQUIPO BICHOTAS
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Robot : MonoBehaviour
{
    string side;
    float axisSide;
    float directionLeftArm = 1.0f;
    float directionBody = 1.0f;
    float directionLeftLeg = 1.0f;
    float directionRightArm = -1.0f;
    float directionRightLeg = -1.0f;
    float minAngleLegs = 25.0f;  
    float maxAngleLegs = -25.0f;  
    float minAngleBody = 2.0f;  
    float maxAngleBody = -2.0f;  
    float deltaLegs = 0.7f;
    float deltaBody = 0.1f;
    float rotationLegsLeft = 0.0f;
    float rotationLegsRight = 0.0f;
    float minAngleArms = 5.0f;  // minimum rotation angle in X
    float maxAngleArms = -5.0f;  
    float minAngleArmsRight = 5.0f;  // minimum rotation angle in X
    float maxAngleArmsRight = -5.0f; 
    float deltaArms = 0.25f;
    float rotationArmsLeft = 0.0f;
    float rotationBody = 0.0f;
    float rotationArmsRight = 0.0f;
    //End of movement variables.
    Vector3 hipsSize = new Vector3(1, 0.3f, 0.5f);
    Vector3 torsoSize = new Vector3(1, 1, 0.5f);
    Vector3 neckSize = new Vector3(0.15f, 0.20f, 0.15f);
    Vector3 headSize = new Vector3(0.4f, 0.4f, 0.4f);
    
    Vector3 shoulderSize = new Vector3(0.4f, 0.4f, 0.4f);
    Vector3 bicepSize = new Vector3(0.3f, 1.0f, 0.3f);
    Vector3 elbowSize = new Vector3(0.2f, 0.2f, 0.2f);
    Vector3 forearmSize = new Vector3(0.3f, 0.6f, 0.3f);
    Vector3 handSize = new Vector3(0.2f, 0.2f, 0.2f);

    Vector3 thighSize = new Vector3(0.5f, 1.0f, 0.45f);
    Vector3 kneeSize = new Vector3(0.5f, 0.4f, 0.45f);
    Vector3 legSize = new Vector3(0.5f, 1.0f, 0.45f);
    Vector3 footSize = new Vector3(0.5f, 0.5f, 1.0f);

    //Array to  name all the game objects created and later on, found them by this name.
    string[] blocksNames = {"Hips", "Torso", "Neck", "Head", 
                            /*Arms*/ 
                            "ShoulderRight","ShoulderLeft", "BicepRight", "BicepLeft",
                            "ElbowRight", "ElbowLeft", "ForearmRight", "ForearmLeft",
                            "HandRight", "HandLeft", 
                             /*Legs*/
                            "ThighRight", "ThighLeft","KneeRight", "KneeLeft",
                            "LegRight","LegLeft",
                            "FootRight", "FootLeft"};

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

    //Single method for aplying translation, scale and then aplying transformations
    Matrix4x4 ModelBlock(string blockToCreate, Vector3 toScale, Vector3 toTranslate)
    {
        Matrix4x4 m = Transformations.ScaleM(toScale.x, toScale.y, toScale.z);
        Matrix4x4 t = Transformations.TranslateM(toTranslate.x, toTranslate.y, toTranslate.z);
        m = t * m;
        ApplyTransformations(m, GameObject.Find(blockToCreate));
        return m;
    }

    Vector3[] ModelBlockWRotationX(string blockToCreate, float rotationAngle, Vector3 toScale, float side, Vector3 toTranslate)
    {
        Matrix4x4 s = Transformations.ScaleM(toScale.x, toScale.y, toScale.z);
        Matrix4x4 t = Transformations.TranslateM(toTranslate.x*side, toTranslate.y, toTranslate.z);
        Matrix4x4 temporalLeftThighMovement = Transformations.RotateM(rotationAngle, Transformations.AXIS.AX_X);
        t = t  *  temporalLeftThighMovement;
        return ApplyTransformations(t * s, GameObject.Find(blockToCreate));
    }


    Vector3[] ModelBlockWRotationY(string blockToCreate, float rotationAngle, Vector3 toScale, Vector3 toTranslate)
    {
        Matrix4x4 s = Transformations.ScaleM(toScale.x, toScale.y, toScale.z);
        Matrix4x4 t = Transformations.TranslateM(toTranslate.x, toTranslate.y, toTranslate.z);
        Matrix4x4 temporalLeftThighMovement = Transformations.RotateM(rotationAngle, Transformations.AXIS.AX_Y);
        t = t  *  temporalLeftThighMovement;
        return ApplyTransformations(t * s, GameObject.Find(blockToCreate));
    }

    Vector3[] ModelBlockWRotationYWithPivot(string blockToCreate, Vector3 pivot, float rotationAngle, Vector3 toScale, Vector3 toTranslate)
    {
        Matrix4x4 s = Transformations.ScaleM(toScale.x, toScale.y, toScale.z);
        Matrix4x4 p = Transformations.Pivoted(pivot.x, pivot.y, pivot.z);
        Matrix4x4 temporalLeftThighMovement = Transformations.RotateM(rotationAngle, Transformations.AXIS.AX_X);
        temporalLeftThighMovement = p * temporalLeftThighMovement;
        Vector3[] tempM = ApplyTransformations(p, GameObject.Find(blockToCreate));
        Matrix4x4 temporalLeftThighMovement2 = Transformations.RotateM(rotationAngle*1.0008f, Transformations.AXIS.AX_Y);
        temporalLeftThighMovement = (p *temporalLeftThighMovement2) * Transformations.Pivoted(pivot.x * -1, pivot.y * -1,  pivot.z * -1);
        Matrix4x4 t = Transformations.TranslateM(toTranslate.x, toTranslate.y, toTranslate.z);
        t *= temporalLeftThighMovement;
        return ApplyTransformations(t * s, GameObject.Find(blockToCreate));
    }

    Vector3[] ModelBlockWRotationXWithPivot(string blockToCreate, Vector3 pivot, float rotationAngle, Vector3 toScale, float side, Vector3 toTranslate)
    {
        Matrix4x4 s = Transformations.ScaleM(toScale.x, toScale.y, toScale.z);
        Matrix4x4 p = Transformations.Pivoted(pivot.x, pivot.y, pivot.z);
        Matrix4x4 temporalLeftThighMovement = Transformations.RotateM(rotationAngle, Transformations.AXIS.AX_X);
        temporalLeftThighMovement = p * temporalLeftThighMovement;
        Vector3[] tempM = ApplyTransformations(p, GameObject.Find(blockToCreate));
        Matrix4x4 temporalLeftThighMovement2 = Transformations.RotateM(rotationAngle*1.0008f, Transformations.AXIS.AX_X);
        temporalLeftThighMovement = (p *temporalLeftThighMovement2) * Transformations.Pivoted(pivot.x * -1, pivot.y * -1,  pivot.z * -1);
        Matrix4x4 t = Transformations.TranslateM(toTranslate.x*side, toTranslate.y, toTranslate.z);
        t *= temporalLeftThighMovement;
        return ApplyTransformations(t * s, GameObject.Find(blockToCreate));
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

    void AnimateLeg(bool isLeft, float rotation){
        float direction;
        if (isLeft){
            side = "Left";
            axisSide = -1.0f;
        }else{
            side = "Right";
            axisSide = 1.0f;
        }
     
        ApplyTransformations(Transformations.ScaleM(thighSize.x, thighSize.y, thighSize.z), GameObject.Find(string.Concat("Thigh", side)));
        //Pivot
        Vector3 thighPos = new Vector3(0.25f, -0.7f, 0f);
        Vector3[] thighM = ModelBlockWRotationX(string.Concat("Thigh", side), rotation, thighSize, axisSide, thighPos/*Translate*/);
        //Knee
        Vector3 kneePos =  new Vector3(0.25f, -1.5f, 0);
        Vector3[] kneeM = ModelBlockWRotationXWithPivot(string.Concat("Knee", side),  new Vector3(thighM[7].x, thighM[7].y, thighM[7].z) ,rotation, kneeSize, axisSide, kneePos);
        //Leg
        Vector3 legPos = new Vector3(0.25f, -2.3f, 0);
        Vector3[] legM = ModelBlockWRotationXWithPivot(string.Concat("Leg", side), new Vector3(kneeM[7].x, kneeM[7].y, kneeM[7].z), rotation, legSize, axisSide, legPos);
        //Foot
        Vector3 footPos = new Vector3(-0.25f, -3.2f, -0.3f);
        Vector3[] footM = ModelBlockWRotationXWithPivot(string.Concat("Foot", side), new Vector3(legM[7].x, legM[7].y, legM[7].z), rotation, footSize, axisSide, footPos);
    }

    void AnimateBody(float rotation){
        ApplyTransformations(Transformations.ScaleM(hipsSize.x, hipsSize.y, hipsSize.z), GameObject.Find("Hips"));
        Vector3 hipsPos = new Vector3(0,0,0);
        Vector3[] hipsM = ModelBlockWRotationY("Hips", rotation, hipsSize, hipsPos);
        
        Vector3 torsoPos = new Vector3(0, 0.7f, 0);
        Vector3[] torsoM = ModelBlockWRotationYWithPivot("Torso", new Vector3(hipsM[7].x, hipsM[7].y, hipsM[7].z),rotation, torsoSize, torsoPos);

        Vector3 neckSize = new Vector3(0.15f, 0.20f, 0.15f);
        Matrix4x4 neckM = ModelBlock("Neck", neckSize /*Scale*/, new Vector3(0, 1.3f, 0) /*Translate*/);
        // HEAD
        Vector3 headSize = new Vector3(0.4f, 0.4f, 0.4f);
        Matrix4x4 headM = ModelBlock("Head", headSize /*Scale*/, new Vector3(0, 1.5f, 0) /*Translate*/);
    }

    void AnimateArm(bool isLeft, float rotation){
        if (isLeft){
            side = "Left";
            axisSide = -1.0f;
        }else{
            side = "Right";
            axisSide = 1.0f;
        }

        ApplyTransformations(Transformations.ScaleM(shoulderSize.x, shoulderSize.y, shoulderSize.z), GameObject.Find(string.Concat("Shoulder", side)));        
        //Pivot
        Vector3 shoulderPos = new Vector3(0.75f, 1.0f, 0f);
        Vector3[] shoulderM = ModelBlockWRotationX(string.Concat("Shoulder", side), rotation, shoulderSize, axisSide, shoulderPos/*Translate*/);
        //Knee
        Vector3 bicepPos =  new Vector3(0.75f, 0.28f, 0);
        Vector3[] bicepM = ModelBlockWRotationXWithPivot(string.Concat("Bicep", side),  new Vector3(shoulderM[7].x, shoulderM[7].y, shoulderM[7].z) ,rotation, bicepSize, axisSide, bicepPos);
        //Leg
        Vector3 elbowPos = new Vector3(0.75f, -0.36f, 0);
        Vector3[] elbowM = ModelBlockWRotationXWithPivot(string.Concat("Elbow", side), new Vector3(bicepM[7].x, bicepM[7].y, bicepM[7].z), rotation, elbowSize, axisSide, elbowPos);
        //Foot
        Vector3 forearmPos = new Vector3(0.75f, -0.77f, 0);
        Vector3[] forearmM = ModelBlockWRotationXWithPivot(string.Concat("Forearm", side), new Vector3(elbowM[7].x, elbowM[7].y, elbowM[7].z), rotation, forearmSize, axisSide, forearmPos);
        //Hand
        Vector3 handPos = new Vector3(0.75f, -1.19f, 0);
        Vector3[] handM = ModelBlockWRotationXWithPivot(string.Concat("Hand", side), new Vector3(forearmM[7].x, forearmM[7].y, forearmM[7].z), rotation, handSize, axisSide, handPos);

    }

    // Update is called once per frame
    void Update()
    {
        rotationBody = rotationBody - directionBody * deltaBody;
        if (rotationBody < maxAngleBody || rotationBody > minAngleBody) directionBody = -directionBody;
        AnimateBody(rotationBody);
        //LeftSide
        rotationLegsLeft = rotationLegsLeft - directionLeftLeg * deltaLegs;
        if (rotationLegsLeft < maxAngleLegs || rotationLegsLeft > minAngleLegs) directionLeftLeg = -directionLeftLeg;
        AnimateLeg(true, rotationLegsLeft);
        rotationArmsLeft = rotationArmsLeft - directionLeftArm * deltaArms;
        if (rotationArmsLeft < maxAngleArms|| rotationArmsLeft > minAngleArms) directionLeftArm = -directionLeftArm;
        AnimateArm(true, rotationArmsLeft);
        //RightSide
        rotationLegsRight= rotationLegsRight - directionRightLeg * deltaLegs;
        if (rotationLegsRight < maxAngleLegs || rotationLegsRight > minAngleLegs) directionRightLeg = -directionRightLeg;
        AnimateLeg(false, rotationLegsRight);
        rotationArmsRight = rotationArmsRight - directionRightArm * deltaArms;
        if (rotationArmsRight < maxAngleArms || rotationArmsRight > minAngleArms) directionRightArm = -directionRightArm;
        AnimateArm(false, rotationArmsRight);
        
    }
}