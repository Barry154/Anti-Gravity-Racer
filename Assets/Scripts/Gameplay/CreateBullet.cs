// This script instantiates a game object when the correct player input is pressed (creates missiles at the location of the vehicle's weapon)

using UnityEngine;

public class CreateBullet : MonoBehaviour
{
    [SerializeField] public GameObject bullet;
    [SerializeField] PlayerInput playerInput;

    // Update is called once per frame
    void Update()
    {
        if (playerInput.fireWeapon)
        {
            // Instantiates/Spawns/Creates a 'bullet' object when the specific input is pressed, called from the PlayerInput script,
            // at the position and rotation of the object on which this script is attached (vehicle weapon in this case)
            Instantiate(bullet, transform.position, transform.rotation);
        } 
    }
}
