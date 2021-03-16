//EQUIPO BICHOTAS
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Robot : MonoBehaviour
{
    //Movement Variables for LeftThigh.
    float directionLeftThigh = 1.0f;    // dir can only be 1 or -1
    readonly float deltaLeftThigh = 0.7f;  // how much to change rotation on each frame
    readonly float minAngleLeftThigh = 25.0f;  // minimum rotation angle in X
    readonly float maxAngleLeftThigh = -25.0f;   // maximum rotation angle in X
    float rotationLeftThigh = 0.0f;  // initial value
    //End of movement variables.

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

    void ApplyTransformations(Matrix4x4 t, GameObject target)
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
    }

    Vector3[] ApplyTransformationsVector(Matrix4x4 t, GameObject target)
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
    Vector3[] ModelBlockWRotationX(string blockToCreate, float rotationAngle, Vector3 toScale, Vector3 toTranslate)
    {
        Matrix4x4 s = Transformations.ScaleM(toScale.x, toScale.y, toScale.z);
        Matrix4x4 t = Transformations.TranslateM(toTranslate.x, toTranslate.y, toTranslate.z);
        Matrix4x4 temporalLeftThighMovement = Transformations.RotateM(rotationAngle, Transformations.AXIS.AX_X);
        t = t  *  temporalLeftThighMovement;
        return ApplyTransformationsVector(t * s, GameObject.Find(blockToCreate));
    }
    Vector3[] ModelBlockWRotationXWithPivot(string blockToCreate, Vector3 pivot, float rotationAngle, Vector3 toScale, Vector3 toTranslate)
    {
        Matrix4x4 s = Transformations.ScaleM(toScale.x, toScale.y, toScale.z);
        Matrix4x4 p = Transformations.Pivoted(pivot.x, pivot.y, pivot.z);
        Matrix4x4 temporalLeftThighMovement = Transformations.RotateM(rotationAngle, Transformations.AXIS.AX_X);
        temporalLeftThighMovement = p * temporalLeftThighMovement;
        Vector3[] tempM = ApplyTransformationsVector(p, GameObject.Find(blockToCreate));
        Matrix4x4 temporalLeftThighMovement2 = Transformations.RotateM(rotationAngle*1.0008f, Transformations.AXIS.AX_X);
        temporalLeftThighMovement = (p *temporalLeftThighMovement2) * Transformations.Pivoted(pivot.x * -1, pivot.y * -1,  pivot.z * -1);
        Matrix4x4 t = Transformations.TranslateM(toTranslate.x, toTranslate.y, toTranslate.z);
        t *= temporalLeftThighMovement;
        return ApplyTransformationsVector(t * s, GameObject.Find(blockToCreate));
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
        //Make sure that the string to send in ModelBlock exists in the array: blocksNames
        // HIPS (Root)
        Vector3 hipsSize = new Vector3(1, 0.3f, 0.5f);
        Matrix4x4 hipsM = ModelBlock("Hips", hipsSize /*Scale*/, new Vector3(0, 0, 0) /*Translate*/);
        //TORSO 
        Vector3 torsoSize = new Vector3(1, 1, 0.5f);
        Matrix4x4 torsoM = ModelBlock("Torso", torsoSize /*Scale*/, new Vector3(0, 0.7f, 0)/*Translate*/);
        // NECK
        Vector3 neckSize = new Vector3(0.15f, 0.20f, 0.15f);
        Matrix4x4 neckM = ModelBlock("Neck", neckSize /*Scale*/, new Vector3(0, 1.3f, 0) /*Translate*/);
        // HEAD
        Vector3 headSize = new Vector3(0.4f, 0.4f, 0.4f);
        Matrix4x4 headM = ModelBlock("Head", headSize /*Scale*/, new Vector3(0, 1.5f, 0) /*Translate*/);
        // LEGS
        // ThighRight
        Vector3 thighSize = new Vector3(0.5f, 1.0f, 0.45f);
        Matrix4x4 thighRightM = ModelBlock("ThighRight", thighSize /*Scale*/, new Vector3(0.25f, -0.7f, 0f) /*Translate*/);

        //ThighLeft
        ApplyTransformations(Transformations.ScaleM(thighSize.x, thighSize.y, thighSize.z), GameObject.Find("ThighLeft"));
        rotationLeftThigh = rotationLeftThigh - directionLeftThigh * deltaLeftThigh;
        Debug.Log("El valor de ángulo del muslo izquierdo es: " + rotationLeftThigh);
        if (rotationLeftThigh < maxAngleLeftThigh || rotationLeftThigh > minAngleLeftThigh) directionLeftThigh = -directionLeftThigh;
        Vector3[] thighLeftM = ModelBlockWRotationX("ThighLeft", rotationLeftThigh, thighSize/*Scale*/, new Vector3(-0.25f, -0.7f, 0f) /*Translate*/);

        //KneeRight
        Vector3 kneeSize = new Vector3(0.5f, 0.4f, 0.45f);
        Matrix4x4 kneeRightM = ModelBlock("KneeRight", kneeSize /*Scale*/, new Vector3(0.25f, -1.5f, 0) /*Translate*/);
        //KneeLeft
        Vector3[] kneeLeftM = ModelBlockWRotationXWithPivot("KneeLeft", new Vector3(thighLeftM[7].x, thighLeftM[7].y, thighLeftM[7].z)   , rotationLeftThigh, kneeSize /*Scale*/, new Vector3(-0.25f, -1.5f, 0) /*Translate*/);

        //LegRight
        Vector3 legSize = new Vector3(0.5f, 1.0f, 0.45f);
        Matrix4x4 legRightM = ModelBlock("LegRight", legSize /*Scale*/, new Vector3(0.25f, -2.3f, 0) /*Translate*/);
        //LegLeft
        Vector3[] legLeftM = ModelBlockWRotationXWithPivot("LegLeft", new Vector3(kneeLeftM[7].x, kneeLeftM[7].y, kneeLeftM[7].z), rotationLeftThigh, legSize /*Scale*/, new Vector3(-0.25f, -2.3f, 0) /*Translate*/);

        //FootRight
        Vector3 footSize = new Vector3(0.5f, 0.5f, 1.0f);
        Matrix4x4 footRightM = ModelBlock("FootRight", footSize /*Scale*/, new Vector3(0.25f, -3.2f, -0.3f) /*Translate*/);
        //FootLeft
        Vector3[] footLeftM = ModelBlockWRotationXWithPivot("FootLeft", new Vector3(legLeftM[7].x, legLeftM[7].y, legLeftM[7].z), rotationLeftThigh, footSize /*Scale*/, new Vector3(-0.25f, -3.2f, -0.3f) /*Translate*/);

        //ARM
        //ShoulderRight
        Vector3 shoulderSize = new Vector3(0.4f, 0.4f, 0.4f);
        Matrix4x4 shoulderRightM = ModelBlock("ShoulderRight", shoulderSize /*Scale*/, new Vector3(0.75f, 1.0f, 0) /*Translate*/);
        //ShoulderLeft
        Matrix4x4 shoulderLeftM = ModelBlock("ShoulderLeft", shoulderSize /*Scale*/, new Vector3(-0.75f, 1.0f, 0) /*Translate*/);

        //BicepRight
        Vector3 bicepSize = new Vector3(0.3f, 1.0f, 0.3f);
        Matrix4x4 bicepRightM = ModelBlock("BicepRight", bicepSize /*Scale*/, new Vector3(0.75f, 0.28f, 0) /*Translate*/);
        //BicepLeft
        Matrix4x4 bicepLeftM = ModelBlock("BicepLeft", bicepSize /*Scale*/, new Vector3(-0.75f, 0.28f, 0) /*Translate*/);

        //ElbowRight
        Vector3 elbowSize = new Vector3(0.2f, 0.2f, 0.2f);
        Matrix4x4 elbowRightM = ModelBlock("ElbowRight", elbowSize /*Scale*/, new Vector3(0.75f, -0.36f, 0) /*Translate*/);
        //ElbowLeft
        Matrix4x4 elbowLeftM = ModelBlock("ElbowLeft", elbowSize /*Scale*/, new Vector3(-0.75f, -0.36f, 0) /*Translate*/);

        //ForearmRight
        Vector3 forearmSize = new Vector3(0.3f, 0.6f, 0.3f);
        Matrix4x4 forearmRightM = ModelBlock("ForearmRight", forearmSize /*Scale*/, new Vector3(0.75f, -0.77f, 0) /*Translate*/);
        //ForearmLeft
        Matrix4x4 forearmLeftM = ModelBlock("ForearmLeft", forearmSize /*Scale*/, new Vector3(-0.75f, -0.77f, 0) /*Translate*/);

        //HandRight
        Vector3 handSize = new Vector3(0.2f, 0.2f, 0.2f);
        Matrix4x4 handRightM = ModelBlock("HandRight", handSize /*Scale*/, new Vector3(0.75f, -1.19f, 0) /*Translate*/);
        //HandLeft
        Matrix4x4 handLeftM = ModelBlock("HandLeft", handSize /*Scale*/, new Vector3(-0.75f, -1.19f, 0) /*Translate*/);

        
    }
}
