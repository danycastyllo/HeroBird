# CLAUDE_CONTEXT.md — Hero Bird

## Stack

| Tecnología | Versión |
|---|---|
| Unity | 6000.4.1f1 |
| 2D Animation | 14.0.3 |
| 2D Tilemap Editor | 1.0.0 |
| uGUI | 2.0.0 |
| Newtonsoft Json | 3.2.2 |
| In App Purchasing | 4.14.2 |
| AI Navigation | 2.0.11 |
| Multiplayer Center | 1.0.1 |
| Input System | Legacy (Input Manager) |
| Persistencia | PlayerPrefs |
| Lenguaje | C# |

---

## Estructura del proyecto

```
## Estructura del proyecto
Assets/
├── Animaciones/
│   ├── Background/
│   ├── Bird/
│   └── Buttons/ (restart, settings, store)
├── Data/
│   ├── Characters/              ← 45 CharacterData assets
│   └── Auras/                   ← 10 AuraData assets
├── Editor/
│   ├── CharacterDataGenerator.cs
│   ├── SkinManagerAssigner.cs
│   ├── ShopManagerAssigner.cs
│   ├── AuraDataGenerator.cs
│   ├── AssetReferenceFinder.cs  ← busca referencias de un asset seleccionado
│   └── FindUnusedAssets.cs      ← detecta assets sin usar (mejorado en sesión)
├── Prefabs/
│   ├── Birds/
│   ├── coins/
│   └── PLATAFORMAS/Muros G/
├── Scripts/
│   ├── Audio/
│   │   ├── AudioManager.cs
│   │   ├── AudioPool.cs
│   │   ├── Sound.cs
│   │   └── SpriteButton.cs
│   ├── Bird/
│   │   ├── ControlBird.cs
│   │   ├── AuraController.cs
│   │   ├── AuraLoader.cs
│   │   ├── LockParticleRotation.cs
│   │   └── SkinManagerGlobal.cs
│   ├── Core/
│   │   ├── GameController.cs
│   │   └── GameKeys.cs          ← centraliza todas las claves de PlayerPrefs
│   ├── Store/
│   │   ├── ShopManager.cs
│   │   ├── ShopItem.cs
│   │   ├── ShopItemData.cs
│   │   ├── CharacterData.cs
│   │   └── AuraData.cs
│   ├── UI/
│   │   ├── Presionar.cs
│   │   ├── Ajustes.cs
│   │   ├── ComienzoTubos.cs
│   │   ├── Ira.cs
│   │   ├── Volver.cs
│   │   ├── SceneChanger.cs
│   │   ├── Loading.cs
│   │   ├── Finaljuego.cs
│   │   ├── AspectUtility.cs
│   │   └── AnchorToLeftEdge.cs  ← ancla GameObjects al borde izquierdo de cámara
│   └── World/
│       ├── CambiaNivel.cs
│       ├── Coins.cs
│       ├── LimiteObjArriba.cs
│       ├── LimiteObjectIzq.cs
│       ├── LogicaTubos.cs
│       ├── MoverIzq.cs
│       └── Rotar.cs
├── Sprites/
│   ├── Background/
│   ├── Birds/                   ← 45 spritesheets, formato "bird (N).png"
│   ├── Coins/
│   ├── End Game/
│   ├── Game Over/
│   ├── Icons/Auras/
│   ├── Loading Screen/
│   ├── Rock/
│   ├── Store/
│   ├── Tubes/
│   ├── UI/
│   └── Walls/
└── Sound/

---

---

## Patrones y convenciones establecidas

- **ScriptableObjects** como capa de datos para `CharacterData` y `AuraData`
- **Un solo ParticleSystem** por pájaro — data-driven vía `AuraData`
- **GameKeys.cs** centraliza todas las claves de PlayerPrefs como constantes
- **Sprite Atlas V2** — Always Enabled, tres atlas separados:
  - `BirdsAtlas` — 45 personajes, fuente sin compresión, atlas ETC2
  - `UIAtlas` — iconos, tienda, UI, loading, gameover, endgame — Max 512
  - `WorldAtlas` — monedas, rocas, tubos, paredes — Max 2048
- **PlayerPrefs keys** — usar siempre `GameKeys.*`, nunca strings literales:
  - `GameKeys.SelectedCharacter` → itemName del CharacterData
  - `GameKeys.SelectedAura` → itemName del AuraData
  - `GameKeys.PlayerCoins` → int acumulado
  - `GameKeys.Record` → float récord
  - `GameKeys.Sound` → int volumen
  - `GameKeys.CharacterUnlocked` → formato con string.Format
  - `GameKeys.AuraUnlocked` → formato con string.Format
- **Naming de sprites**: `"bird (N)_0"` a `"bird (N)_3"` — formato obligatorio
- **itemName** (no `name`) como identificador en todos los ScriptableObjects
- **World Space** en ParticleSystem + compensación de velocidad `-2f` en X
- **MenuItems de editor** bajo namespace `HeroBird/`
- **Singleton pattern** con guard de duplicados en `SkinManagerGlobal` y `AudioPool`
- **FindAnyObjectByType** solo en `Awake`, nunca en `Update`
- **GetComponent** cacheado en `Awake` en todos los scripts
- **Naming convention**: camelCase para campos privados, PascalCase para clases y propiedades públicas
- **Estructura de estados**: enum `GameState { Menu, Playing, GameOver }` en `GameController`

---

## Interfaces de módulos clave

```csharp
// AuraController
void ApplyAura(AuraData aura)
void StopAura()

