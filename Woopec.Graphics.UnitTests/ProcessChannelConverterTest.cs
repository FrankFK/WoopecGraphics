using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.Json;
using Woopec.Graphics;
using Woopec.Graphics.Helpers;
using Woopec.Graphics.InternalCommunication;
using Woopec.Graphics.Interface.Dtos;

namespace TurtleCore.UnitTests
{
    [TestClass]
    public class ProcessChannelConverterTest
    {

        [TestMethod]
        public void ShapeSerialization_Works()
        {
            // Create a list with all derived classes of ShapeBase
            DtoShape shape = DtoMapper.Map( new Shape(new() { (0, 0), (10, -5), (0, 10), (-10, -5) }));
            DtoImageShape imageShape = DtoMapper.Map(new ImageShape(""));

            var objects = new List<DtoShapeBase> { shape, imageShape };

            var shapeBaseConverter = new ProcessChannelConverter<DtoShapeBase>(DtoShapeBase.JsonTypeDiscriminatorAsInt, DtoShapeBase.JsonWrite, DtoShapeBase.JsonRead);

            // Using: System.Text.Json
            var options = new JsonSerializerOptions
            {
                Converters = { shapeBaseConverter },
                WriteIndented = true
            };

            string jsonString = JsonSerializer.Serialize(objects, options);

            var roundTrip = JsonSerializer.Deserialize<List<DtoShapeBase>>(jsonString, options);
            roundTrip.Should().BeEquivalentTo(objects);
        }

        [TestMethod]
        public void ScreenAnimationEffectSerialization_Works()
        {
            // Create a list with all derived classes of ScreenAnimationEffect
            var objects = new List<ScreenAnimationEffect>();

            objects.Add(new ScreenAnimationEffect() { Milliseconds = 100 });
            objects.Add(new ScreenAnimationMovement() { Milliseconds = 200, StartValue = new DtoVec2D(1, 2), AnimatedProperty = ScreenAnimationMovementProperty.Point2 });
            objects.Add(new ScreenAnimationRotation() { Milliseconds = 300, StartValue = 20, AnimatedProperty = ScreenAnimationRotationProperty.Heading });

            var effectConverter = new ProcessChannelConverter<ScreenAnimationEffect>(ScreenAnimationEffect.JsonTypeDiscriminatorAsInt, ScreenAnimationEffect.JsonWrite, ScreenAnimationEffect.JsonRead);

            // Using: System.Text.Json
            var options = new JsonSerializerOptions
            {
                Converters = { effectConverter },
                WriteIndented = true
            };

            string jsonString = JsonSerializer.Serialize(objects, options);

            var roundTrip = JsonSerializer.Deserialize<List<ScreenAnimationEffect>>(jsonString, options);
            roundTrip.Should().BeEquivalentTo(objects);
        }

        [TestMethod]
        public void ScreenObjectSerialization_Works()
        {
            // Create a list with all derived classes of ScreenAnimationEffect
            var objects = new List<ScreenObject>();

            var figure = new ScreenFigure(5)
            {
                ID = 5,
                GroupID = 17,
                WaitForCompletedAnimationsOfSameGroup = true,
                WaitForCompletedAnimationsOfAnotherGroup = 15,
                Animation = new ScreenAnimation()
                {
                    Effects = new List<ScreenAnimationEffect>()
                    {
                        new ScreenAnimationRotation() { Milliseconds = 300, StartValue = 20, AnimatedProperty = ScreenAnimationRotationProperty.Heading  }
                    }
                },
                Position = new DtoVec2D(1, 2),
                Heading = 20,
                FillColor = DtoMapper.Map(Colors.Red),
                OutlineColor = DtoMapper.Map(Colors.Blue),
                IsVisible = true
            };

            var components = new List<DtoShapeComponent>();
            var component = new DtoShapeComponent();
            component.Polygon = new List<DtoVec2D>() { new DtoVec2D(0, 0), new DtoVec2D(10, -5), new DtoVec2D(0, 10), new DtoVec2D(-10, -5) };
            component.FillColor = DtoMapper.Map(Colors.Red);
            component.OutlineColor = DtoMapper.Map(Colors.Blue);

            figure.Shape = new DtoShape("some name", DtoShapeType.Polygon, components);

            var line = new ScreenLine()
            {
                ID = 5,
                GroupID = 17,
                WaitForCompletedAnimationsOfSameGroup = true,
                WaitForCompletedAnimationsOfAnotherGroup = 15,
                Animation = new ScreenAnimation()
                {
                    Effects = new List<ScreenAnimationEffect>()
                    {
                        new ScreenAnimationMovement() { Milliseconds = 300, StartValue = new DtoVec2D(1, 2), AnimatedProperty = ScreenAnimationMovementProperty.Point2  }
                    }
                },
                Point1 = new DtoVec2D(1, 2),
                Point2 = new DtoVec2D(100, 2),
                Color = DtoMapper.Map(Colors.Red),
            };

            var textBox = new ScreenTextBlock()
            {
                ID = 5,
                GroupID = 17,
                WaitForCompletedAnimationsOfSameGroup = true,
                WaitForCompletedAnimationsOfAnotherGroup = 15,
                Position = new DtoVec2D(1, 2),
                Text = "Hallo?\nZweite Zeile",
                TextStyle = new DtoTextStyle()
                {
                    FontFamilyName = "Courier New",
                    FontSize = 12,
                    FontStyle = DtoFontStyleType.Italic,
                    FontWeight = DtoFontWeightType.Bold,
                    BackgroundColor = null,
                    ForegroundColor = DtoMapper.Map(Colors.Blue)
                },
                Alignment = DtoTextAlignmentType.Left,
                ReturnLowerRightCorner = true,
            };

            objects.Add(figure);
            objects.Add(line);
            objects.Add(textBox);

            var shapeBaseConverter = new ProcessChannelConverter<DtoShapeBase>(DtoShapeBase.JsonTypeDiscriminatorAsInt, DtoShapeBase.JsonWrite, DtoShapeBase.JsonRead);
            var effectConverter = new ProcessChannelConverter<ScreenAnimationEffect>(ScreenAnimationEffect.JsonTypeDiscriminatorAsInt, ScreenAnimationEffect.JsonWrite, ScreenAnimationEffect.JsonRead);
            var screenObjetctConverter = new ProcessChannelConverter<ScreenObject>(ScreenObject.JsonTypeDiscriminatorAsInt, ScreenObject.JsonWrite, ScreenObject.JsonRead);

            // Using: System.Text.Json
            var options = new JsonSerializerOptions
            {
                Converters = { shapeBaseConverter, effectConverter, screenObjetctConverter },
                WriteIndented = true
            };

            string jsonString = JsonSerializer.Serialize(objects, options);

            var roundTrip = JsonSerializer.Deserialize<List<ScreenObject>>(jsonString, options);

            roundTrip.Count.Should().Be(objects.Count);

            roundTrip[0].Should().BeOfType<ScreenFigure>();
            (roundTrip[0] as ScreenFigure).Should().BeEquivalentTo(figure);

            roundTrip[1].Should().BeOfType<ScreenLine>();
            (roundTrip[1] as ScreenLine).Should().BeEquivalentTo(line);

            roundTrip[2].Should().BeOfType<ScreenTextBlock>();
            (roundTrip[2] as ScreenTextBlock).Should().BeEquivalentTo(textBox);
        }


    }
}
