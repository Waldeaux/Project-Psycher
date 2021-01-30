using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.testing
{
    public class PlayerData
    {
        public Vector3 Position { get; set; }
        public Quaternion Rotation { get; set; }
        public Vector3 LocalGravity { get; set; }
        public Vector3 Velocity { get; set; }
        public Vector3 AngularVelocity { get; set; }
        public int Frame { get; set; }

        public override string ToString()
        {
            return $"Position: {this.Position}\nRotation: {this.Rotation.eulerAngles}\nFrame: {this.Frame}";
        }
    }

    public struct PlayerStruct
    {
        public readonly Vector3 position;
        public readonly int frame;
        public readonly Quaternion rotation;

        public PlayerStruct(Vector3 position,
            int frame,
            Quaternion rotation)
        {
            this.position = position;
            this.frame = frame;
            this.rotation = rotation;
        }

    }
}
