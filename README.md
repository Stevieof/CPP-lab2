# CrossApp

Навчальний проєкт з курсу «Крос-платформне програмування». Предметна область — **склад**: товари, партії, залишки та переміщення.

## Лабораторна робота №2

Збір інформації про середовище винесено в бібліотеку `Core`. Метод `EnvironmentInfo.Collect()` повертає запис `EnvironmentReport` з даними про студента, ОС, архітектуру процесу, .NET, RID, каталоги та предметну область. `Cli` підключає бібліотеку через `ProjectReference` і відповідає за обробку аргументів та виведення таблиці або JSON.

`DetectedRid` формується за ОС та архітектурою процесу, а `ReportedRid` повертається середовищем .NET. Для окремих платформ, наприклад Linux musl, вони можуть відрізнятися.

## Структура рішення

```text
CrossApp/
├── CrossApp.sln
├── README.md
├── .gitignore
└── src/
    ├── Core/
    │   ├── Core.csproj
    │   └── EnvironmentInfo.cs
    └── Cli/
        ├── Cli.csproj
        └── Program.cs
```

## Збірка та запуск

Потрібен .NET SDK 9.0. Для збірки Core під .NET 8 також потрібен відповідний targeting pack; на машині перевірки встановлено SDK 8 і 9. Команди виконуйте з каталогу, де розташований `CrossApp.sln`.

Збірка всього рішення:

```bash
dotnet build
```

Запуск із таблицею:

```bash
dotnet run --project src/Cli
```

Запуск із JSON одним рядком:

```bash
dotnet run --project src/Cli -- --json
```

## Multi-targeting

У `Core.csproj` задано `<TargetFrameworks>net8.0;net9.0</TargetFrameworks>`. Бібліотека збирається для обох версій, а консольний застосунок використовує `net9.0`.

Окрема перевірка кожної цільової платформи:

```bash
dotnet build src/Core -f net8.0
dotnet build src/Core -f net9.0
```

## Публікація для macOS ARM64

Self-contained — із середовищем виконання .NET:

```bash
dotnet publish src/Cli -c Release -r osx-arm64 --self-contained true -o publish/lab02/osx-arm64/self-contained
```

Framework-dependent — використовує встановлене середовище .NET 9:

```bash
dotnet publish src/Cli -c Release -r osx-arm64 --self-contained false -o publish/lab02/osx-arm64/framework-dependent
```

Результати вимірювання через `du -sk` на macOS після публікації:

| RID | Режим | Розмір на диску | Потрібен встановлений runtime |
| --- | --- | --- | --- |
| osx-arm64 | self-contained | 83 124 КіБ (≈81,2 МіБ) | Ні |
| osx-arm64 | framework-dependent | 180 КіБ | Так, .NET 9 |

Розміри можуть змінюватися залежно від версії .NET та файлової системи. Каталог `publish` виключено з Git.

Запуск із каталогу self-contained публікації (з кореня проєкту):

```bash
cd publish/lab02/osx-arm64/self-contained
./Cli
./Cli --json
```

Запуск framework-dependent публікації (з кореня проєкту):

```bash
cd publish/lab02/osx-arm64/framework-dependent
./Cli
./Cli --json
```

Перевірено: збірку Core для обох цільових платформ, збірку Cli та запуск обох публікацій у табличному й JSON-форматах.
