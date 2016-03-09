﻿using Mono.Cecil;
using Mono.Cecil.Cil;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace StardewModdingAPI.Helpers
{
    public enum CecilContextType
    {
        SMAPI,
        Stardew
    }
    public class CecilContext
    {
        public CecilContextType ContextType { get; private set;}

        private AssemblyDefinition _assemblyDefinition { get; set; }
        private bool _isMemoryStreamDirty { get; set; }
        
        private MemoryStream _modifiedAssembly;
        private MemoryStream ModifiedAssembly
        {
            get
            {
                if(_modifiedAssembly == null)
                {
                    _modifiedAssembly = new MemoryStream();
                    _assemblyDefinition.Write(_modifiedAssembly);
                }
                else
                {
                    if(_isMemoryStreamDirty)
                    {
                        _modifiedAssembly.Dispose();
                        _modifiedAssembly = new MemoryStream();
                        _assemblyDefinition.Write(_modifiedAssembly);
                    }
                }
                return _modifiedAssembly;
            }
        }
        
        public CecilContext(CecilContextType contextType)
        {
            ContextType = contextType;

            if (ContextType == CecilContextType.SMAPI)
                _assemblyDefinition = AssemblyDefinition.ReadAssembly(Assembly.GetExecutingAssembly().Location);
            else
                _assemblyDefinition = AssemblyDefinition.ReadAssembly(Constants.StardewExePath);
        }

        public ILProcessor GetMethodILProcessor(string type, string method)
        {
            if (_assemblyDefinition == null)
                throw new Exception("ERROR Assembly not properly read. Cannot parse");

            if (string.IsNullOrWhiteSpace(type) || string.IsNullOrWhiteSpace(method))
                throw new ArgumentNullException("Both type and method must be set");

            Mono.Cecil.Cil.ILProcessor ilProcessor = null;
            TypeDefinition typeDef = _assemblyDefinition.MainModule.Types.FirstOrDefault(n => n.FullName == type);
            if (typeDef != null)
            {
                MethodDefinition methodDef = typeDef.Methods.FirstOrDefault(m => m.Name == method);
                if (methodDef != null)
                {
                    ilProcessor = methodDef.Body.GetILProcessor();
                }
            }

            return ilProcessor;
        }

        public MethodInfo GetSMAPIMethodReference(string type, string method)
        {
            if (_assemblyDefinition == null)
                throw new Exception("ERROR Assembly not properly read. Cannot parse");

            if (ContextType != CecilContextType.SMAPI)
                throw new Exception("GetSMAPIMethodReference can only be called on the SMAPI context");

            MethodInfo methodInfo = null;

            var smapiAssembly = Assembly.GetExecutingAssembly().GetType(type);
            if (smapiAssembly != null)
            {
                methodInfo = smapiAssembly.GetMethod(method);
            }

            return methodInfo;
        }

        public MethodReference ImportSMAPIMethodInStardew(MethodInfo method)
        {
            if (_assemblyDefinition == null)
                throw new Exception("ERROR Assembly not properly read. Cannot parse");

            if (ContextType != CecilContextType.SMAPI)
                throw new Exception("ImportSmapiMethodInStardew can only be called on the Stardew context");

            MethodReference reference = null;
            if (method != null)
            {                
                reference = _assemblyDefinition.MainModule.Import(method);
            }
            return reference;
        }
    }
}