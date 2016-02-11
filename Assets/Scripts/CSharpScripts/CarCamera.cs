using UnityEngine;
using System.Collections;

public class CarCamera : MonoBehaviour
{
    //目标位置
    public Transform target = null;
    //摄像机在目标位置上方的距离
    public float height = 1f;
    //
	public float positionDamping = 3f;
    //差值目标对象的刚体运动速度时的系数
	public float velocityDamping = 3f;
	public float distance = 4f;
    
	private RaycastHit hit = new RaycastHit();
    
    //射线探测时忽略的层
    public LayerMask ignoreLayers = -1;
    //射线探测时接收的层
    private LayerMask raycastLayers = -1;

    //目标对象刚体组件前一帧的运动速度
    private Vector3 prevVelocity = Vector3.zero;
    //目标对象刚体组件当前帧的运动速度
    private Vector3 currentVelocity = Vector3.zero;
	
	void Start()
	{
        //初始化raycastLayers为除ignoreLayers之外的所有层
        raycastLayers = ~ignoreLayers;
	}

	void FixedUpdate()
	{
		currentVelocity = Vector3.Lerp(prevVelocity, target.root.rigidbody.velocity, velocityDamping * Time.deltaTime);
		currentVelocity.y = 0;
		prevVelocity = currentVelocity;
	}
	
	void LateUpdate()
	{
		float speedFactor = Mathf.Clamp01(target.root.rigidbody.velocity.magnitude / 70.0f);
		camera.fieldOfView = Mathf.Lerp(55, 72, speedFactor);
		float currentDistance = Mathf.Lerp(7.5f, 6.5f, speedFactor);
		
		currentVelocity = currentVelocity.normalized;
		
		Vector3 newTargetPosition = target.position + Vector3.up * height;
		Vector3 newPosition = newTargetPosition - (currentVelocity * currentDistance);
		newPosition.y = newTargetPosition.y;
		
		Vector3 targetDirection = newPosition - newTargetPosition;
		if(Physics.Raycast(newTargetPosition, targetDirection, out hit, currentDistance, raycastLayers))
			newPosition = hit.point;
		
		transform.position = newPosition;
		transform.LookAt(newTargetPosition);
		
	}
}
