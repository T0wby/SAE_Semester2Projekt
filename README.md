# SAE_Semester2Projekt

## Simulation visuelle Eigenschaften:
Zur Simulation von visuellen Eigenschaften, habe ich interaktiven Schnee und Sand
implementiert. Zudem gibt es eine Wellen Simulation einer sogenannten “Gerstner Wave”,
die über einen Compute Shader berechnet wird.
Shader und Skripte können im Projekt unter dem VFX Ordner gefunden werden.
- Schnee Boden
- Sand Boden
- Wellen

## Simulation visuelle Effekte:
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
