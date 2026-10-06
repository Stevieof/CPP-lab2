using System.Runtime.InteropServices;

namespace Core;

public sealed record EnvironmentReport(
    string Application,
    string Student,
    string OSDescription,
    string OSVersion,
    string ProcessArchitecture,
    string DotNetVersion,
    string Framework,
    string DetectedRid,
    string ReportedRid,
    string AppDirectory,
    string CurrentDirectory,
    string Domain);

public static class EnvironmentInfo
{
    public static EnvironmentReport Collect() => new(
        Application: "CrossApp",
        Student: "Зарічний Степан, група ФЕІ-33с",
        OSDescription: RuntimeInformation.OSDescription,
        OSVersion: Environment.OSVersion.ToString(),
        ProcessArchitecture: RuntimeInformation.ProcessArchitecture.ToString(),
        DotNetVersion: Environment.Version.ToString(),
        Framework: RuntimeInformation.FrameworkDescription,
        DetectedRid: DetectRid(),
        ReportedRid: RuntimeInformation.RuntimeIdentifier,
        AppDirectory: AppContext.BaseDirectory,
        CurrentDirectory: Environment.CurrentDirectory,
        Domain: "Склад (товари, партії, залишки, переміщення)");

    // Узагальнений RID за ОС та архітектурою процесу.
    // Точний RID середовища (зокрема для Linux musl) міститься в ReportedRid.
    private static string DetectRid()
    {
        string os =
            RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "win" :
            RuntimeInformation.IsOSPlatform(OSPlatform.Linux) ? "linux" :
            RuntimeInformation.IsOSPlatform(OSPlatform.OSX) ? "osx" : "unknown";

        string architecture = RuntimeInformation.ProcessArchitecture switch
        {
            Architecture.X64 => "x64",
            Architecture.X86 => "x86",
            Architecture.Arm64 => "arm64",
            Architecture.Arm => "arm",
            _ => "unknown"
        };

        return $"{os}-{architecture}";
    }
}
