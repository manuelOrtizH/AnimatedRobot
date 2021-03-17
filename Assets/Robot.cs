//EQUIPO BICHOTAS
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Robot : MonoBehaviour
{
    float dir = 1.0f;    // dir can only be 1 or -1
    float delta = 0.05f;// how much to change rotation on each frame
    float minAngle = -25.0f;  // minimum rotation angle in Z
    float maxAngle = 20.0f;// maximum rotation angle in Z
    float rotLegs = 0.0f;

    //rotation arms (shoulders)


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

    void CreateLegs(Matrix4x4 attachedI, float axisSide, string side){
        Matrix4x4 thighT = Transformations.TranslateM(0.30f*axisSide, -0.65f, 0);
        Matrix4x4 thighS = Transformations.ScaleM(0.4f,0.8f, 0.5f);
        Matrix4x4 thighI = attachedI * thighT;
        Matrix4x4 thighM = thighI * thighS;
        ApplyTransformations(thighM, GameObject.Find(string.Concat("Thigh", side))); //Concat to get ThighRight or ThighLeft
        
        Matrix4x4 kneeT = Transformations.TranslateM(0.0f*axisSide, -0.60f, 0);
        Matrix4x4 kneeS = Transformations.ScaleM(0.4f, 0.4f, 0.5f);
        Matrix4x4 kneeI = thighI * kneeT;
        Matrix4x4 kneeM = kneeI * kneeS;
        ApplyTransformations(kneeM, GameObject.Find(string.Concat("Knee", side))); //Concat to get KneeRight or KneeLeft

        Matrix4x4 legT = Transformations.TranslateM(0.0f*axisSide, -0.70f, 0);
        Matrix4x4 legS = Transformations.ScaleM(0.4f, 1.0f, 0.5f);
        Matrix4x4 legI = kneeI * legT;
        Matrix4x4 legM = legI * legS;
        ApplyTransformations(legM, GameObject.Find(string.Concat("Leg", side)));

        Matrix4x4 footT = Transformations.TranslateM(0.0f*axisSide, -0.70f, -0.25f);
        Matrix4x4 footS = Transformations.ScaleM(0.4f, 0.4f, 0.8f);
        Matrix4x4 footM = legI * footT * footS;
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
        
        // rotLegs += dir * delta;
        // if (rotLegs > maxAngle || rotLegs < minAngle) dir = -dir;
        CreateLegs(hipsI, 1.0f, "Right");
        CreateLegs(hipsI, -1.0f, "Left");

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
    }
}