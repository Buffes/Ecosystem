using System;
using Unity.Entities;
using UnityEngine;

namespace Ecosystem.ECS.Debugging.Graphics
{
    public struct ShapeStyle : ISharedComponentData, IEquatable<ShapeStyle>
    {
        public Material Material;

        public bool Equals(ShapeStyle other) => ReferenceEquals(Material, other);
        public override int GetHashCode() => Material == null ? 0 : Material.GetHashCode();
    }
}
