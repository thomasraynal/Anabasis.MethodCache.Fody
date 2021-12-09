using Mono.Cecil;
using Mono.Cecil.Cil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Anabasis.MemoryCache.Fody
{
    public static class DebugWriteLine
    {

        public static void WeaveMethod(ModuleDefinition moduleDefinition, 
            MethodDefinition method,
            References references)
        {
            var processor = method.Body.GetILProcessor();
            var current = method.Body.Instructions.First();

            //Create Nop instruction to use as a starting point
            //for the rest of our instructions
            var first = Instruction.Create(OpCodes.Nop);
            processor.InsertBefore(current, first);
            current = first;

            //Insert all instructions for debug output after Nop
            foreach (Instruction instruction in GetInstructions(moduleDefinition, method, references))
            {
                processor.InsertAfter(current, instruction);
                current = instruction;
            }
        }

        private static string CreateCacheKeyMethodName(MethodDefinition methodDefinition)
        {
            if (methodDefinition == null)
            {
                throw new ArgumentNullException(nameof(methodDefinition));
            }

            StringBuilder builder = new StringBuilder();

            TypeDefinition declaringType = methodDefinition.DeclaringType;

            if (declaringType.HasGenericParameters)
            {
                builder.Append(declaringType.FullName.Substring(0, declaringType.FullName.IndexOf('`')));
            }
            else
            {
                builder.Append(declaringType.FullName);
            }

            if (declaringType.GenericParameters.Any())
            {
                builder.Append('<');
                builder.Append(string.Join(", ", declaringType.GenericParameters.Select(x => x.FullName)));
                builder.Append('>');
            }

            builder.Append('.');
            builder.Append(methodDefinition.Name);

            if (methodDefinition.GenericParameters.Any())
            {
                builder.Append('<');
                builder.Append(string.Join(", ", methodDefinition.GenericParameters.Select(x => x.FullName)));
                builder.Append('>');
            }

            return builder.ToString();
        }


        private static IEnumerable<Instruction> GetInstructions(ModuleDefinition moduleDefinition, 
            MethodDefinition method,
            References references)
        {
            var methodName = CreateCacheKeyMethodName(method);
            processorContext = processorContext.Append(x => x.Create(OpCodes.Ldstr, methodName));


            yield return Instruction.Create(OpCodes.Ldstr, $"DEBUG: {method.Name}({{0}})");

            yield return Instruction.Create(OpCodes.Ldc_I4, method.Parameters.Count);
            yield return Instruction.Create(OpCodes.Newarr, moduleDefinition.ImportReference(typeof(object)));

            for (int i = 0; i < method.Parameters.Count; i++)
            {
                yield return Instruction.Create(OpCodes.Dup);
                yield return Instruction.Create(OpCodes.Ldc_I4, i);
                yield return Instruction.Create(OpCodes.Ldarg, method.Parameters[i]);

                if (method.Parameters[i].ParameterType.IsValueType)
                    yield return Instruction.Create(OpCodes.Box, method.Parameters[i].ParameterType);

                yield return Instruction.Create(OpCodes.Stelem_Ref);
            }

        //    yield return Instruction.Create(OpCodes.Call, references.StringJoinMethodReference);
            yield return Instruction.Create(OpCodes.Call, references.StringFormatMethodReference);
            yield return Instruction.Create(OpCodes.Call, references.DebugWriteLineMethodReference);
        }
    }
}
