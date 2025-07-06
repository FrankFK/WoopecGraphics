using ArchUnitNET.Domain;
using ArchUnitNET.Fluent;
using ArchUnitNET.Loader;
using ArchUnitNET.xUnit;
using Xunit;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace Woopec.Graphics.ArchTests
{
    public class UnitTest1
    {
        // TIP: load your architecture once at the start to maximize performance of your tests
        private static readonly Architecture Architecture = new ArchLoader()
            .LoadAssemblies(typeof(Turtle).Assembly)
            .Build();

        /// <summary>
        /// The objects that are available to the outside (Turtle, Figure, Pen) etc.
        /// </summary>
        private readonly IObjectProvider<IType> WoopecGraphicsPublic = Types()
            .That()
            .ResideInNamespace("Woopec.Graphics")
            .As("WoopecGraphics public objects");

        /// <summary>
        /// Small helper objectes
        /// </summary>
        private readonly IObjectProvider<IType> WoopecGraphicsHelpers = Types()
            .That()
            .ResideInNamespace("Woopec.Graphics.Helpers")
            .As("WoopecGraphics small helper objects");

        /// <summary>
        /// Woopec.Graphics exchanges data with the ILowLevelScreen. For this purpose, (partially asynchronous) methods of ILowLevelScreen are called. 
        /// This layer contains the objects that WoopecGraphicPublic needs to know.
        /// </summary>
        private readonly IObjectProvider<IType> WoopecGraphicsLowLevelScreenInterface = Types()
            .That()
            .ResideInNamespace("Woopec.Graphics.LowLevelScreen")
            .As("WoopecGraphics low level screen interface");

        /// <summary>
        /// The internal implementation for LowLevelScreen
        /// This layer contains the objects that WoopecGraphicPublic needs to know.
        /// </summary>
        private readonly IObjectProvider<IType> WoopecGraphicsLowLevelScreenImplementation = Types()
            .That()
            .ResideInNamespace("Woopec.Graphics.Internal")
            .As("WoopecGraphics low level screen implementation");

        /// <summary>
        /// Factories to generate the appropriate implementation for an interface. Currently, no dependency injection is used in the solution. 
        /// This could perhaps be changed later.
        /// </summary>
        private readonly IObjectProvider<IType> WoopecGraphicsFactories = Types()
            .That()
            .ResideInNamespace("Woopec.Graphics.Factories")
            .As("WoopecGraphics low level screen implementation");

        /// <summary>
        /// WoopecGraphics examples
        /// </summary>
        private readonly IObjectProvider<IType> WoopecGraphicsExamples = Types()
            .That()
            .ResideInNamespace("Woopec.Graphics.Examples")
            .As("WoopecGraphics examples");



        [Fact]
        public void AllNameSpacesShouldBeReferencedCorrectly()
        {
            IArchRule checkGraphicsPublic = Types().That().Are(WoopecGraphicsPublic).Should().Exist();
            checkGraphicsPublic.Check(Architecture);

            Types().That().Are(WoopecGraphicsHelpers).Should().Exist().Check(Architecture);
            Types().That().Are(WoopecGraphicsExamples).Should().Exist().Check(Architecture);
            Types().That().Are(WoopecGraphicsFactories).Should().Exist().Check(Architecture);
            Types().That().Are(WoopecGraphicsLowLevelScreenInterface).Should().Exist().Check(Architecture);
            Types().That().Are(WoopecGraphicsLowLevelScreenImplementation).Should().Exist().Check(Architecture);
        }

        [Fact]
        public void PublicObjectsShouldNotUseLowLevelScreenImplementation()
        {
            IArchRule doNotAcessLowLevelImplementation = Types()
                .That()
                .Are(WoopecGraphicsPublic)
                .Should()
                .NotDependOnAny(WoopecGraphicsLowLevelScreenImplementation)
                .Because("Public objects should only use LowLevelScreenInterface, but not it's implementation in LowLevelScreenImplementation");
            doNotAcessLowLevelImplementation.Check(Architecture);

        }

        [Fact]
        public void ExamplesShouldOnlyUsePublicObjects()
        {
            var forbiddenLayers = new List<IObjectProvider<IType>> { WoopecGraphicsLowLevelScreenImplementation, WoopecGraphicsHelpers, WoopecGraphicsFactories,
                WoopecGraphicsLowLevelScreenInterface, WoopecGraphicsLowLevelScreenImplementation};

            foreach (var layer in forbiddenLayers)
            {
                IArchRule doNotAcessLowLevelImplementation = Types()
                    .That()
                    .Are(WoopecGraphicsExamples)
                    .Should()
                    .NotDependOnAny(layer)
                    .Because("Examples should only use public WoopecGraphics objects");
                doNotAcessLowLevelImplementation.Check(Architecture);
            }
        }

        [Fact]
        public void HelpersShouldNotUseOtherLayers()
        {
            var forbiddenLayers = new List<IObjectProvider<IType>> { WoopecGraphicsLowLevelScreenImplementation, WoopecGraphicsFactories,
                WoopecGraphicsLowLevelScreenInterface, WoopecGraphicsLowLevelScreenImplementation};

            foreach (var layer in forbiddenLayers)
            {
                IArchRule doNotAcessLowLevelImplementation = Types()
                    .That()
                    .Are(WoopecGraphicsHelpers)
                    .Should()
                    .NotDependOnAny(layer)
                    .Because("Helpers should be simple helpers");
                doNotAcessLowLevelImplementation.Check(Architecture);
            }
        }
    }
}

