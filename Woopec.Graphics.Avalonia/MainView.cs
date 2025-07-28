
using Avalonia.Markup.Declarative;

namespace Woopec.Graphics.Avalonia
{
    public class MainView : ComponentBase
    {
        protected override object Build()
            => New<WoopecGraphicsComponent>() // creates Child SimpleComponent and injects it's dependencies into constructor
                .Name(nameof(MainView)); //set component name
    }
}
