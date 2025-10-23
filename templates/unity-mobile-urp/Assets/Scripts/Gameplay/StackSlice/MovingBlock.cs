using UnityEngine;
public class MovingBlock : MonoBehaviour
{
    private float _speed = 1.6f; private int _dir = 1; private bool _stopped=false;
    public void SetSpeed(float s){ _speed=s; } public void StopMovement(){ _stopped=true; } public void ResumeMovement(){ _stopped=false; }
    void Update()
    {
        if (_stopped) return;
        transform.Translate(Vector3.right * _dir * _speed * Time.deltaTime);
        if (transform.position.x > 3f) _dir = -1;
        if (transform.position.x < -3f) _dir = 1;
    }
}