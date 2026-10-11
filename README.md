# Unity Leaderboard System

A dynamic and animated leaderboard system built in Unity. Designed for use in games to visually present player rankings, avatars, and stats with flair.

---

![Screenshot](https://github.com/PiotrCynowski/AnimatedLeaderboard/blob/master/Pics/gameplay.png?raw=true)

---

## How to Run
- `Scenes/Main.unity` – The main scene to run the leaderboard demo.
- Ensure you're testing in **Simulator View**, **not Game View**. The UI is built with dynamic sizing and animations optimized for simulator preview.

---

## Controls

- Tap or click anywhere to **reveal the leaderboard**.
- Press the **exit button** to generate a new randomized leaderboard entry set.

## Features

- Dynamic UI scaling for leaderboard entries.
- Object pooling for efficient UI element reuse.
- Color-coded avatars and flag display.
- Podium decoration for top rankings.
- Editor tool to generate random JSON leaderboard files.

---

## Developer Tools

- A custom editor utility is included:  
**Tools > User Entry Json Generator**  
This lets you:
- Load a base JSON file.
- Randomly generate X number of players.
- Automatically assign it to the `GameManager` for runtime testing.

---

## Dependencies

- **DOTween** – Used for smooth UI animations.
- **Input System** – Handles tap/click input for simulator interactions.
  Make sure DOTween is installed and setup via:
  Tools > Demigiant > DOTween Utility Panel > Setup DOTween...

## Notes

- Sprite assets (e.g., flags, avatars, podiums) are managed via `LeaderboardSpriteSelector` ScriptableObjects.
- If JSON assignment fails at runtime, ensure `Resources/` contains valid JSON files and GameManager is initialized correctly.

---
