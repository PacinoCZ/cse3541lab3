using System;
using UnityEngine;

public class Particle : MonoBehaviour {

	public Vector3 Position;
	public Vector3 Velocity;
	public Vector3 Force;
	public TimeSpan Age;
	public TimeSpan MaxAge;
	public float Mass = 1.0f;
	public GameObject Apperance;

	private Vector3 acceleration;

	// Use this for initialization
	void Start () {
		acceleration = Force / Mass;
	}
	
	// Update is called once per frame
	void Update () {
		float deltaTime = Time.deltaTime;
		this.Position = CalculateNewPosition(deltaTime);
		this.Velocity = CalculateNewVelocity(deltaTime);
		this.transform.position = this.Position;
	}

	private Vector3 CalculateNewPosition(float deltaTime)
	{
		return this.Position + CalculateNewVelocity(deltaTime / 2) * deltaTime;
	}

	private Vector3 CalculateNewVelocity(float deltaTime)
	{
		return this.Velocity + this.acceleration * deltaTime;
	}
	
	private void CollisionDetection(){
		
		Matrix4x4 localToWorld = transform.localToWorldMatrix;
		Vector3[] local_vertices = Apperance.GetComponent<MeshFilter>().mesh.vertices;
		Vector3[] world_vertices = new Vector3[local_vertices.Length];
		for (int i = 0; i < local_vertices.Length; i++) {
			world_vertices [i] = localToWorld.MultiplyPoint3x4(local_vertices[i]);
		}

		float sum_x = 0f, sum_y = 0f, sum_z = 0f;
		for (int i = 0; i < local_vertices.Length; i++) {
			sum_x += world_vertices [i].x;
			sum_y += world_vertices [i].y;
			sum_z += world_vertices [i].z;
		}

		Vector3 center = new Vector3 (sum_x / world_vertices.Length, sum_y / world_vertices.Length, sum_z / world_vertices.Length);
		float radius = 0f;
		for (int i = 0; i < world_vertices.Length; i++) {
			float distance = Mathf.Pow ((world_vertices [i].x - center.x), 2) + Mathf.Pow ((world_vertices [i].y - center.y), 2) + Mathf.Pow ((world_vertices [i].z - center.z), 2);
			if (distance > radius)
				radius = distance;
		}

	}

}
