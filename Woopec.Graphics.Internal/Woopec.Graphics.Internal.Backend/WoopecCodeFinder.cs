using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Woopec.Graphics.Internal.Backend
{
    /// <summary>
    /// To make it easier for coding programmers, this class searches for the turtle code
    /// </summary>
    internal class WoopecCodeFinder
    {

        public static Action Find()
        {
            var found = Find("TurtleMain");
            if (found == null)
            {
                found = Find("WoopecMain");
            }
            return found;

        }

        private static Action Find(string mainMethodName)
        {
            var rightDeclaration = $"public static void {mainMethodName}()";

            // only search in the process executable assembly
            var assembly = Assembly.GetEntryAssembly();

            var foundClasses = assembly.GetTypes().Where(t => (t.GetMethod(mainMethodName) != null)).ToList();

            if (foundClasses.Count == 1)
            {

                var method = foundClasses[0].GetMethod(mainMethodName);
                if (!method.IsPublic)
                    throw new MissingMethodException($"The method {mainMethodName} of class {foundClasses[0].Name} is not public. You have to change it to: {rightDeclaration}");
                if (!method.IsStatic)
                    throw new MissingMethodException($"The method {mainMethodName} of class {foundClasses[0].Name} is not static. You have to change it to: {rightDeclaration}.");
                if (method.ReturnType != typeof(void))
                    throw new MissingMethodException($"The method {mainMethodName} of class {foundClasses[0].Name} has wrong return type. You have to change it to: {rightDeclaration}.");
                if (method.GetParameters().Length > 0)
                    throw new MissingMethodException($"The method {mainMethodName} of class {foundClasses[0].Name} is not allowed to have parameters. You have to change it to: {rightDeclaration}.");

                var action = (Action)Delegate.CreateDelegate(typeof(Action), method);
                return action;
            }
            else if (foundClasses.Count > 1)
            {
                throw new MissingMethodException($"We have found more than one method witdh name {mainMethodName}. Only one method with this name is allowed.");
            }
            else
            {
                return null;
            }

        }

    }
}
