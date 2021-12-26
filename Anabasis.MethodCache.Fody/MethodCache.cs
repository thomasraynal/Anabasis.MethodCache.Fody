using Mono.Cecil;
using Mono.Cecil.Cil;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Anabasis.MethodCache.Fody
{
    public static class MethodCache
    {

        public static void WeaveMethod(ModuleDefinition moduleDefinition,
            WeavingCandidate weavingCandidate,
            References references)
        {

            var methodDefinition = weavingCandidate.MethodDefinition;

            var processor = methodDefinition.Body.GetILProcessor();

            var current = methodDefinition.Body.Instructions.First();
            var firstNonWeaved = current;

            var first = Instruction.Create(OpCodes.Nop);
            processor.InsertBefore(current, first);
            current = first;

            VariableDefinition resultVariable = null;

            var cacheKeyVariable = new VariableDefinition(moduleDefinition.TypeSystem.String);

            if (!methodDefinition.ReturnType.IsEnumerableT())
            {
                resultVariable = methodDefinition.Body.Variables[methodDefinition.Body.Variables.Count - 1];
            }

            foreach (var instruction in WeaveTryGetCacheValue(moduleDefinition, methodDefinition, references, firstNonWeaved, cacheKeyVariable))
            {
                processor.InsertAfter(current, instruction);
                current = instruction;
            }

            var allReturnInstructions = methodDefinition.Body.Instructions.Where(instruction => instruction.OpCode == OpCodes.Ret).ToArray();
          
            foreach (var returnInstruction in allReturnInstructions.Skip(1))
            {
                foreach (var instruction in WeaveSetCacheValue(references, weavingCandidate, resultVariable, cacheKeyVariable))
                {
                    processor.InsertBefore(returnInstruction, instruction);
                }
            }

        }
        //https://github.com/SpatialFocus/MethodCache.Fody/blob/695cde0768722032865e35ec06c72db7eaff57f9/src/SpatialFocus.MethodCache.Fody/MemoryCache.cs#L226
        private static string CreateCacheKeyMethodName(MethodDefinition methodDefinition)
        {
            var builder = new StringBuilder();

            var declaringType = methodDefinition.DeclaringType;

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
                builder.Append(string.Join(", ", declaringType.GenericParameters.Select(genericParameter => genericParameter.FullName)));
                builder.Append('>');
            }

            builder.Append('.');
            builder.Append(methodDefinition.Name);

            if (methodDefinition.GenericParameters.Any())
            {
                builder.Append('<');
                builder.Append(string.Join(", ", methodDefinition.GenericParameters.Select(genericParameter => genericParameter.FullName)));
                builder.Append('>');
            }

            return builder.ToString();
        }

        private static IEnumerable<Instruction> WeaveSetCacheValue(
            References references,
            WeavingCandidate weavingCandidate,
            VariableDefinition resultVariable,
            VariableDefinition cacheKeyVariable)
        {
            var methodDefinition = weavingCandidate.MethodDefinition;

            var absoluteExpirationRelativeToNowInMillisecondsArgument = weavingCandidate.CacheAttribute.Properties.FirstOrDefault(customAttributeNamedArgument=> 
            customAttributeNamedArgument.Name == "AbsoluteExpirationRelativeToNowInMilliseconds");

            var absoluteExpirationRelativeToNowInMilliseconds = absoluteExpirationRelativeToNowInMillisecondsArgument.IsNull() ? 0L 
                : (long)absoluteExpirationRelativeToNowInMillisecondsArgument.Argument.Value;

            var slidingExpirationInMillisecondsArgument = weavingCandidate.CacheAttribute.Properties.FirstOrDefault(customAttributeNamedArgument =>
            customAttributeNamedArgument.Name == "SlidingExpirationInMilliseconds");

            var slidingExpirationInMilliseconds = slidingExpirationInMillisecondsArgument.IsNull() ? 0L
                : (long)slidingExpirationInMillisecondsArgument.Argument.Value;


            if (methodDefinition.ReturnType.IsTaskT() || methodDefinition.ReturnType.IsValueTaskT())
            {
                var taskVariable = new VariableDefinition(methodDefinition.ReturnType);
                methodDefinition.Body.Variables.Add(taskVariable);

                yield return Instruction.Create(OpCodes.Stloc, taskVariable);
                yield return Instruction.Create(OpCodes.Call, references.GetBackendTypeReference);
                yield return Instruction.Create(OpCodes.Ldloc, cacheKeyVariable);
                yield return Instruction.Create(OpCodes.Ldloc, taskVariable);
                yield return Instruction.Create(OpCodes.Ldc_I8, absoluteExpirationRelativeToNowInMilliseconds);
                yield return Instruction.Create(OpCodes.Ldc_I8, slidingExpirationInMilliseconds);
                yield return Instruction.Create(OpCodes.Callvirt, references.GetSetValue(methodDefinition.ReturnType));
                yield return Instruction.Create(OpCodes.Ldloc, taskVariable);

            }
            else if (methodDefinition.ReturnType.IsEnumerableT())
            {
                var enumerableVariable = new VariableDefinition(methodDefinition.ReturnType);
                methodDefinition.Body.Variables.Add(enumerableVariable);

                yield return Instruction.Create(OpCodes.Stloc, enumerableVariable);
                yield return Instruction.Create(OpCodes.Call, references.GetBackendTypeReference);
                yield return Instruction.Create(OpCodes.Ldloc, cacheKeyVariable);
                yield return Instruction.Create(OpCodes.Ldloc, enumerableVariable);
                yield return Instruction.Create(OpCodes.Ldc_I8, absoluteExpirationRelativeToNowInMilliseconds);
                yield return Instruction.Create(OpCodes.Ldc_I8, slidingExpirationInMilliseconds);
                yield return Instruction.Create(OpCodes.Callvirt, references.GetSetValue(methodDefinition.ReturnType));
                yield return Instruction.Create(OpCodes.Ldloc, enumerableVariable);
            }
            else
            {
                yield return Instruction.Create(OpCodes.Call, references.GetBackendTypeReference);
                yield return Instruction.Create(OpCodes.Ldloc, cacheKeyVariable);
                yield return Instruction.Create(OpCodes.Ldloc, resultVariable);
                yield return Instruction.Create(OpCodes.Ldc_I8, absoluteExpirationRelativeToNowInMilliseconds);
                yield return Instruction.Create(OpCodes.Ldc_I8, slidingExpirationInMilliseconds);
                yield return Instruction.Create(OpCodes.Callvirt, references.GetSetValue(methodDefinition.ReturnType));
            }

        }

        private static IEnumerable<Instruction> WeaveTryGetCacheValue(
            ModuleDefinition moduleDefinition,
            MethodDefinition methodDefinition,
            References references,
            Instruction firstNonWeaved,
            VariableDefinition cacheKeyVariable)
        {
            var methodName = CreateCacheKeyMethodName(methodDefinition);

            var cacheValueVariable = new VariableDefinition(methodDefinition.ReturnType);
            var hasCacheValue = new VariableDefinition(moduleDefinition.TypeSystem.Boolean);

            methodDefinition.Body.Variables.Add(cacheKeyVariable);
            methodDefinition.Body.Variables.Add(cacheValueVariable);
            methodDefinition.Body.Variables.Add(hasCacheValue);

            yield return Instruction.Create(OpCodes.Call, references.GetCacheKeyBuilderTypeReference);

            //method name
            yield return Instruction.Create(OpCodes.Ldstr, methodName);


            var arguments = methodDefinition.Parameters.Where(parameter => !parameter.CustomAttributes.Any(customAttribute => customAttribute.AttributeType.CompareTo(references.NoKeyAttributeTypeReference))).ToArray();

            //arguments names
            yield return Instruction.Create(OpCodes.Ldc_I4, arguments.Length);
            yield return Instruction.Create(OpCodes.Newarr, moduleDefinition.ImportReference(typeof(string)));

            for (int i = 0; i < arguments.Length; i++)
            {
                yield return Instruction.Create(OpCodes.Dup);
                yield return Instruction.Create(OpCodes.Ldc_I4, i);
                yield return Instruction.Create(OpCodes.Ldstr, arguments[i].Name);
                yield return Instruction.Create(OpCodes.Stelem_Ref);
            }

            //arguments values
            yield return Instruction.Create(OpCodes.Ldc_I4, arguments.Length);
            yield return Instruction.Create(OpCodes.Newarr, moduleDefinition.ImportReference(typeof(object)));

            for (int i = 0; i < arguments.Length; i++)
            {
                
                yield return Instruction.Create(OpCodes.Dup);
                yield return Instruction.Create(OpCodes.Ldc_I4, i);
                yield return Instruction.Create(OpCodes.Ldarg, arguments[i]);

                if (arguments[i].ParameterType.IsGenericParameter || arguments[i].ParameterType.IsValueType)
                    yield return Instruction.Create(OpCodes.Box, arguments[i].ParameterType);

                yield return Instruction.Create(OpCodes.Stelem_Ref);
            }

            yield return Instruction.Create(OpCodes.Callvirt, references.CreateKeyMethodReference);
            yield return Instruction.Create(OpCodes.Stloc, cacheKeyVariable);

            yield return Instruction.Create(OpCodes.Call, references.GetBackendTypeReference);
            yield return Instruction.Create(OpCodes.Ldloc, cacheKeyVariable);
            yield return Instruction.Create(OpCodes.Ldloca, cacheValueVariable);

       
            yield return Instruction.Create(OpCodes.Callvirt, references.GetTryGetValue(methodDefinition.ReturnType));
            yield return Instruction.Create(OpCodes.Stloc, hasCacheValue);
            yield return Instruction.Create(OpCodes.Ldloc, hasCacheValue);

            yield return Instruction.Create(OpCodes.Brfalse, firstNonWeaved);

            yield return Instruction.Create(OpCodes.Ldstr, $"hit cache - {methodName}");
            yield return Instruction.Create(OpCodes.Call, references.DebugWriteLineStringMethodReference);


            yield return Instruction.Create(OpCodes.Ldloc, cacheValueVariable);
            yield return Instruction.Create(OpCodes.Ret);

        }
    }
}
