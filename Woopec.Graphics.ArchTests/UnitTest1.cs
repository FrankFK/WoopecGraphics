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
        /// Helper objectes and interfaces used by WoopecGraphicsPublic
        /// </summary>
        private readonly IObjectProvider<IType> WoopecGraphicsHelpers = Types()
            .That()
            .ResideInNamespace("Woopec.Graphics.Helpers")
            .As("WoopecGraphics helper objects and interfaces for the public objects");

        /// <summary>
        /// Infrastructure code for the internal backend part of the graphics. This part interacts with the frontend via
        /// interfaces to channels, that connect backend and frontend
        /// </summary>
        private readonly IObjectProvider<IType> WoopecGraphicsInternalBackend = Types()
            .That()
            .ResideInNamespace("Woopec.Graphics.InternalBackend")
            .As("WoopecGraphics internal backend part");

        /// <summary>
        /// Infrastructure code for the internal frontend part of the graphics. This part interacts with the backend via
        /// interfaces to channels, that connect backend and frontend
        /// </summary>
        private readonly IObjectProvider<IType> WoopecGraphicsInternalFrontend = Types()
            .That()
            .ResideInNamespace("Woopec.Graphics.InternalFrontend")
            .As("WoopecGraphics internal frontend part");

        /// <summary>
        /// This layer connects backend and frontend and initializes everything
        /// </summary>
        private readonly IObjectProvider<IType> WoopecGraphicsInternalCommunication = Types()
            .That()
            .ResideInNamespace("Woopec.Graphics.InternalCommunication")
            .As("WoopecGraphics communication between backend and frontend");

        /// <summary>
        /// Objects which travel between backend and frontend
        /// </summary>
        private readonly IObjectProvider<IType> WoopecGraphicsCommunicatedObjects = Types()
            .That()
            .ResideInNamespace("Woopec.Graphics.InternalCommunicatedObjects")
            .As("WoopecGraphics communicated objects");

        /// <summary>
        /// WoopecGraphics examples
        /// </summary>
        private readonly IObjectProvider<IType> WoopecGraphicsExamples = Types()
            .That()
            .ResideInNamespace("Woopec.Graphics.Examples")
            .As("WoopecGraphics examples");



        private List<IObjectProvider<IType>> AllLayers()
        {
            return new List<IObjectProvider<IType>>() {WoopecGraphicsPublic, WoopecGraphicsHelpers, WoopecGraphicsInternalBackend,
            WoopecGraphicsInternalCommunication, WoopecGraphicsInternalFrontend, WoopecGraphicsCommunicatedObjects, WoopecGraphicsExamples};
        }

        private List<IObjectProvider<IType>> AllLayersExcept(List<IObjectProvider<IType>> except)
        {
            List<IObjectProvider<IType>> allExcept = AllLayers();
            foreach (IObjectProvider<IType> layer in except)
            {
                allExcept.Remove(layer);
            }
            return allExcept;
        }

        [Fact]
        public void AllLayersShouldHaveContent()
        {
            foreach (var layer in AllLayers())
            {
                Types().That().Are(layer).Should().Exist().Check(Architecture);
            }
        }

        [Fact]
        public void PublicObjectsShouldOnlyUseHelpersAndCommunicatedObjects()
        {
            foreach (var layer in AllLayersExcept([WoopecGraphicsHelpers, WoopecGraphicsCommunicatedObjects, WoopecGraphicsPublic]))
            {
                // Check this other ones
                IArchRule doNotAcessLowLevelImplementation = Types()
                .That()
                .Are(WoopecGraphicsPublic)
                .Should()
                .NotDependOnAny(layer)
                .Because("Public objects should only use a few layers");

                doNotAcessLowLevelImplementation.Check(Architecture);
            }
        }

        [Fact]
        public void HelpersShouldShouldOnlyUsePublicAndCommunicatedObjects()
        {
            foreach (var layer in AllLayersExcept([WoopecGraphicsPublic, WoopecGraphicsCommunicatedObjects, WoopecGraphicsHelpers]))
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

        [Fact]
        public void ExamplesShouldOnlyUsePublicObjects()
        {
            foreach (var layer in AllLayersExcept([WoopecGraphicsPublic, WoopecGraphicsExamples]))
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
        public void InternalBackendShouldUseOnlyCommunicatedObjectsAndPublicAndHelpers()
        {
            foreach (var layer in AllLayersExcept([WoopecGraphicsCommunicatedObjects, WoopecGraphicsPublic, WoopecGraphicsHelpers, WoopecGraphicsInternalBackend]))
            {
                IArchRule doNotAcessLowLevelImplementation = Types()
                    .That()
                    .Are(WoopecGraphicsInternalBackend)
                    .Should()
                    .NotDependOnAny(layer)
                    .Because("Internal backend should only use ...");
                doNotAcessLowLevelImplementation.Check(Architecture);
            }
        }

        [Fact]
        public void InternalFrontendShouldOnlyUseCommunicatedObjects()
        {
            foreach (var layer in AllLayersExcept([WoopecGraphicsCommunicatedObjects, WoopecGraphicsInternalFrontend]))
            {
                IArchRule doNotAcessLowLevelImplementation = Types()
                    .That()
                    .Are(WoopecGraphicsInternalFrontend)
                    .Should()
                    .NotDependOnAny(layer)
                    .Because("Internal backend should only use ...");
                doNotAcessLowLevelImplementation.Check(Architecture);
            }
        }

        [Fact]
        public void CommunicatedObjectsShouldOnlyUse()
        {
            foreach (var layer in AllLayersExcept([WoopecGraphicsCommunicatedObjects]))
            {
                IArchRule doNotAcessLowLevelImplementation = Types()
                    .That()
                    .Are(WoopecGraphicsCommunicatedObjects)
                    .Should()
                    .NotDependOnAny(layer)
                    .Because("Internal backend should only use ...");
                doNotAcessLowLevelImplementation.Check(Architecture);
            }
        }


    }
}

