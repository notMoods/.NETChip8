using OpenTK.Graphics.OpenGL;


namespace Moody;

public class Shader : IDisposable
{
    private int handle;
    private bool disposedValue = false;

    public Shader(string vertexPath, string fragmentPath)
    {
        int vertexShader, fragmentShader;

        var vertexShaderSoure = File.ReadAllText(vertexPath);
        var fragmentShaderSoure = File.ReadAllText(fragmentPath);


        vertexShader = GL.CreateShader(ShaderType.VertexShader);
        GL.ShaderSource(vertexShader, vertexShaderSoure);

        fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
        GL.ShaderSource(fragmentShader, fragmentShaderSoure);


        GL.CompileShader(vertexShader);
        GL.GetShader(vertexShader, ShaderParameter.CompileStatus, out int success);
        if (success == 0) Console.WriteLine(GL.GetShaderInfoLog(vertexShader));


        GL.CompileShader(fragmentShader);
        GL.GetShader(fragmentShader, ShaderParameter.CompileStatus, out success);
        if (success == 0) Console.WriteLine(GL.GetShaderInfoLog(fragmentShader));

        handle = GL.CreateProgram();

        GL.AttachShader(handle, vertexShader);
        GL.AttachShader(handle, fragmentShader);

        GL.LinkProgram(handle);

        GL.GetProgram(handle, GetProgramParameterName.LinkStatus, out success);
        if (success == 0) Console.WriteLine(GL.GetProgramInfoLog(handle));

        GL.DetachShader(handle, vertexShader);
        GL.DetachShader(handle, fragmentShader);
        GL.DeleteShader(fragmentShader);
        GL.DeleteShader(vertexShader);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if(disposedValue) return;

        GL.DeleteProgram(handle);
        disposedValue = true;
    }

    public void Use() => GL.UseProgram(handle);

    ~Shader()
    {
        if(disposedValue == false)
            Console.WriteLine("GPU Resource leak! Did you forget to call Dispose()?");
    }
}