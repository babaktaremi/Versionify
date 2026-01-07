namespace Versionify;

public static class ConsoleExtensions
{
    public static void PrintTable(List<ProjectInfo> projectInfos)
    {
        var table = new Table();
        table.Border(TableBorder.Rounded);
        table.BorderColor(Color.Grey);
        table.AddColumn(new TableColumn("[cyan]#[/]").Centered());
        table.AddColumn(new TableColumn("[yellow]Project File[/]"));
        table.AddColumn(new TableColumn("[green]Target Framework[/]"));
        table.AddColumn(new TableColumn("[dim]Path[/]"));

        for (int i = 0; i < projectInfos.Count; i++)
        {
            var frameworkMarkup =  !string.IsNullOrEmpty(projectInfos[i].TargetFramework)
                ? $"[green]{projectInfos[i].TargetFramework}[/]"
                : $"[red]{projectInfos[i].TargetFramework ?? "Not found"}[/]";
            
            table.AddRow(
                $"[cyan]{i + 1}[/]",
                $"[bold]{Path.GetFileName(projectInfos[i].FilePath)}[/]",
                frameworkMarkup,
                $"[dim]{Path.GetDirectoryName(projectInfos[i].FilePath)}[/]"
            );
        }

        AnsiConsole.Write(table);
        AnsiConsole.WriteLine();
    }
}