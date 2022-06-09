using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    [Header("카메라 기본속성")]

    private Transform cameraTransform = null;

    
    public GameObject objTarget = null;

    
    private Transform objTargetTransform = null;

    
    public enum CameraTypeState { First, Second, Third }

   
    public CameraTypeState cameraTypeState = CameraTypeState.Third;

    [Header("3인칭 카메라")]
    
    public float distance = 6.0f;
    
    public float height = 1.75f;


    public float heightDamping = 2.0f;

    public float rotationDamping = 3.0f;

    [Header("2인칭 카메라")]

    public float rotationSpd = 10.0f;

    [Header("1인칭 카메라")]

    public float detailX = 5.0f;
    public float detailY = 5.0f;

   
    public float rotationX = 0.0f;
    public float rotationY = 0.0f;


    public Transform posfirstCameraTarget = null;

    // Start is called before the first frame update
    void Start()
    {
        
        cameraTransform = GetComponent<Transform>();

       
        if (objTarget != null)
        {
           
            objTargetTransform = objTarget.transform;
        }
    }

    /// <summary>
    /// 3인칭 카메라 함수
    /// </summary>
    void ThirdCamera()
    {
       
        float objTargetRotationAngle = objTargetTransform.eulerAngles.y;
        float objHeight = objTargetTransform.position.y + height;
        float nowRotationAngle = cameraTransform.eulerAngles.y;
      
        float nowHeight = cameraTransform.position.y;

        
        nowRotationAngle = Mathf.LerpAngle(nowRotationAngle, objTargetRotationAngle, rotationDamping * Time.deltaTime);
        
        nowHeight = Mathf.Lerp(nowHeight, objHeight, heightDamping * Time.deltaTime);

        
        Quaternion nowRotation = Quaternion.Euler(0f, nowRotationAngle, 0f);


        cameraTransform.position = objTargetTransform.position;


        cameraTransform.position -= nowRotation * Vector3.forward * distance;


        cameraTransform.position = new Vector3(cameraTransform.position.x, nowHeight, cameraTransform.position.z);

        
        cameraTransform.LookAt(objTargetTransform);
    }

    /// <summary>
    /// 2인칭 카메라 조작 함수
    /// </summary>
    void SecondCamera()
    {
       
        cameraTransform.RotateAround(objTargetTransform.position, Vector3.up, rotationSpd * Time.deltaTime);

       
        cameraTransform.LookAt(objTargetTransform);

    }

    /// <summary>
    /// 1인칭 카메라 조작 함수
    /// </summary>
    void FirstCamera()
    {
        
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        
        rotationX = cameraTransform.localEulerAngles.y + mouseX * detailX;

        
        rotationX = (rotationX > 180.0f) ? rotationX - 360.0f : rotationX;

       
        rotationY = rotationY + mouseY * detailY;
       
        rotationY = (rotationY > 180.0f) ? rotationY - 360.0f : rotationY;

        
        cameraTransform.localEulerAngles = new Vector3(-rotationY, rotationX, 0f);
        
        cameraTransform.position = posfirstCameraTarget.position;
    }

   
    private void LateUpdate()
    {
       
        if (objTarget == null)
        {
            return;
        }

       
        if (objTargetTransform == null)
        {
            objTargetTransform = objTarget.transform;
        }

   
        switch (cameraTypeState)
        {
           
            case CameraTypeState.Third:
                ThirdCamera();
                break;
          
            case CameraTypeState.Second:
                SecondCamera();
                break;
            
            case CameraTypeState.First:
                FirstCamera();
                break;
        }
    }
}
