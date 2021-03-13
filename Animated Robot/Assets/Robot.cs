//EQUIPO BICHOTAS
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Robot : MonoBehaviour
{   
    //Array to  name all the game objects created and later on, found them by this name.
    string[] blocksNames = {"Hips", "Torso", "Neck", "Head", 
                            /*Arms*/ 
                            "ShoulderLeft", "ShoulderRight", "BicepLeft", "BicepRight",
                            "ElbowLeft", "ElbowRight", "ForearmLeft", "ForearmRight", "HandLeft", "HandRight",
                             /*Legs*/
                            "ThighLeft", "ThighRight", "KneeLeft", "KneeRight", "LegLeft", "LegRight", 
                            "FootLeft", "FootRight"};

    void ApplyTransformations(Matrix4x4 t, GameObject target){
            
        Block b = target.GetComponent<Block>();
        Vector3[] newVerts = new Vector3[8];
        for(int v = 0; v < b.vertices.Length; v++){
            Vector4 temp = b.vertices[v];
            temp.w = 1;
            newVerts[v] = t * temp;
        }

        b.myMesh.vertices = newVerts;
    }
    //Single method for aplying translation, scale and then aplying transformations
    void ModelBlock(string blockToCreate , Vector3 toScale, Vector3 toTranslate){
        Matrix4x4 m = Transformations.ScaleM(toScale.x, toScale.y, toScale.z);
        Matrix4x4 t = Transformations.TranslateM(toTranslate.x, toTranslate.y, toTranslate.z);
        m = t * m;
        ApplyTransformations(m, GameObject.Find(blockToCreate));
    }

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i<blocksNames.Length; i++){
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
        ModelBlock("Hips",  hipsSize /*Scale*/, new Vector3(0, 0, 0) /*Translate*/);
        //TORSO 
        Vector3 torsoSize = new Vector3(1, 1, 0.5f);
        ModelBlock("Torso", torsoSize /*Scale*/, new Vector3(0, 0.7f, 0)/*Translate*/);
        // NECK
        Vector3 neckSize = new Vector3(0.15f, 0.20f, 0.15f);
        ModelBlock("Neck", neckSize /*Scale*/, new Vector3(0, 1.3f, 0) /*Translate*/);
        // HEAD
        Vector3 headSize = new Vector3(0.4f, 0.4f, 0.4f);
        ModelBlock("Head", headSize /*Scale*/, new Vector3(0, 1.5f, 0) /*Translate*/);
        // LEGS
            // ThighRight
        Vector3 thighSize = new Vector3(0.5f, 1.0f, 0.45f);
        ModelBlock("ThighRight",thighSize /*Scale*/, new Vector3(0.25f, -0.7f, 0f) /*Translate*/);
            //ThighLeft
        ModelBlock("ThighLeft",thighSize/*Scale*/, new Vector3(-0.25f, -0.7f, 0f) /*Translate*/);
        
            //KneeRight
        Vector3 kneeSize = new Vector3(0.5f, 0.4f, 0.45f);
        ModelBlock("KneeRight",kneeSize /*Scale*/, new Vector3(0.25f, -1.5f, 0) /*Translate*/);
            //KneeLeft
        ModelBlock("KneeLeft",kneeSize /*Scale*/, new Vector3(-0.25f, -1.5f, 0) /*Translate*/);

            //LegRight
        Vector3 legSize = new Vector3(0.5f, 1.0f, 0.45f);
        ModelBlock("LegRight",legSize /*Scale*/, new Vector3(0.25f, -2.3f, 0) /*Translate*/);
            //LegLeft
        ModelBlock("LegLeft",legSize /*Scale*/, new Vector3(-0.25f, -2.3f, 0) /*Translate*/);

            //FootRight
        Vector3 footSize = new Vector3(0.5f, 0.5f, 1.0f);
        ModelBlock("FootRight",footSize /*Scale*/, new Vector3(0.25f, -3.2f, 0.3f) /*Translate*/);
            //FootLeft
        ModelBlock("FootLeft",footSize /*Scale*/, new Vector3(-0.25f, -3.2f, 0.3f) /*Translate*/);

        //ARM
            //ShoulderRight
        Vector3 shoulderSize = new Vector3(0.4f, 0.4f, 0.4f);
        ModelBlock("ShoulderRight",shoulderSize /*Scale*/, new Vector3(0.75f, 1.0f, 0) /*Translate*/);
            //ShoulderLeft
        ModelBlock("ShoulderLeft",shoulderSize /*Scale*/, new Vector3(-0.75f, 1.0f, 0) /*Translate*/);

            //BicepRight
        Vector3 bicepSize = new Vector3(1, 0.3f, 0.3f);
        ModelBlock("BicepRight",bicepSize /*Scale*/, new Vector3(1.50f, 1.0f, 0) /*Translate*/);
            //BicepLeft
        ModelBlock("BicepLeft",bicepSize /*Scale*/, new Vector3(-1.50f, 1.0f, 0) /*Translate*/);

            //ElbowRight
        Vector3 elbowSize = new Vector3(0.2f, 0.2f, 0.2f);
        ModelBlock("ElbowRight",elbowSize /*Scale*/, new Vector3(2.15f, 1.0f, 0) /*Translate*/);
            //ElbowLeft
        ModelBlock("ElbowLeft",elbowSize /*Scale*/, new Vector3(-2.15f, 1.0f, 0) /*Translate*/);

            //ForearmRight
        Vector3 forearmSize = new Vector3(0.6f, 0.3f, 0.3f);
        ModelBlock("ForearmRight",forearmSize /*Scale*/, new Vector3(2.35f, 1.0f, 0) /*Translate*/);
            //ForearmLeft
        ModelBlock("ForearmLeft",forearmSize /*Scale*/, new Vector3(-2.35f, 1.0f, 0) /*Translate*/);

            //HandRight
        Vector3 handSize = new Vector3(0.2f, 0.2f, 0.2f);
        ModelBlock("HandRight",handSize /*Scale*/, new Vector3(2.8f, 1.0f, 0) /*Translate*/);
            //HandLeft
        ModelBlock("HandLeft",handSize /*Scale*/, new Vector3(-2.8f, 1.0f, 0) /*Translate*/);

    }
}
