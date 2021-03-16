//EQUIPO BICHOTAS
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Robot : MonoBehaviour
{
    string side;
    float axisSide;
    //Directions
    float directionLeftArm = 1.0f;
    float directionBody = 1.0f;
    float directionLeftLeg = 1.0f;
    float directionRightArm = -1.0f;
    float directionRightLeg = -1.0f;
    //Angle
    readonly float minAngleLegs = 25.0f;  
    readonly float maxAngleLegs = -25.0f;  
    readonly float minAngleBody = 5.0f;  
    readonly float maxAngleBody = -5.0f;  
    readonly float minAngleArms = 10.0f;  
    readonly float maxAngleArms = -10.0f;  
    //Deltas
    float deltaLegs = 0.45f;
    float deltaBody = 0.05f;
    float deltaArms = 0.25f;
    //Rotations
    float rotationArmsLeft = 0.0f;
    float rotationLegsLeft = 0.0f;
    float rotationBody = 0.0f;
    float rotationArmsRight = 0.0f;
    float rotationLegsRight = 0.0f;
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

    Vector3[] Model(GameObject blockToCreate, float rotationAngle, Vector3 toScale, float axisSide, Vector3 toTranslate, Transformations.AXIS axis){
        Matrix4x4 s = Transformations.ScaleM(toScale.x, toScale.y, toScale.z);
        Matrix4x4 t = Transformations.TranslateM(toTranslate.x*axisSide, toTranslate.y, toTranslate.z);
        Matrix4x4 tempMovement = Transformations.RotateM(rotationAngle, axis);
        t = t  *  tempMovement;
        return ApplyTransformations(t * s, blockToCreate);
    }

    Vector3[] ModelWithPivot(GameObject blockToCreate, Vector3 pivot, float rotationAngle, Vector3 toScale, float axisSide,Vector3 toTranslate, Transformations.AXIS axis){
        Matrix4x4 s = Transformations.ScaleM(toScale.x, toScale.y, toScale.z);
        Matrix4x4 p = Transformations.Pivoted(pivot.x, pivot.y, pivot.z);
        Matrix4x4 tempMovement = Transformations.RotateM(rotationAngle, axis);
        tempMovement = p * tempMovement;
        Vector3[] tempM = ApplyTransformations(p, blockToCreate);
        Matrix4x4 tempMovement2 = Transformations.RotateM(rotationAngle*1.0008f, axis);
        tempMovement = (p *tempMovement2) * Transformations.Pivoted(pivot.x * -1, pivot.y * -1,  pivot.z * -1);
        Matrix4x4 t = Transformations.TranslateM(toTranslate.x*axisSide, toTranslate.y, toTranslate.z);
        t *= tempMovement;
        return ApplyTransformations(t * s, blockToCreate);
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
    //Method to check if the arm/leg is the right or left.
    void CheckAxisSide(bool isLeft){
        if (isLeft){
            side = "Left";
            axisSide = -1.0f;
        }else{
            side = "Right";
            axisSide = 1.0f;
        }
    }
    //Method to animate both legs
    void AnimateLeg(bool isLeft, float rotation){
        CheckAxisSide(isLeft);
        ApplyTransformations(Transformations.ScaleM(thighSize.x, thighSize.y, thighSize.z), GameObject.Find(string.Concat("Thigh", side)));
        Transformations.AXIS axis = Transformations.AXIS.AX_X;
        //Pivot
        Vector3 thighPos = new Vector3(0.25f, -0.7f, 0f);
        Vector3[] thighM = Model(GameObject.Find(string.Concat("Thigh", side)), rotation, thighSize, axisSide, thighPos, axis);
        //Knee
        Vector3 kneePos =  new Vector3(0.25f, -1.5f, 0);
        Vector3[] kneeM = ModelWithPivot(GameObject.Find(string.Concat("Knee", side)),  new Vector3(thighM[7].x, thighM[7].y, thighM[7].z) ,rotation, kneeSize, axisSide, kneePos, axis);
        //Leg
        Vector3 legPos = new Vector3(0.25f, -2.3f, 0);
        Vector3[] legM = ModelWithPivot(GameObject.Find(string.Concat("Leg", side)), new Vector3(kneeM[7].x, kneeM[7].y, kneeM[7].z), rotation, legSize, axisSide, legPos, axis);
        //Foot
        Vector3 footPos = new Vector3(-0.25f, -3.2f, -0.3f);
        Vector3[] footM = ModelWithPivot(GameObject.Find(string.Concat("Foot", side)), new Vector3(legM[7].x, legM[7].y, legM[7].z), rotation, footSize, axisSide, footPos, axis);
    }

    void AnimateBody(float rotation){
        ApplyTransformations(Transformations.ScaleM(hipsSize.x, hipsSize.y, hipsSize.z), GameObject.Find("Hips"));
        Transformations.AXIS axis = Transformations.AXIS.AX_Z;
        axisSide = 1.0f;
        //Hips
        Vector3 hipsPos = new Vector3(0,0,0);
        Vector3[] hipsM = Model(GameObject.Find("Hips"), rotation, hipsSize, axisSide,hipsPos, axis);
        //Torso
        Vector3 torsoPos = new Vector3(0, 0.7f, 0);
        Vector3[] torsoM = ModelWithPivot(GameObject.Find("Torso"), new Vector3(hipsM[7].x, hipsM[7].y, hipsM[7].z),rotation, torsoSize, axisSide,torsoPos, axis);
        //Neck
        Vector3 neckSize = new Vector3(0.15f, 0.20f, 0.15f);
        Matrix4x4 neckM = ModelBlock("Neck", neckSize /*Scale*/, new Vector3(0, 1.3f, 0));
        //Head
        Vector3 headSize = new Vector3(0.4f, 0.4f, 0.4f);
        Matrix4x4 headM = ModelBlock("Head", headSize /*Scale*/, new Vector3(0, 1.5f, 0));
    }
    //Method to animate both arms
    void AnimateArm(bool isLeft, float rotation){
        CheckAxisSide(isLeft);
        ApplyTransformations(Transformations.ScaleM(shoulderSize.x, shoulderSize.y, shoulderSize.z), GameObject.Find(string.Concat("Shoulder", side)));        
        Transformations.AXIS axis = Transformations.AXIS.AX_X;
        //Pivot
        Vector3 shoulderPos = new Vector3(0.75f, 1.0f, 0f);
        Vector3[] shoulderM = Model(GameObject.Find(string.Concat("Shoulder", side)), rotation, shoulderSize, axisSide, shoulderPos, axis);
        //Knee
        Vector3 bicepPos =  new Vector3(0.75f, 0.28f, 0);
        Vector3[] bicepM = ModelWithPivot(GameObject.Find(string.Concat("Bicep", side)),  new Vector3(shoulderM[7].x, shoulderM[7].y, shoulderM[7].z) ,rotation, bicepSize, axisSide, bicepPos, axis);
        //Leg
        Vector3 elbowPos = new Vector3(0.75f, -0.36f, 0);
        Vector3[] elbowM = ModelWithPivot(GameObject.Find(string.Concat("Elbow", side)), new Vector3(bicepM[7].x, bicepM[7].y, bicepM[7].z), rotation, elbowSize, axisSide, elbowPos, axis);
        //Foot
        Vector3 forearmPos = new Vector3(0.75f, -0.77f, 0);
        Vector3[] forearmM = ModelWithPivot(GameObject.Find(string.Concat("Forearm", side)), new Vector3(elbowM[7].x, elbowM[7].y, elbowM[7].z), rotation, forearmSize, axisSide, forearmPos, axis);
        //Hand
        Vector3 handPos = new Vector3(0.75f, -1.19f, 0);
        Vector3[] handM = ModelWithPivot(GameObject.Find(string.Concat("Hand", side)), new Vector3(forearmM[7].x, forearmM[7].y, forearmM[7].z), rotation, handSize, axisSide, handPos, axis);
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