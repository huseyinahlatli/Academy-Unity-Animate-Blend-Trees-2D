using Unity.VisualScripting;
using UnityEngine;

public class TwoDimensionalAnimationStateController : MonoBehaviour
{
    [SerializeField] private float acceleration = 2.0f;
    [SerializeField] private float deceleration = 2.0f;
    [SerializeField] private float maxWalkVelocity = 0.5f;
    [SerializeField] private float maxRunVelocity = 2.0f;

    private static readonly int VelocityXHash = Animator.StringToHash("Velocity X");
    private static readonly int VelocityZHash = Animator.StringToHash("Velocity Z");
    private float _velocityX = 0.0f;
    private float _velocityZ = 0.0f;
    private Animator _animator;
    
    private void Start()
    {
        _animator = GetComponent<Animator>();
    }
    
    private void Update()
    {
        var forwardPressed = Input.GetKey(KeyCode.W);
        var leftPressed = Input.GetKey(KeyCode.A);
        var rightPressed = Input.GetKey(KeyCode.D);
        var runPressed = Input.GetKey(KeyCode.LeftShift);
        
        var currentMaxVelocity = runPressed ? maxRunVelocity : maxWalkVelocity; // set current MaxVelocity
        
        ChangeVelocity(forwardPressed, leftPressed, rightPressed, runPressed, currentMaxVelocity);
        LockOrResetVelocity(forwardPressed, leftPressed, rightPressed, runPressed, currentMaxVelocity);

        _animator.SetFloat(VelocityZHash, _velocityZ);
        _animator.SetFloat(VelocityXHash, _velocityX);
    }

    private void ChangeVelocity(bool forwardPressed, bool leftPressed, bool rightPressed, bool runPressed, float currentMaxVelocity)
    {
        if (forwardPressed && _velocityZ < currentMaxVelocity) // if player presses forward, increase velocity in z direction
            _velocityZ += Time.deltaTime * acceleration;
        
        if (leftPressed && _velocityX > -currentMaxVelocity) // increase velocity in left direction
            _velocityX -= Time.deltaTime * acceleration;
         
        if (rightPressed && _velocityX < currentMaxVelocity) // increase velocity in right direction
            _velocityX += Time.deltaTime * acceleration;

        if (!forwardPressed && _velocityZ > 0.0f) // decrease velocityZ
            _velocityZ -= Time.deltaTime * deceleration;
        
        if (!leftPressed && _velocityX < 0.0f) // increase velocityX if left is not pressed and velocityX < 0
            _velocityX += Time.deltaTime * deceleration;

        if (!rightPressed && _velocityX > 0.0f) // decrease velocityX if left is not pressed and velocityX > 0
            _velocityX -= Time.deltaTime * deceleration; 
    }

    private void LockOrResetVelocity(bool forwardPressed, bool leftPressed, bool rightPressed, bool runPressed, float currentMaxVelocity)
    {
        if (!forwardPressed && _velocityZ < 0.0f) // reset velocityZ
            _velocityZ = 0.0f;
        
        if (!leftPressed && !rightPressed && _velocityX != 0.0f && _velocityX is > -0.05f and < 0.05f) // reset velocityX
            _velocityX = 0.0f;
        
        if (forwardPressed && runPressed && _velocityZ > currentMaxVelocity) // lock forward
            _velocityZ = currentMaxVelocity;
        
        else if (forwardPressed && _velocityZ > currentMaxVelocity) // decelerate to the maximum walk velocity
        {
            _velocityZ -= Time.deltaTime * deceleration;
            
            if (_velocityZ > currentMaxVelocity && _velocityZ < currentMaxVelocity + 0.05f) // round to the current MaxVelocity if within offset
                _velocityZ = currentMaxVelocity;
        }
        
        else if (forwardPressed && _velocityZ < currentMaxVelocity && _velocityZ > currentMaxVelocity - 0.05f) // round to the current MaxVelocity if within offset
            _velocityZ = currentMaxVelocity;
        
        if (leftPressed && runPressed && _velocityX < -currentMaxVelocity) // Locking left
            _velocityX = -currentMaxVelocity;
        
        else if (leftPressed && _velocityX < -currentMaxVelocity) // decelerate to the maximum walk velocity
        {
            _velocityX += Time.deltaTime * deceleration;
            
            if (_velocityX < -currentMaxVelocity && _velocityX > -currentMaxVelocity - 0.05f) // round to the current MaxVelocity if within offset
                _velocityX = -currentMaxVelocity;
        }
       
        else if (leftPressed && _velocityX > -currentMaxVelocity && _velocityX < -currentMaxVelocity + 0.05f)  // round to the currentMaxVelocity if within offset
            _velocityX = -currentMaxVelocity;
        
        if (rightPressed && runPressed && _velocityX > currentMaxVelocity) // locking right
            _velocityX = currentMaxVelocity;
       
        else if (rightPressed && _velocityX > currentMaxVelocity) // decelerate to the maximum walk velocity
        {
            _velocityX -= Time.deltaTime * deceleration;
            
            if (_velocityX > currentMaxVelocity && _velocityX < currentMaxVelocity + 0.05f) // round to the currentMaxVelocity if within offset
                _velocityX = currentMaxVelocity;
        }
        
        else if (rightPressed && _velocityX < currentMaxVelocity && _velocityX > currentMaxVelocity - 0.05f) // round to the currentMaxVelocity if within offset
            _velocityX = currentMaxVelocity;
    }
}
