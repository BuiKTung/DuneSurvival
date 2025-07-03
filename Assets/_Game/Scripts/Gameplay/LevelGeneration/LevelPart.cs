using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace _Game.Scripts.Gameplay.LevelGeneration
{
    public class LevelPart : MonoBehaviour
    {
        public Enemy.Enemy[] MyEnemies() => GetComponentsInChildren<Enemy.Enemy>(true);
        
        [Header("Intersection check")]
        [SerializeField] private LayerMask intersectionLayerMask;
        [SerializeField] private Collider[] intersectionCheckColliders;
        [SerializeField] private Transform intersectionCheckParent;

        private void Start()
        {
            if (intersectionCheckColliders.Length <= 0)
            {
                intersectionCheckColliders = intersectionCheckParent.GetComponentsInChildren<Collider>();
            }
        }
        public bool IntersectionDetected()
        {
            Physics.SyncTransforms();

            foreach (var collider in intersectionCheckColliders)
            {
                Collider[] hitColliders = 
                    Physics.OverlapBox(collider.bounds.center, collider.bounds.extents, quaternion.identity, intersectionLayerMask);

                foreach (var hit in hitColliders)
                {
                     IntersectionCheck interectionCheck = hit.GetComponentInParent<IntersectionCheck>();
                     
                     if(interectionCheck != null && intersectionCheckParent != interectionCheck.transform)
                         return true;
                }
            }
            return false;
        }
        public void SnapAndAlignPartTo(SnapPoint target)
        {
            SnapPoint enhancedSnapPoint = GetEnhancedPoint();
            
            AlignTo(enhancedSnapPoint, target);
            SnapTo(enhancedSnapPoint, target);
        }

        private void AlignTo(SnapPoint ownSnapPoint, SnapPoint targetSnapPoint)
        {
            // Calculate the rotation offset beetwen the level part's current rotation
            // and it's own snap point's rotation. This help in fine-tuning the alignment later.
            var rotationOffset =
                ownSnapPoint.transform.rotation.eulerAngles.y - transform.rotation.eulerAngles.y;

            // Set the level part's rotation to match the target snap point's rotation.
            // This is the initial step to align the orientations of the two parts.
            transform.rotation = targetSnapPoint.transform.rotation;

            // Rotate the level part by 180 degrees around the Y-axis.This is necessary becasue the snap points
            // are typically facing opposite directions, and this rotation aligns them to face each other correctly.
            transform.Rotate(0, 180, 0);

            // Apply the previusly calculated offset. This step fine-tunes the alignment by adjusting the
            // level part's rotation to account for any initial difference in orientation beetwen the level
            // part's own snap point and the main body of the part.
            transform.Rotate(0, -rotationOffset, 0);
        }
        private void SnapTo(SnapPoint ownSnapPoint, SnapPoint targetSnapPoint)
        {
            // Calculate the offset beetwen the level part's current position
            // and it's own snap point's position. This offset represents the
            // distance and direction from the level part's pivot to its snap point.
            var offset = transform.position - ownSnapPoint.transform.position;

            // Determnine the new position for the level part. It's calculated by 
            // adding the previously co,puted offset to the target snap point's position.
            // This effectively moves the level part so that its snap point aligns 
            // with the target snap point's position.
            var newPosition = targetSnapPoint.transform.position + offset;

            // Update the level part's position to the newly calculated position by using snap points.
            transform.position = newPosition;
        }
        
        public SnapPoint GetEnhancedPoint() => GetSnapPointOfType(SnapPointType.Enter);
        public SnapPoint GetExitPoint() => GetSnapPointOfType(SnapPointType.Exit);
        private SnapPoint GetSnapPointOfType(SnapPointType snapPointType)
        {
            SnapPoint[] snapPoints = GetComponentsInChildren<SnapPoint>();
            List<SnapPoint> filteredSnapPoints = new List<SnapPoint>();

            // Collect all snap points of the spesified type
            foreach (SnapPoint snapPoint in snapPoints)
            {
                if (snapPoint.pointType == snapPointType)
                {
                    filteredSnapPoints.Add(snapPoint);
                }
            }
            // If there are matching snap points, choose one at random
            if (filteredSnapPoints.Count > 0)
            {
                int randomIndex = Random.Range(0, filteredSnapPoints.Count);
                return filteredSnapPoints[randomIndex];
            }
            // Return null if no matching snap points are found
            return null;
        }
        [ContextMenu("Set static to envoirment layer")]
        private void AdjustLayerForStaticObjcets()
        {
            foreach (Transform childTransorm in transform.GetComponentsInChildren<Transform>(true))
            {
                if (childTransorm.gameObject.isStatic)
                {
                    childTransorm.gameObject.layer = LayerMask.NameToLayer("Environment");
                }
            }
        }
    }
}
