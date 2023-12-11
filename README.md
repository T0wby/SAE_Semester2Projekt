# SAE_Semester2Projekt


## Simulation
### Simulation visuelle Eigenschaften:
Zur Simulation von visuellen Eigenschaften, habe ich interaktiven Schnee und Sand
implementiert. Zudem gibt es eine Wellen Simulation einer sogenannten “Gerstner Wave”,
die über einen Compute Shader berechnet wird.
Shader und Skripte können im Projekt unter dem VFX Ordner gefunden werden.
- Schnee Boden
- Sand Boden
- Wellen

### Simulation visuelle Effekte:
Zur Simulation von visuellen Effekten, habe ich viele einzelne Teile benutzt.
Zuerst gibt es viele verschiedene Portale, die jeweils über eine Render Textur, das
Teleportations Ziel live anzeigen.
Jede einzelne Region besitzt dann auch noch ein eigenes Volume, mit verschiedenen
Eigenschaften, die sich unter Umständen auch in Runtime ändern können.
Weitere visuelle Effekte in bestimmten Orten sind auch:
- Schnee(schneien)
- Sandtornado
- Sandsturm
- Feuer + Rauch
  
Shader und Skripte können auch hier im Projekt unter dem VFX Ordner gefunden werden,
mit Ausnahme der Skripte, die auf den Portalen liegen. Diese befinden sich im Portal Ordner
unter Scripts

## AI
### SnowAI
- Bewegen sich zum Spieler, solange sich dieser im Blickwinkel befindet.
- Befinden sich in der SnowArea.
### HideAI
- Versteckt sich hinter Objekten, sobald der Spieler in das Sichtfeld gerät.
- Befinden sich in der HideArea.
### Animals
- Laufen zufällig herum
- Essen, Trinken und Paaren sich, sobald bestimmte Werte zu niedrig oder
hoch sind.
- Befinden sich in der AnimalArea.
### Flocking
- Mehrere Boids fliegen in einem Vogelschwarm. Dabei werden die
Berechnungen der einzelnen Boids über einen Compute Shader an die GPU
ausgelagert.
- Befinden sich in der WaterArea.

### Skripte
Alle KI-Skripte befinden sich unter Assets/Scripts/AI im Projekt.
Die Skripte für die ScriptableObjects der KI befinden sich unter Assets/ScriptableObjects /AI.

## Terrain Editor
### Beschreibung:

Bei meinem Terrain Editor handelt es sich um eine Planetengenerierung. Im Groben erstelle
ich dabei zuerst einen Würfel und berechne danach je nach gegebenem Radius die
einzelnen Vertex Positionen um, damit sich eine Sphere formt. Danach benutze ich
verschiedene Noise, um die Sphere weiter zu formen und mehr wie Planeten aussehen zu
lassen. Zum Schluss wird dann noch ein Gradient benutzt, um den Planeten verschiedene
Farben per Höhe zu geben.
### Speicherung:

Die Daten für die Form und Farbe der Planeten werden in 2 verschiedenen
ScriptableObjects(SO) abgespeichert. In sogenannten ShapeSettings und ColourSettings.
Zur Erleichterung der Bearbeitung der einzelnen Settings, habe ich noch über ein
Editor-Skript gesorgt, dass die einzelnen ScriptableObjects im Inspektorfenster selbst
bearbeitet werden können, nachdem man diese an das Skript für den Planeten angehängt
hat. Dadurch ist kein ständiges wechseln von SO zu SO mehr nötig.

Alle Skripte befinden sich unter Assets/Scripts/Planet
