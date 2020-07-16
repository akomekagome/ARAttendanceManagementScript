using UnityEngine;

namespace ARAM.Utils
{
    public static class PhysicsExtenstion
    {
        public static int OverlapCapsuleColliderNonAlloc(CapsuleCollider capsuleCollider, Vector3 origin, ref Collider[] results)
        {
            return Physics.OverlapCapsuleNonAlloc(
                origin + capsuleCollider.center + capsuleCollider.height * GetAxis(capsuleCollider.direction),
                origin + capsuleCollider.center + capsuleCollider.height * -GetAxis(capsuleCollider.direction),
                capsuleCollider.radius,
                results
            );
        }
        
        public static int OverlapCapsuleColliderNonAlloc(CapsuleCollider capsuleCollider, Vector3 origin, float radius, ref Collider[] results)
        {
            return Physics.OverlapCapsuleNonAlloc(
                origin + capsuleCollider.center + capsuleCollider.height * GetAxis(capsuleCollider.direction),
                origin + capsuleCollider.center + capsuleCollider.height * -GetAxis(capsuleCollider.direction),
                radius,
                results
            );
        }
        
        public static Collider[] OverlapCapsuleCollider(CapsuleCollider capsuleCollider, Vector3 origin)
        {
            return Physics.OverlapCapsule(
                origin + capsuleCollider.center + capsuleCollider.height * GetAxis(capsuleCollider.direction),
                origin + capsuleCollider.center + capsuleCollider.height * -GetAxis(capsuleCollider.direction),
                capsuleCollider.radius
            );
        }
        
        public static Collider[] OverlapCapsuleCollider(CapsuleCollider capsuleCollider, Vector3 origin, float radius)
        {
            return Physics.OverlapCapsule(
                origin + capsuleCollider.center + capsuleCollider.height * GetAxis(capsuleCollider.direction),
                origin + capsuleCollider.center + capsuleCollider.height * -GetAxis(capsuleCollider.direction),
                radius
            );
        }
        
        public static int OverlapCharacterControllerNonAlloc(CharacterController cc, Vector3 origin, ref Collider[] results)
        {
            return Physics.OverlapCapsuleNonAlloc(
                origin + cc.center + cc.height * Vector3.right,
                origin + cc.center + cc.height * -Vector3.right,
                cc.radius,
                results
            );
        }
        
        public static int OverlapCharacterControllerNonAlloc(CharacterController cc, Vector3 origin, float radius, ref Collider[] results)
        {
            return Physics.OverlapCapsuleNonAlloc(
                origin + cc.center + cc.height * Vector3.right,
                origin + cc.center + cc.height * -Vector3.right,
                radius,
                results
            );
        }
        
        public static Collider[] OverlapCharacterController(CharacterController cc, Vector3 origin)
        {
            return Physics.OverlapCapsule(
                origin + cc.center + cc.height * Vector3.right,
                origin + cc.center + cc.height * -Vector3.right,
                cc.radius
            );
        }
        
        public static Collider[] OverlapCharacterController(CharacterController cc, Vector3 origin, float radius)
        {
            return Physics.OverlapCapsule(
                origin + cc.center + cc.height * Vector3.right,
                origin + cc.center + cc.height * -Vector3.right,
                radius
            );
        }

        private static Vector3 GetAxis(int direction)
        {
            switch (direction)
            {
                case 0:
                    return Vector3.right;
                case 1:
                    return Vector3.up;
                case 2:
                    return Vector3.forward;
            }

            return default;
        }
    }
}