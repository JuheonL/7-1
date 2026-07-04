using UnityEditor;
using System.Diagnostics;

public static class ClaudeCodeLauncher
{
    [MenuItem("Tools/Open Claude Code")]
    public static void OpenClaudeCode()
    {
        string projectPath = System.IO.Path.GetFullPath(".");

        Process process = new Process();
        process.StartInfo.FileName = "cmd.exe";
        process.StartInfo.Arguments = $"/k \"cd /d \"{projectPath}\" && claude\"";
        process.StartInfo.UseShellExecute = true;
        process.Start();
    }
}
