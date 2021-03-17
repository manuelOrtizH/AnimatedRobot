//EQUIPO BICHOTAS
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Robot : MonoBehaviour
{

    float[] dirLeft = new float[4]{1.0f, 1.0f, 1.0f, 1.0f};
    float[] dirRight = new float[4]{-1.0f, -1.0f, -1.0f, -1.0f};
    Dictionary <string, float> anglesLeg = new Dictionary<string, float>(){ {"mAllLeg", -10.0f}, {"MAllLeg", 10.0f},
                                                                            {"mKnee", -1.0f}, {"MKnee", 1.0f},
                                                                            {"mFoot",0.0f}, {"MFoot", -10.0f}};
    Dictionary <string, float> rotsLeg = new Dictionary<string, float>(){ {"AllLegL", 0.0f}, {"WalkL", 10.0f}, {"FootL", 8.0f}, {"KneeL", 0.0f},
                                                                          {"AllLegR", 0.0f}, {"WalkR", 10.0f}, {"FootR", 8.0f}, {"KneeR", 0.0f}};
    float delta = 0.2f;
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

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < blocksNames.Length; i++){
            GameObject go = new GameObject(blocksNames[i]);
            go.AddComponent<Block>();
        }
 
    }


    void CreateLegs(Matrix4x4 attachedI, float axisSide, string side, float rotLegs, float rotWalk, float rotKnee, float rotFoot){
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

        //LEFT LEG
        rotsLeg["AllLegL"] += dirLeft[0] * delta;
        if (rotsLeg["AllLegL"] > anglesLeg["MAllLeg"] || rotsLeg["AllLegL"] < anglesLeg["mAllLeg"]) dirLeft[0] = -dirLeft[0];
        rotsLeg["WalkL"] += dirLeft[1] * delta;
        if (rotsLeg["WalkL"] > anglesLeg["MAllLeg"] || rotsLeg["WalkL"] < anglesLeg["mAllLeg"]) dirLeft[1] = -dirLeft[1];
        rotsLeg["KneeL"] += dirLeft[2] * delta;
        if (rotsLeg["KneeL"] > anglesLeg["MKnee"] || rotsLeg["KneeL"] < anglesLeg["mKnee"]) dirLeft[2] = -dirLeft[2];
        rotsLeg["FootL"] += dirLeft[3] * delta;
        if (rotsLeg["FootL"] > anglesLeg["MFoot"] || rotsLeg["FootL"] < anglesLeg["mFoot"]) dirLeft[3] = -dirLeft[3];
        CreateLegs(hipsI, -1.0f, "Left", rotsLeg["AllLegL"], rotsLeg["WalkL"] , rotsLeg["KneeL"], rotsLeg["FootL"]);

        //RIGHT LEG
        rotsLeg["AllLegR"] += dirRight[0] * delta;
        if (rotsLeg["AllLegR"] > anglesLeg["MAllLeg"] || rotsLeg["AllLegR"] < anglesLeg["mAllLeg"]) dirRight[0] = -dirRight[0];
        rotsLeg["WalkR"] += dirRight[1] * delta;
        if (rotsLeg["WalkR"] > anglesLeg["MAllLeg"] || rotsLeg["WalkR"] < anglesLeg["mAllLeg"]) dirRight[1] = -dirRight[1];
        rotsLeg["KneeR"] += dirRight[2] * delta;
        if (rotsLeg["KneeR"] > anglesLeg["MKnee"] || rotsLeg["KneeR"] < anglesLeg["mKnee"]) dirRight[2] = -dirRight[2];
        rotsLeg["FootR"] += dirRight[3] * delta;
        if (rotsLeg["FootR"] > anglesLeg["MFoot"] || rotsLeg["FootR"] < anglesLeg["mFoot"]) dirRight[3] = -dirRight[3];
        CreateLegs(hipsI, 1.0f, "Right", rotsLeg["AllLegR"], rotsLeg["WalkR"] , rotsLeg["KneeR"], rotsLeg["FootR"]);
    }
}