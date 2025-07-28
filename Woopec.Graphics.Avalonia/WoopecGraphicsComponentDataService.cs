namespace Woopec.Graphics.Avalonia
{
    // TODO: Vielleicht für future use behalten, aber mit weniger Methoden.
    public class WoopecGraphicsComponentDataService
    {
        public string GetData() { return _value; }

        public void SetData(string value) { _value = value; }

        private string _value = "this text is from sample data service";
    }
}

