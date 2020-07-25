using Shinobytes.Ravenfall.Data.Entities;
using System;

namespace Shinobytes.Ravenfall.RavenNet.Models
{
    public class Transform : Entity<Transform>
    {
        private double positionX;
        private double positionY;
        private double positionZ;
        private double rotationX;
        private double rotationY;
        private double rotationZ;

        public double PositionX { get => positionX; set => Set(ref positionX, value); }
        public double PositionY { get => positionY; set => Set(ref positionY, value); }
        public double PositionZ { get => positionZ; set => Set(ref positionZ, value); }

        public double DestinationX { get; set; }
        public double DestinationY { get; set; }
        public double DestinationZ { get; set; }

        public double RotationX { get => rotationX; set => Set(ref rotationX, value); }
        public double RotationY { get => rotationY; set => Set(ref rotationY, value); }
        public double RotationZ { get => rotationZ; set => Set(ref rotationZ, value); }

        public Vector3 GetDestination() => new Vector3((float)DestinationX, (float)DestinationY, (float)DestinationZ);
        public Vector3 GetPosition() => new Vector3((float)PositionX, (float)PositionY, (float)PositionZ);
        public Vector3 GetRotation() => new Vector3((float)RotationX, (float)RotationY, (float)RotationZ);

        public void SetPosition(Vector3 position)
        {
            this.PositionX = position.X;
            this.PositionY = position.Y;
            this.PositionZ = position.Z;
        }

        public void SetRotation(Vector3 rotation)
        {
            this.RotationX = rotation.X;
            this.RotationY = rotation.Y;
            this.RotationZ = rotation.Z;
        }

        public void SetDestination(Vector3 destination)
        {
            this.DestinationX = destination.X;
            this.DestinationY = destination.Y;
            this.DestinationZ = destination.Z;
        }
    }
}