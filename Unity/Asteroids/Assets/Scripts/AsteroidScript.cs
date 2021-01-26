using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidScript : MonoBehaviour
{
	public float maxThrust;
	public float maxTorque;
	public Rigidbody2D rb;
	public float screenTop;
	public float screenBottom;
	public float screenLeft;
	public float screenRight;
	public int asteroidSize; //3-Large, 2- Midium, 1-Small
	public GameObject asteroidMedium;
	public GameObject asteroidSmall;
	public int points;
	public GameObject player;
	public GameObject explosion;
	
	public GameManager gm;
	
    // Start is called before the first frame update
    void Start()
    {
		//Add a random amount of torque and thrust to the asteroid 
		Vector2 thrust = new Vector2(Random.Range(-maxThrust,maxThrust),Random.Range(-maxThrust,maxThrust));
		float torque = Random.Range(-maxTorque,maxTorque);
		
		rb.AddForce(thrust);
		rb.AddTorque(torque);
        
		//Find the player
		player = GameObject.FindWithTag("Player");
		//Find the Game Manager
		gm = GameObject.FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
		//Screen Wraping
        Vector2 newPos=transform.position;
		if(transform.position.y > screenTop){
			newPos.y = screenBottom;
		}
		if(transform.position.y < screenBottom){
			newPos.y = screenTop;
		}
		if(transform.position.x > screenRight){
			newPos.x = screenLeft;
		}
		if(transform.position.x < screenLeft){
			newPos.x = screenRight;
		}
		
		transform.position = newPos;
    }
	
	void OnTriggerEnter2D(Collider2D other)
	{
		//Debug.Log("Hit");	
		//Debug.Log("Hit by" + other.name);
		//Check to see if its a bullet
		if(other.CompareTag("bullet")){
			//destroy the bullet
			Destroy (other.gameObject);
			//Check the size of asteroidsandspawn in the next smaller size
			if(asteroidSize == 3){
				//Spawn two medium asteroids
				 Instantiate(asteroidMedium, transform.position,transform.rotation);
				 Instantiate(asteroidMedium, transform.position,transform.rotation);	
								
				gm.UpdateNumberOfAsteroids(1);
			}
			else if(asteroidSize == 2){
				//Spawn 1 small asteroids
				 Instantiate(asteroidSmall, transform.position,transform.rotation);
				 Instantiate(asteroidSmall, transform.position,transform.rotation);
			
				 gm.UpdateNumberOfAsteroids(1);
			}
			else if(asteroidSize == 1){
				//Remove the asteroids
				gm.UpdateNumberOfAsteroids(-1);
			}
			//Tell the player to score some points
			player.SendMessage("ScorePoints",points);
			
			//Make an explosion
			GameObject newExplosion = Instantiate(explosion, transform.position,transform.rotation);
			Destroy(newExplosion,3f);
			//Remove the current asteroid
			Destroy(gameObject);
		}
	}
}
