using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;

public class Controllers : MonoBehaviour
{
	public List <Transform> waypoints = new List <Transform>();
	public List <Transform> cars = new List <Transform>();
	
	private Transform targetWaypoint;
	private int targetWaypointIndex = 5;
	private int lastWaypointIndex;
	private float minDistance = 0.1f;
	
	//private float speed = 3.0f;
	public int[,] FSMC = new int[19, 19]; // Modelo de Markov - via la matriz Adjunta
	
	private float rotationSpeed = 4.0f;
	
	//private float speed = 0.0f;
	
	// Start is called before the first frame update
	void Start()
	{

		for (int i = 0; i < 18; i++)
		{
	  	for (int j = 0; j < 18; j++)
			{
	  		FSMC[i,j] = 0;
			}	
	  }

		FSMC[3,5] = 1; // del nodo 4-1 al nodo 6-1 es valido el movimiento
		FSMC[5,6] = 1; // del nodo 1 al nodo 2 es valido el movimiento
		FSMC[6,7] = 1; // del nodo 2 al nodo 3 es valido el movimiento
		FSMC[7,9] = 1; // del nodo 3 al nodo 0 es valido el movimiento
		FSMC[9,10] = 1;
		FSMC[10,11] = 1;
		FSMC[11, 13] = 1;
		FSMC[13, 14] = 1;
		FSMC[14, 15] = 1;
		FSMC[15, 18] = 1;
		FSMC[7, 8] = 1;
		FSMC[8, 16] = 1;
		FSMC[16, 17] = 1;
		FSMC[17, 3] = 1;

		lastWaypointIndex = waypoints.Count -1;
		targetWaypoint = waypoints[targetWaypointIndex];
		
	}

	// Update is called once per frame
	void Update()
	{
		float movementStep = UnityEngine.Random.Range(10.0f, 15.0f) * Time.deltaTime;
		float rotationStep = rotationSpeed * Time.deltaTime;
		
		Vector3 direction2target = targetWaypoint.position - transform.position;
		
		Quaternion rotation2target = Quaternion.LookRotation(direction2target);
		
		transform.rotation = Quaternion.Slerp(transform.rotation, rotation2target, rotationStep);
		
		Debug.DrawRay(transform.position, transform.forward * 10f, Color.green, 0f);
		Debug.DrawRay(transform.position, direction2target, Color.red, 0f);
		
		float currentDistance = Vector3.Distance(transform.position, targetWaypoint.position);
		
		CheckDistance2Waypont(currentDistance);
		
		//Debug.Log("Distance: " + currentDistance);
		Debug.Log("Cars: " + cars.Count);
		
		transform.position = Vector3.MoveTowards(transform.position, targetWaypoint.position, movementStep);
	}
	
	void CheckDistance2Waypont(float currentDistance)
	{
		
		if( currentDistance <= minDistance)
		{
			int[] numbers = new [] {0, 1, 2, 3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18}; // Esta son las opciones de los nodos posibles
			int[] shuffled = numbers.OrderBy(n => Guid.NewGuid()).ToArray(); // Presento las opciones de manera aleatoria
			
			for(int i = 0; i <= 3; i++) // itero las opciones del agente
			{
				if( FSMC[targetWaypointIndex, shuffled[i]] == 1){ // En el momento que una opcion sea valida, la tomo
					targetWaypointIndex = shuffled[i]; // Asignamiento
					break; // Rompo el proceso dado que ya acepte una
				}
			}

			UpdateTargetWaypoint();
		}
	}
	
	void UpdateTargetWaypoint()
	{
		
		if( targetWaypointIndex >  lastWaypointIndex ){
			targetWaypointIndex = 3;
		}
	
		targetWaypoint = waypoints[targetWaypointIndex];
	
	}
	
}
