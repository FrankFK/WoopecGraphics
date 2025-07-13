using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Woopec.Graphics.Helpers;
using Woopec.Graphics.InternalDtos;

namespace Woopec.Graphics.UnitTests
{
    [TestClass]
    public class DtoMapperTest
    {

        [TestMethod]
        public void Vec2D_MapsToVec2DValue()
        {
            var vector1 = new Vec2D(1, 2);
            Vec2DValue result = DtoMapper.Map(vector1);
            result.X.Should().Be(vector1.X);
            result.Y.Should().Be(vector1.Y);
        }

        [TestMethod]
        public void Vec2D_NullMapsToNull()
        {
            Vec2D value = null;
            Vec2DValue result = DtoMapper.Map(value);
            result.Should().BeNull();
        }

    }
}
