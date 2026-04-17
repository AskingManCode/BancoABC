<%@ Application Language="C#" %>
<%@ Import Namespace="System.Reflection" %>

<script runat="server">
    void Application_Start(object sender, EventArgs e)
    {
        AppDomain.CurrentDomain.AssemblyResolve += (s, args) =>
        {
            var requestedAssembly = new AssemblyName(args.Name);
            string binPath = Server.MapPath("~/bin/");

            // Lista de ensamblados conflictivos
            var mappings = new System.Collections.Generic.Dictionary<string, string>
            {
                { "System.Memory", "System.Memory.dll" },
                { "System.Buffers", "System.Buffers.dll" },
                { "System.Runtime.CompilerServices.Unsafe", "System.Runtime.CompilerServices.Unsafe.dll" },
                { "System.Threading.Tasks.Extensions", "System.Threading.Tasks.Extensions.dll" },
                { "Microsoft.Bcl.AsyncInterfaces", "Microsoft.Bcl.AsyncInterfaces.dll" }
            };

            if (mappings.ContainsKey(requestedAssembly.Name))
            {
                string fullPath = System.IO.Path.Combine(binPath, mappings[requestedAssembly.Name]);
                if (System.IO.File.Exists(fullPath))
                {
                    // Carga el ensamblado desde el archivo sin verificar versión
                    return Assembly.LoadFile(fullPath);
                }
            }
            return null; 
        };
    }
</script>