using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour {
    
    public Transform target;//跟隨目標
	public float rotX;
	public float rotY;
	public float sensitivity = 30f;//靈敏度
    public float distence;//當前攝影機與主角的距離;
    public float disSpeed = 20f;//滾輪靈敏度
    public float minDistence = 2;//攝影機與主角的最小距離
    public float maxDistence = 5;//攝影機與主角的最大距離
    private Quaternion rotationEuler;
    private Vector3 cameraPosition;
    public bool lockCursor = true;
    private bool m_cursorIsLocked = true;
    private Vector3 startposition = new Vector3(0, 1.5f, -1.5f);


    public void SetCursorLock(bool value)//鎖住鼠標
    {
        lockCursor = value;
        if (!lockCursor)
        {//we force unlock the cursor if the user disable the cursor locking helper
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    public void UpdateCursorLock()
    {
        
        //if the user set "lockCursor" we check & properly lock the cursos
        if (lockCursor)
            InternalLockUpdate();
    }

    private void InternalLockUpdate()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            m_cursorIsLocked = false;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            m_cursorIsLocked = true;
        }

        if (m_cursorIsLocked)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else if (!m_cursorIsLocked)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }





    // LateUpdate is called once per frame after other Update
    void LateUpdate ()
    {
        //讀取滑鼠的X、Y軸移動訊息
        rotX += Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
		rotY -= Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;

        rotX += Input.GetAxis("joy3") * sensitivity * Time.deltaTime;
        rotY += Input.GetAxis("joy4") * sensitivity * Time.deltaTime;

        //保證X在360度以內
        if (rotX>360)
        {
			rotX -= 360;
        }
		else if (rotX < 0)
        {
			rotX += 360;
        }
		if (rotY > 45)
        {
			rotY = 45;
        }
		else if (rotY < -20)
        {
			rotY = -20;
        }
        //讀取滑鼠滾輪的數值
        distence -= Input.GetAxis("Mouse ScrollWheel") * disSpeed * Time.deltaTime;

        //限制距離
        distence = Mathf.Clamp(distence, minDistence, maxDistence);

        //運算攝影機座標、旋轉
        rotationEuler = Quaternion.Euler(rotY, rotX, 0);
        cameraPosition = rotationEuler * (new Vector3(0, 0,-distence)+startposition)+target.position;

        //應用
        transform.rotation = rotationEuler;
        //target.transform.localRotation = Quaternion.AngleAxis(rotX, target.transform.up);//人物轉向 但ASD不能用
        transform.position = cameraPosition;
       // UpdateCursorLock();

    }


    /*
        public float XSensitivity = 2f;
        public float YSensitivity = 2f;
        public bool clampVerticalRotation = true;
        public float MinimumX = -90F;
        public float MaximumX = 90F;
        public bool smooth;
        public float smoothTime = 5f;
        public bool lockCursor = false;

        private Quaternion m_CharacterTargetRot;
        private Quaternion m_CameraTargetRot;
        private bool m_cursorIsLocked = true;

        private void Start()
        {

            Init(transform, Camera.main.transform);
        }

        public void Init(Transform character, Transform camera)
        {
            m_CharacterTargetRot = character.localRotation;
            m_CameraTargetRot = camera.localRotation;
        }
        private void Update()
        {
            RotateView();

        }
        private void FixedUpdate()
        {
            UpdateCursorLock();
        }

        public void LookRotation(Transform character, Transform camera)
        {
            float yRot = Input.GetAxis("Mouse X") * XSensitivity;
            float xRot = Input.GetAxis("Mouse Y") * YSensitivity;

            m_CharacterTargetRot *= Quaternion.Euler(0f, yRot, 0f);
            m_CameraTargetRot *= Quaternion.Euler(-xRot, 0f, 0f);

            if (clampVerticalRotation)
                m_CameraTargetRot = ClampRotationAroundXAxis(m_CameraTargetRot);

            if (smooth)
            {
                character.localRotation = Quaternion.Slerp(character.localRotation, m_CharacterTargetRot,
                    smoothTime * Time.deltaTime);
                camera.localRotation = Quaternion.Slerp(camera.localRotation, m_CameraTargetRot,
                    smoothTime * Time.deltaTime);
            }
            else
            {
                character.localRotation = m_CharacterTargetRot;
                camera.localRotation = m_CameraTargetRot;
            }

            UpdateCursorLock();
        }
           private void RotateView()
        {
            LookRotation(transform, Camera.main.transform);
        }
          Quaternion ClampRotationAroundXAxis(Quaternion q)
        {
            q.x /= q.w;
            q.y /= q.w;
            q.z /= q.w;
            q.w = 1.0f;

            float angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan(q.x);

            angleX = Mathf.Clamp(angleX, MinimumX, MaximumX);

            q.x = Mathf.Tan(0.5f * Mathf.Deg2Rad * angleX);

            return q;
        }

         */
}
