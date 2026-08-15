# Steampunk Bomb Diffusal

Steampunk Bomb Diffusal is a fast, pattern-reading mini-game built with Godot 4 and C#.
You are trying to identify the one gauge that drives the entire mechanism before the timer meter reaches maximum.

## Core Objective

Each round spawns a dashboard with 5 spinning gauges and a timer meter.

- Your goal is to click the driving gauge.
- If you click the correct gauge, the bomb is defused and the next round starts.
- If the timer meter fills, the bomb explodes and the run ends.

## How It Works

At the start of every round, all 5 gauges are assigned a randomized internal order.
That order determines which gauge is the true driver and how speed propagates through the system.

When you click a non-driving gauge:

- One attempt is consumed.
- That gauge is marked as diffused.
- Rotation speeds of downstream gauges are recalculated.
- The timer speed also changes based on the updated chain.

This means each wrong choice gives new information, but also changes the behavior you are trying to read.

## Hints (In-Game Rule)

The key observation is:

- The driving gauge does not change speed when other gauges are stopped.

Use this to test candidates and infer which gauge is upstream from all others.

## Attempts and Failure

- You start each round with 3 attempts (shown as pin icons).
- Every click consumes one attempt, including the correct click.
- After attempts are gone, further clicks are ignored.
- The round is still running until either:
	- you already clicked the driver (success), or
	- the timer meter reaches its limit (failure).

## Run Flow

1. Press Start on the title screen.
2. Play consecutive rounds, one bomb at a time.
3. Each successful defusal increments your run count.
4. On explosion, a kill screen shows how many bombs you defused.
5. After a short delay, the game resets to the start screen.

## Controls

- Mouse Left Click: click a gauge to attempt a defusal.

## Tech Stack

- Engine: Godot 4.7
- Language: C#
- .NET target: net10.0
- 2dog host: desktop and browser hosts via NuGet packages
- Test framework: xUnit via 2dog/xUnit integration

## Project Layout

- `sbd.csproj` — the Godot game project
- `sbd.2dog/` — desktop host for local debugging and running
- `sbd.web/` — browser host for WebAssembly publishing
- `sbd.tests/` — automated tests

## Required Dependencies

Install the required SDK and tooling before building or publishing:

```bash
dotnet --version
```

This project pins .NET 10 via `global.json`:

```bash
dotnet restore sbd.slnx
```

For the browser host, install the WebAssembly workload:

```bash
dotnet workload install wasm-tools
```

The project also uses 2dog packages referenced from the host projects:

- `2dog.engine`
- `2dog.browser-wasm`
- `2dog.xunit`


## Run the Game

### Option 1: Godot Editor

1. Open the project in Godot 4.7 with C#/.NET support enabled.
2. Open the project root and run the main scene.

### Option 2: Desktop debug via 2dog

From the project root:

```bash
dotnet restore sbd.slnx
dotnet build sbd.slnx
dotnet run --project sbd.2dog --configuration Debug
```

### Option 3: Publish web build

From the project root:

```bash
dotnet publish sbd.web -c Release
```

Then serve the generated AppBundle locally with a simple static server, such as VS Code Live Server or any local file server (for example, Python's built-in HTTP server):

```bash
cd sbd.web/AppBundle
python3 -m http.server 8000
```

Then open `http://localhost:8000` in a browser.

## Licensing

This project is multi-licensed:
* **Code**: All source code is licensed under the [MIT License](LICENSE-CODE).
* **Assets**: All original visual graphics, music, sound effects, and multimedia assets created for this project are licensed under the [Creative Commons Attribution 4.0 International (CC BY 4.0) License](LICENSE-ASSETS), excluding the third-party font files in [assets/font](assets/font), which retain their own licensing terms.

### Attribution
If you use the assets from this repository, you must give appropriate credit. 
Example attribution: "Assets by Parthav Patel, used under CC BY 4.0."
