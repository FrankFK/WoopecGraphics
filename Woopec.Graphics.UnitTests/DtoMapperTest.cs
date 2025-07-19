using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Woopec.Graphics.Helpers;
using Woopec.Graphics.Interface.Dtos;

namespace Woopec.Graphics.UnitTests
{
    [TestClass]
    public class DtoMapperTest
    {

        [TestMethod]
        public void Vec2D_MapsToVec2DValue()
        {
            var vector1 = new Vec2D(1, 2);
            DtoVec2D result = DtoMapper.Map(vector1);
            result.X.Should().Be(vector1.X);
            result.Y.Should().Be(vector1.Y);
        }

        [TestMethod]
        public void Vec2D_NullMapsToNull()
        {
            Vec2D value = null;
            DtoVec2D result = DtoMapper.Map(value);
            result.Should().BeNull();
        }

    }
}
