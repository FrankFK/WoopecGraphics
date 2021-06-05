using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TurtleCore.Internal
{
    /// <summary>
    /// To make it easier for coding programmers, this class searches for the turtle code
    /// </summary>
    internal class TurtleCodeFinder
    {

        public static Action Find()
        {
            const string turtleMainName = "TurtleMain";
            var rightDeclaration = $"public static void {turtleMainName}()";

            // only search in the process executable assembly
            var assembly = Assembly.GetEntryAssembly();

            var foundClasses = assembly.GetTypes().Where(t => (t.GetMethod(turtleMainName) != null)).ToList();

            if (foundClasses.Count == 1)
            {

                var method = foundClasses[0].GetMethod(turtleMainName);
                if (!method.IsPublic)
                    throw new MissingMethodException($"The method {turtleMainName} of class {foundClasses[0].Name} is not public. You have to change it to: {rightDeclaration}");
                if (!method.IsStatic)
                    throw new MissingMethodException($"The method {turtleMainName} of class {foundClasses[0].Name} is not static. You have to change it to: {rightDeclaration}.");
                if (method.ReturnType != typeof(void))
                    throw new MissingMethodException($"The method {turtleMainName} of class {foundClasses[0].Name} has wrong return type. You have to change it to: {rightDeclaration}.");
                if (method.GetParameters().Length > 0)
                    throw new MissingMethodException($"The method {turtleMainName} of class {foundClasses[0].Name} is not allowed to have parameters. You have to change it to: {rightDeclaration}.");

                var action = (Action)Delegate.CreateDelegate(typeof(Action), method);
                return action;
            }
            else if (foundClasses.Count > 1)
            {
                throw new MissingMethodException($"We have found more than one method witdh name {turtleMainName}. Only one method with this name is allowed.");
            }
            else
            {
                return null;
            }

        }
    }
}
