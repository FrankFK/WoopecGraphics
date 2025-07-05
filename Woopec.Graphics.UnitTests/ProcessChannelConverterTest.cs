using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.Json;
using Woopec.Core;
using Woopec.Core.Internal;

namespace TurtleCore.UnitTests
{
    [TestClass]
    public class ProcessChannelConverterTest
    {

        [TestMethod]
        public void ShapeSerialization_Works()
        {
            // Create a list with all derived classes of ShapeBase
            var shape = new Shape(new() { (0, 0), (10, -5), (0, 10), (-10, -5) });
            var imageShape = new ImageShape("");

            var objects = new List<ShapeBase> { shape, imageShape };

            var shapeBaseConverter = new ProcessChannelConverter<ShapeBase>(ShapeBase.JsonTypeDiscriminatorAsInt, ShapeBase.JsonWrite, ShapeBase.JsonRead);

            // Using: System.Text.Json
            var options = new JsonSerializerOptions
            {
                Converters = { shapeBaseConverter },
                WriteIndented = true
            };

            string jsonString = JsonSerializer.Serialize(objects, options);

            var roundTrip = JsonSerializer.Deserialize<List<ShapeBase>>(jsonString, options);
            roundTrip.Should().BeEquivalentTo(objects);
        }

        [TestMethod]
        public void ScreenAnimationEffectSerialization_Works()
        {
            // Create a list with all derived classes of ScreenAnimationEffect
            var objects = new List<ScreenAnimationEffect>();

            objects.Add(new ScreenAnimationEffect() { Milliseconds = 100 });
            objects.Add(new ScreenAnimationMovement() { Milliseconds = 200, StartValue = (1, 2), AnimatedProperty = ScreenAnimationMovementProperty.Point2 });
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
                Position = (1, 2),
                Heading = 20,
                FillColor = Colors.Red,
                OutlineColor = Colors.Blue,
                IsVisible = true,
                Shape = new Shape(new() { (0, 0), (10, -5), (0, 10), (-10, -5) })
            };

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
                        new ScreenAnimationMovement() { Milliseconds = 300, StartValue = (1, 2), AnimatedProperty = ScreenAnimationMovementProperty.Point2  }
                    }
                },
                Point1 = (1, 2),
                Point2 = (100, 2),
                Color = Colors.Red,
            };

            var textBox = new ScreenTextBlock()
            {
                ID = 5,
                GroupID = 17,
                WaitForCompletedAnimationsOfSameGroup = true,
                WaitForCompletedAnimationsOfAnotherGroup = 15,
                Position = (1, 2),
                Text = "Hallo?\nZweite Zeile",
                TextStyle = new TextStyle()
                {
                    FontFamilyName = "Courier New",
                    FontSize = 12,
                    FontStyle = FontStyleType.Italic,
                    FontWeight = FontWeightType.Bold,
                    BackgroundColor = null,
                    ForegroundColor = Colors.Blue
                },
                Alignment = TextAlignmentType.Left,
                ReturnLowerRightCorner = true,
            };

            objects.Add(figure);
            objects.Add(line);
            objects.Add(textBox);

            var shapeBaseConverter = new ProcessChannelConverter<ShapeBase>(ShapeBase.JsonTypeDiscriminatorAsInt, ShapeBase.JsonWrite, ShapeBase.JsonRead);
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