// AuraLoader
void ActivateAura()
void DeactivateAura()

// SkinManagerGlobal
void ApplySkin(string skinName)

// ShopManager
void SelectShopItem(ShopItem shopItem)
void TryBuyCharacter(CharacterData character)
void TryBuyAura(AuraData aura)
void PopulateShop(List<ShopItemData> items, Transform contentPanel)
void SwitchTab(ShopTab tab)          // enum ShopTab { Characters, Auras }
void SwitchToCharacters()            // para botones de UI
void SwitchToAuras()                 // para botones de UI

// ShopItem
void Setup<T>(T item, ShopManager manager) where T : ShopItemData
void DisableBuyButton()
void SetSelectedState(bool isSelected)

// AudioManager
void Play(string name)
void Stop(string name)
Sound Find(string name)
void PlaySoundCoins()

// AudioPool
void PlaySound(AudioClip clip)

// GameController
void StartGame()
void RestartGame()

// AnchorToLeftEdge
// Ancla el GameObject padre al borde izquierdo de la cámara en Awake
// Campo: public float offsetX
```

---

## Decisiones de arquitectura

| Decisión | Razón |
|---|---|
| ScriptableObjects para CharacterData y AuraData | Permite referenciar desde múltiples escenas |
| Un solo ParticleSystem por pájaro | Sin prefabs por aura, data-driven, óptimo para mobile |
| `itemName` en vez de `name` | `ScriptableObject` ya hereda propiedad `name`, causa conflicto |
| World Space en ParticleSystem | Local Space mueve partículas con el pájaro |
| Compensación `-2f` en velocidad X | El mundo se mueve, el pájaro no |
| `MaterialPropertyBlock` para textura | Evita instanciar materiales nuevos en cada `ApplyAura` |
| `SkinManagerGlobal` lee frame desde Animator | Evita dependencia frágil en nombres de sprites |
| Sin `AnimatorOverrideController` | 45 personajes hacen inviable crear un override por skin |
| `GameKeys.cs` para PlayerPrefs | Elimina strings literales dispersos, typos causan bugs silenciosos |
| Tres Sprite Atlas separados | Birds/UI/World — evitar atlas único demasiado grande |
| Fuente sin compresión en Birds | El atlas aplica ETC2, comprimir dos veces pierde calidad |
| `AnchorToLeftEdge` en objeto padre | Un solo script posiciona todos los hijos al borde de cámara |
| `ShopTab` enum en ShopManager | Elimina magic strings "Characters" y "Auras" |
| `TryBuy` unificado en ShopManager | `TryBuyCharacter` y `TryBuyAura` comparten lógica base |
| Input Manager legacy mantenido | Migrar a Input System nuevo antes del lanzamiento es riesgo alto |
| `AspectUtility` simplificado | Solo fuerza landscape, adaptación de pantalla pendiente para V2 |

---

## Errores ya resueltos — NO repetir

- `using UnityEditor` en scripts de runtime → rompe build de producción
- `psRenderer.material.mainTexture = ...` → crea instancia de material nueva (memory leak)
- `main.simulationSpace = World` en runtime → no funciona en Unity 6, configurar en Inspector
- Alpha 0 en `AuraData.color` → partículas invisibles
- `lastFrame` optimización en `SkinManagerGlobal` → Animator sobreescribe, quitar optimización
- `name` vs `itemName` en ScriptableObject → conflicto con propiedad heredada
- `LateUpdate` en `SkinManagerGlobal` bloquea animación de muerte → `if (ControlBird.isDead) return`
- `Particle Velocity curves must all be in the same mode` → todos los ejes deben usar mismo modo
- `Cannot modify return value of limitVelocityOverLifetime` → asignar a variable privada en `Awake`
- `TryBuyCharacter` sin validación de monedas → corregido con validación en `TryBuy`
- PlayerPrefs vacío al iniciar → `ApplySkin` usa `characters[0]` como fallback sin warning
- `FindAnyObjectByType` en `Update` o métodos frecuentes → cachear en `Awake`
- `GetComponent` en `Update` → cachear en `Awake`
- Sprite Atlas con texturas fuente comprimidas → warning y pérdida de calidad, usar None en fuente
- `SpriteButton.cs` desactivado accidentalmente → siempre verificar que el componente está activo antes de debuggear
- `ShopManager.playerCoins` privado sin propiedad → agregar `public int PlayerCoins => playerCoins`

---

## Baseline de rendimiento (Vivo Y27 — Helio G85 / Mali-G52)

| Métrica | Baseline inicial | Post Fase 4 |
|---|---|---|
| CPU frame time | 7–22ms | — |
| GPU frame time | 20–42ms | Mejorado |
| SetPass Calls | 11 | — |
| Triangles | 15.6k | — |
| Memoria total | 356MB | 175MB ✅ |
| Texturas | 93 obj / 95MB | 28 obj / 18MB ✅ |

---

## Estado actual por módulo

| Módulo | Estado | Notas |
|---|---|---|
| GameController | ✅ | Refactorizado, enum GameState, campos descriptivos |
| ControlBird | ✅ | Cacheado, lógica redundante eliminada |
| CambiaNivel | ✅ | Constante emptyLevelSlot = 111 |
| AudioManager | ✅ | Null guards en Play/Stop |
| AudioPool | ✅ | Singleton con guard de duplicados |
| ShopManager | ✅ | Enum ShopTab, TryBuy unificado |
| ShopItem | ✅ | Constraint genérico, validación simplificada |
| ShopItemData | ✅ | Sin cambios necesarios |
| CharacterData | ✅ | Sin cambios necesarios |
| AuraData | ✅ | Sin cambios estructurales |
| AuraController | ✅ | Sin cambios estructurales |
| AuraLoader | ✅ | Sin cambios estructurales |
| SkinManagerGlobal | ✅ | Singleton completo, guard lista vacía |
| GameKeys | ✅ | Nuevo — centraliza PlayerPrefs |
| AnchorToLeftEdge | ✅ | Nuevo — adaptación de UI a cualquier pantalla |
| AspectUtility | ⚠️ | Simplificado, adaptación completa pendiente para V2 |
| Loading | ✅ | GetComponent cacheado, clase renombrada |
| LogicaTubos | ✅ | SpawnObstacle extraído como método |
| MoverIzq | ✅ | GetComponent cacheado |
| Presionar | ✅ | Mantenido, funcional |
| Ajustes | ✅ | Renombrado a isOpen |
| Volver | ✅ | Limpiado |
| Ira | ✅ | Código muerto eliminado |
| Finaljuego | ✅ | Limpiado |
| ComienzoTubos | ✅ | Limpiado |
| Rotar | ✅ | PascalCase, limpiado |
| Coins | ✅ | Simplificado |
| LimiteObjArriba | ✅ | Limpiado |
| LimiteObjectIzq | ✅ | Dos variables de rango unificadas |
| BirdsAtlas | ✅ | 45 personajes, ETC2, fuente sin compresión |
| UIAtlas | ✅ | Max 512, High Quality |
| WorldAtlas | ✅ | Max 2048, High Quality |
| CharacterDataGenerator | ✅ | Sin cambios |
| SkinManagerAssigner | ✅ | Sin cambios |
| ShopManagerAssigner | ✅ | Sin cambios |

---

## Pendientes conocidos

- [ ] Asignar `AuraLoader` al campo en Inspector de `GameController`
- [ ] Configurar `Simulation Space → World` en ParticleSystem desde el Inspector
- [ ] Configurar `Sorting Layer` y `Order in Layer` del ParticleSystem sobre el sprite del pájaro
- [ ] Verificar que todos los assets `AuraData` tienen `Alpha = 255` en sus colores
- [ ] Ajustar compensación de velocidad X si la velocidad del mundo cambia (hardcoded `-2f`)
- [ ] Poblar campos de `AuraData` assets (particleSprite, colorMode, intensity, etc.)
- [ ] Asignar `AuraData` assets al `AuraLoader` en Inspector (lista `allAuras`)
- [ ] Precio de los 45 `CharacterData` — generados con `100` por defecto, revisar individualmente
- [ ] Sistema de adaptación de pantalla completo para tablets y distintas resoluciones (V2)
- [ ] Optimización de resolución de render para GPU (bajar al 75% de resolución nativa)
- [ ] Optimización de Audio — Load Type para mobile
- [ ] Fase 5 — Build de producción y checklist de lanzamiento
- [ ] Multiplayer (V2 — no implementar en V1)
