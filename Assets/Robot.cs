/*EQUIPO
Gerardo Arturo Miranda Godoy
Mónica Lara Pineda
Manuel Ortiz Hernández
Óscar Contreras Palacios
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Robot : MonoBehaviour
{

    float[] dirLeftLeg = new float[4]{1.0f, 1.0f, 1.0f, 1.0f};
    float[] dirRightLeg = new float[4]{-1.0f, -1.0f, -1.0f, -1.0f};
    float[] dirLeftArm = new float[2]{1.0f, 1.0f};
    float[] dirRightArm = new float[2]{-1.0f, -1.0f}; 
    float dirChest = -1.0f;
    //Dictionary that saves all the angles                              /*Legs Angles*/
    Dictionary <string, float> angles = new Dictionary<string, float>(){{"mMovement", -10.0f}, {"MMovement", 10.0f},{"mKnee", -1.0f}, 
                                                                        {"MKnee", 1.0f},{"mFoot",20.0f}, {"MFoot", 0.0f},
                                                                        /*Arms Angles*/
                                                                        {"mShoulder", 10.0f}, {"MShoulder", -45.0f},
                                                                        /*Chest*/
                                                                        {"mChest", -10.0f}, {"MChest", 10.0f}};
    //Dictionary that saves all the rots variables                      /*Legs Rots*/
    Dictionary <string, float> rots = new Dictionary<string, float>(){  {"AllLegL", 0.0f}, {"WalkL", 10.0f}, {"FootL", 5.0f}, 
                                                                        {"KneeL", 0.5f}, {"AllLegR", 0.0f}, {"WalkR", 10.0f}, 
                                                                        {"FootR", 5.0f}, {"KneeR", 0.5f},
                                                                        /*Arms Rots*/
                                                                        {"ArmR", 0.0f}, {"ArmL", 0.0f}, {"ShoulderR", -10.0f}, 
                                                                        {"ShoulderL", -10.0f},
                                                                        /*Chest*/
                                                                        {"Chest", 0.0f}};
    float delta = 0.4f;
    //Array to  name all the game objects created and later on, found them by this name.
    string[] blocksNames = {"Hips", "Torso", "Neck", "Head","Chest",  
                            /*Arms*/ 
                            "ShoulderRight","ShoulderLeft", "BicepRight", "BicepLeft",
                            "ElbowRight", "ElbowLeft", "ForearmRight", "ForearmLeft",
                            "HandRight", "HandLeft", 
                             /*Legs*/
                            "ThighRight", "ThighLeft","KneeRight", "KneeLeft",
                            "LegRight","LegLeft",
                            "FootRight", "FootLeft"};

    void ApplyTransformations(Matrix4x4 t, GameObject target){
        Block b = target.GetComponent<Block>();
        Vector3[] newVerts = new Vector3[8];
        for (int v = 0; v < b.vertices.Length; v++){
            Vector4 temp = b.vertices[v];
            temp.w = 1;
            newVerts[v] = t * temp;
        }
        b.myMesh.vertices = newVerts;
    }

    // Start is called before the first frame update
    void Start(){
        for (int i = 0; i < blocksNames.Length; i++){
            GameObject go = new GameObject(blocksNames[i]);
            go.AddComponent<Block>();
        }
    }

    void AnimateArms(Matrix4x4 attachedI, float axisSide, string side, float rotArm, float rotShoulder){
        //SHOULDER 
        Matrix4x4 shoulderT = Transformations.TranslateM(0.7f*axisSide,0.05f,0);
        Matrix4x4 shoulderR = Transformations.RotateM(90, Transformations.AXIS.AX_Z);
        Matrix4x4 shoulderMove = Transformations.RotateM(rotArm, Transformations.AXIS.AX_Y);
        Matrix4x4 shoulderS = Transformations.ScaleM(0.3f,0.3f,0.3f);
        Matrix4x4 shoulderI = attachedI * shoulderT * shoulderR * shoulderMove;
        Matrix4x4 shoulderM = shoulderI * shoulderS;
        ApplyTransformations(shoulderM,GameObject.Find(string.Concat("Shoulder", side)) ) ;

        //BICEP
        Matrix4x4  bicepT = Transformations.TranslateM(-0.4f,0.0f,0);
        Matrix4x4  bicepR = Transformations.RotateM(90, Transformations.AXIS.AX_Z);
        Matrix4x4  bicepS = Transformations.ScaleM(0.2f,0.4f,0.2f);
        Matrix4x4  bicepI = shoulderI * bicepT * bicepR;
        Matrix4x4  bicepM = bicepI * bicepS;
        ApplyTransformations(bicepM, GameObject.Find(string.Concat("Bicep", side))) ;

        //ELBOW
        Matrix4x4  elbowT = Transformations.TranslateM(0.0f*axisSide,0.3f,0);
        Matrix4x4  elbowR = Transformations.RotateM(rotShoulder, Transformations.AXIS.AX_X);
        Matrix4x4  elbowS = Transformations.ScaleM(0.2f,0.2f,0.2f);
        Matrix4x4  elbowI = bicepI * elbowT * elbowR ;
        Matrix4x4  elbowM = elbowI * elbowS;
        ApplyTransformations(elbowM,GameObject.Find(string.Concat("Elbow", side)) ) ;

        //FOREARM
        Matrix4x4  forearmT = Transformations.TranslateM(0.0f*axisSide,0.3f,0);
        Matrix4x4  forearmR = Transformations.RotateM(90, Transformations.AXIS.AX_Z);
        Matrix4x4  forearmS = Transformations.ScaleM(0.4f,0.2f,0.2f);
        Matrix4x4  forearmI = elbowI * forearmT * forearmR ;
        Matrix4x4  forearmM = forearmI * forearmS;
        ApplyTransformations(forearmM,GameObject.Find(string.Concat("Forearm", side)) ) ;

        //HAND
        Matrix4x4  handT = Transformations.TranslateM(1.6f,0.0f,0);
        Matrix4x4  handS = Transformations.ScaleM(0.2f,0.2f,0.2f);
        Matrix4x4  handM = forearmI  * handS * handT  ;
        ApplyTransformations(handM,GameObject.Find(string.Concat("Hand", side)) ) ;

    }

    void AnimateLegs(Matrix4x4 attachedI, float axisSide, string side, float rotLegs, float rotWalk, float rotKnee, float rotFoot){
        Matrix4x4 thighT = Transformations.TranslateM(0.30f*axisSide, -0.65f, 0);
        Matrix4x4 thighS = Transformations.ScaleM(0.4f,0.8f, 0.5f);
        Matrix4x4 thighR = Transformations.RotateM(rotLegs*2.0f, Transformations.AXIS.AX_X);
        Matrix4x4 thighI = attachedI * thighT * thighR;
        Matrix4x4 thighM = thighI * thighS;
        ApplyTransformations(thighM, GameObject.Find(string.Concat("Thigh", side))); //Concat to get ThighRight or ThighLeft
        
        Matrix4x4 kneeT = Transformations.TranslateM(0.0f*axisSide, -0.60f, 0);
        Matrix4x4 kneeS = Transformations.ScaleM(0.4f, 0.4f, 0.5f);
        Matrix4x4 kneeRotation = Transformations.RotateM(rotLegs, Transformations.AXIS.AX_X);
        Matrix4x4 kneeR = Transformations.RotateM(rotKnee/1.2f, Transformations.AXIS.AX_X);
        Matrix4x4 kneeI = thighI * kneeT * kneeRotation * kneeR ;
        Matrix4x4 kneeM = kneeI * kneeS;
        ApplyTransformations(kneeM, GameObject.Find(string.Concat("Knee", side))); //Concat to get KneeRight or KneeLeft

        Matrix4x4 legT = Transformations.TranslateM(0.0f*axisSide, -0.70f, 0);
        Matrix4x4 legS = Transformations.ScaleM(0.4f, 1.0f, 0.5f);
        Matrix4x4 legR = Transformations.RotateM(-rotLegs, Transformations.AXIS.AX_X);
        Matrix4x4 legRotation = Transformations.RotateM(rotWalk, Transformations.AXIS.AX_X);
        Matrix4x4 legI = kneeI * legT * legRotation * legR;
        Matrix4x4 legM = legI * legS;
        ApplyTransformations(legM, GameObject.Find(string.Concat("Leg", side)));

        Matrix4x4 footT = Transformations.TranslateM(0.0f*axisSide, -0.70f, -0.25f);
        Matrix4x4 footS = Transformations.ScaleM(0.4f, 0.4f, 0.8f);
        Matrix4x4 footR = Transformations.RotateM(-rotFoot, Transformations.AXIS.AX_X);
        Matrix4x4 footM = legI * footT * footS * footR;
        ApplyTransformations(footM, GameObject.Find(string.Concat("Foot", side)));
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
        Matrix4x4 torsoT = Transformations.TranslateM(0,0.6f,0);
        Matrix4x4 torsoS = Transformations.ScaleM(1.0f,0.7f,0.5f);;
        Matrix4x4 torsoI = hipsI * torsoT; //inherit from torso
        Matrix4x4 torsoM = torsoI * torsoS;
        ApplyTransformations(torsoM, GameObject.Find("Torso"));

        //CHEST
        rots["Chest"] += dirChest * delta;
        if (rots["Chest"] > angles["MChest"] || rots["Chest"] < angles["mChest"]) dirChest = -dirChest;
        Matrix4x4 chestT = Transformations.TranslateM(0, 0.55f, 0);
        Matrix4x4 chestS = Transformations.ScaleM(1.0f, 0.4f, 0.8f);
        Matrix4x4 chestR = Transformations.RotateM(rots["Chest"], Transformations.AXIS.AX_X);
        Matrix4x4 chestI = torsoI * chestT;
        Matrix4x4 chestM = chestI *chestS * chestR; //inherit from torso
        ApplyTransformations(chestM, GameObject.Find("Chest"));
        
        //LEFT ARM
        rots["ArmL"] += dirLeftArm[0] * delta;
        if (rots["ArmL"] > angles["MMovement"] || rots["ArmL"] < angles["mMovement"]) dirLeftArm[0] = -dirLeftArm[0];    
        rots["ShoulderL"] += dirLeftArm[1] * delta;
        if (rots["ShoulderL"] > angles["MShoulder"] || rots["ShoulderL"] < angles["mShoulder"]) dirLeftArm[1] = -dirLeftArm[1];
        AnimateArms(chestI, -1.0f, "Left", rots["ArmL"], rots["ShoulderL"]);

        //RIGHT ARM
        rots["ArmR"] += dirRightArm[0] * delta;
        if (rots["ArmR"] > angles["MMovement"] || rots["ArmR"] < angles["mMovement"]) dirRightArm[0] = -dirRightArm[0];    
        rots["ShoulderR"] += dirRightArm[1] * delta;
        if (rots["ShoulderR"] > angles["MShoulder"] || rots["ShoulderR"] < angles["mShoulder"]) dirRightArm[1] = -dirRightArm[1];
        AnimateArms(chestI, 1.0f, "Right", rots["ArmR"], rots["ShoulderR"]);

        //NECK
        Matrix4x4 neckT = Transformations.TranslateM(0,0.35f,0);
        Matrix4x4 neckS = Transformations.ScaleM(0.3f,0.3f,0.3f);
        Matrix4x4 neckI = chestI * neckT ; //inherit from torso y rotacion si tiene
        Matrix4x4 neckM = neckI * neckS;
        ApplyTransformations(neckM, GameObject.Find("Neck"));

        //HEAD, does not pass property
        Matrix4x4 headT = Transformations.TranslateM(0,0.4f,0);
        Matrix4x4 headS = Transformations.ScaleM(0.5f,0.5f,0.5f);;
        Matrix4x4 headM = neckI * headT * headS;
        ApplyTransformations(headM, GameObject.Find("Head"));

        //LEFT LEG
        rots["AllLegL"] += dirLeftLeg[0] * delta; //For the thigh
        if (rots["AllLegL"] > angles["MMovement"] || rots["AllLegL"] < angles["mMovement"]) dirLeftLeg[0] = -dirLeftLeg[0]; 
        rots["WalkL"] += dirLeftLeg[1] * delta; //For the way the walk looks (the movement of the leg)
        if (rots["WalkL"] > angles["MMovement"] || rots["WalkL"] < angles["mMovement"]) dirLeftLeg[1] = -dirLeftLeg[1];
        rots["KneeL"] += dirLeftLeg[2] * delta; //Minor rotation of the knee
        if (rots["KneeL"] > angles["MKnee"] || rots["KneeL"] < angles["mKnee"]) dirLeftLeg[2] = -dirLeftLeg[2];
        rots["FootL"] += dirLeftLeg[3] * delta; //For the foot
        if (rots["FootL"] > angles["MFoot"] || rots["FootL"] < angles["mFoot"]) dirLeftLeg[3] = -dirLeftLeg[3];
        AnimateLegs(hipsI, -1.0f, "Left", rots["AllLegL"], rots["WalkL"] , rots["KneeL"], rots["FootL"]);

        //RIGHT LEG
        rots["AllLegR"] += dirRightLeg[0] * delta;//For the thigh
        if (rots["AllLegR"] > angles["MMovement"] || rots["AllLegR"] < angles["mMovement"]) dirRightLeg[0] = -dirRightLeg[0];
        rots["WalkR"] += dirRightLeg[1] * delta;//For the way the walk looks (the movement of the leg)
        if (rots["WalkR"] > angles["MMovement"] || rots["WalkR"] < angles["mMovement"]) dirRightLeg[1] = -dirRightLeg[1];
        rots["KneeR"] += dirRightLeg[2] * delta;//Minor rotation of the knee
        if (rots["KneeR"] > angles["MKnee"] || rots["KneeR"] < angles["mKnee"]) dirRightLeg[2] = -dirRightLeg[2];
        rots["FootR"] += dirRightLeg[3] * delta;//For the foot
        if (rots["FootR"] > angles["MFoot"] || rots["FootR"] < angles["mFoot"]) dirRightLeg[3] = -dirRightLeg[3];
        AnimateLegs(hipsI, 1.0f, "Right", rots["AllLegR"], rots["WalkR"] , rots["KneeR"], rots["FootR"]);
    }
}