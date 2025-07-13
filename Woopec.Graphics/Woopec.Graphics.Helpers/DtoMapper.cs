using Woopec.Graphics.InternalDtos;

namespace Woopec.Graphics.Helpers
{
    /// <summary>
    /// I do not want to have any dependencies to the data types (i.e. to Vec2D, Color, etc.) of Woopec.Graphics in the other namespaces.
    /// That's why I map these data types to Dto types.
    /// </summary>
    internal static class DtoMapper
    {
        public static Vec2DValue Map(Vec2D woopecValue) => (woopecValue is null) ? null : new Vec2DValue(woopecValue.X, woopecValue.Y);
        public static Vec2D Map(Vec2DValue dtoValue) => (dtoValue is null) ? null : new Vec2D(dtoValue.X, dtoValue.Y);
    }


}
