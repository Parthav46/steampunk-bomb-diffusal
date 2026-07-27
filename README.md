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
- .NET target: net8.0

## Run the Game

### Option 1: Godot Editor

1. Open the project in Godot 4.7 (C#/.NET enabled).
2. Run the main scene.

### Option 2: Build from CLI

From the project root:

```bash
dotnet build
```

Then run from Godot or your preferred .NET/Godot workflow.
