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
Assets/
├── Animaciones/
│   ├── Background/
│   ├── Bird/
│   └── Buttons/ (restart, settings, store)
├── Data/                        ← ScriptableObjects (creado en esta sesión)
│   ├── Characters/              ← 45 CharacterData assets
│   └── Auras/                   ← 10 AuraData assets
├── Editor/                      ← Scripts de editor (creados en esta sesión)
│   ├── CharacterDataGenerator.cs
│   ├── SkinManagerAssigner.cs
│   └── ShopManagerAssigner.cs
├── Prefabs/
│   ├── Birds/
│   ├── coins/
│   └── PLATAFORMAS/Muros G/
├── Scripts/
│   ├── Auras/
│   │   ├── AuraData.cs
│   │   ├── AuraController.cs
│   │   └── AuraLoader.cs
│   ├── Botones/
│   ├── juego/
│   │   └── CambiaNivel.cs
│   ├── Music and Sounds/
│   │   └── AudioManager.cs
│   └── Store/
│       ├── ShopItemData.cs
│       ├── CharacterData.cs
│       ├── AuraData.cs
│       ├── ShopItem.cs
│       ├── ShopManager.cs
│       └── SkinManagerGlobal.cs
├── Sprites/Birds/               ← 45 spritesheets, formato "bird (N).png"
└── Sound/
```

---

## Patrones y convenciones establecidas

- **ScriptableObjects** como capa de datos para `CharacterData` y `AuraData`
- **Un solo ParticleSystem** por pájaro — data-driven vía `AuraData`
- **PlayerPrefs keys** en uso:
  - `"SelectedCharacterName"` → itemName del CharacterData
  - `"SelectedAuraName"` → itemName del AuraData
  - `"PlayerCoins"` → int acumulado
  - `"record"` → float récord
  - `"Sound"` → int volumen
  - `"Character_{itemName}_Unlocked"` → int 0/1
  - `"Aura_{itemName}_Unlocked"` → int 0/1
- **Naming de sprites**: `"bird (N)_0"` a `"bird (N)_3"` — formato obligatorio
- **itemName** (no `name`) como identificador en todos los ScriptableObjects
- **World Space** en ParticleSystem + compensación de velocidad `-2f` en X
- **MenuItems de editor** bajo namespace `HeroBird/`

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
void UnlockItem(ShopItemData item, string prefix)
void PopulateShop(List<ShopItemData> items, Transform contentPanel)
void SwitchTab(string tabName)         // "Characters" | "Auras"

// ShopItem
void Setup<T>(T item, ShopManager manager)
void DisableBuyButton()
void SetSelectedState(bool isSelected)

// AudioManager
void Play(string name)
Sound Find(string name)
void PlaySoundCoins()
```

---

## Decisiones de arquitectura

| Decisión | Razón |
|---|---|
| ScriptableObjects para CharacterData y AuraData | Permite referenciar desde múltiples escenas (tienda + gameplay) |
| Un solo ParticleSystem por pájaro | Sin prefabs por aura, data-driven, óptimo para mobile |
| `itemName` en vez de `name` | `ScriptableObject` ya hereda propiedad `name`, causa conflicto |
| World Space en ParticleSystem | Local Space mueve partículas con el pájaro, no genera estela |
| Compensación `-2f` en velocidad X | El mundo se mueve, el pájaro no — las partículas deben compensar |
| `MaterialPropertyBlock` para textura | Evita instanciar materiales nuevos en cada `ApplyAura` |
| `SkinManagerGlobal` lee frame desde Animator | Evita dependencia frágil en nombres de sprites |
| Sin `AnimatorOverrideController` | 45 personajes hacen inviable crear un override por skin |
| `InvokeRepeating(nameof(...))` en CambiaNivel | Refactor-safe vs string literal |
| Dos listas separadas en ShopManager | `List<CharacterData>` y `List<AuraData>` con cast a `ShopItemData` |
| `AuraLoader` activado desde `GameController` | Evita aura estática durante posicionamiento inicial (3 seg) |
| `intensity` como único slider de aura | El comportamiento específico lo maneja AuraController internamente |

---

## Errores ya resueltos — NO repetir

- `using UnityEditor` en `ShopManager` y `SkinManagerGlobal` → rompe build de producción
- `psRenderer.material.mainTexture = ...` → crea instancia de material nueva (memory leak)
- `main.simulationSpace = World` en runtime → no funciona en Unity 6, configurar en Inspector
- Alpha 0 en `AuraData.color` → partículas invisibles
- `lastFrame` optimización en `SkinManagerGlobal` → Animator sobreescribe, quitar optimización
- `name` vs `itemName` en ScriptableObject → conflicto con propiedad heredada
- `LateUpdate` en `SkinManagerGlobal` bloquea animación de muerte → agregar `if (ControlBird.isDead) return`
- `Particle Velocity curves must all be in the same mode` → todos los ejes deben usar mismo modo de curva
- `Cannot modify return value of limitVelocityOverLifetime` → asignar a variable privada en `Awake`
- `TryBuyCharacter` sin validación de monedas → corregido con `if (playerCoins >= price)`
- PlayerPrefs vacío al iniciar → `ApplySkin` usa `characters[0]` como fallback sin warning

---

## Estado actual por módulo

| Módulo | Estado | Notas |
|---|---|---|
| GameController | ✅ | Integrado con AuraLoader |
| ControlBird | ✅ | Sin cambios estructurales |
| CambiaNivel | ✅ | Se dejó el original, funcionaba correctamente |
| AudioManager | ✅ | Sin cambios, `FindAnyObjectByType` aceptado |
| ShopManager | ✅ | ScriptableObjects, validación monedas, tabs |
| ShopItem | ✅ | Auto-select al comprar, color por colorMode |
| ShopItemData | ✅ | ScriptableObject base |
| CharacterData | ✅ | ScriptableObject + spritesheet[4] |
| AuraData | ✅ | ScriptableObject + 7 tipos + colorMode + parámetros avanzados |
| AuraController | ✅ | 7 comportamientos, MaterialPropertyBlock, World Space |
| AuraLoader | ✅ | Activado/desactivado desde GameController |
| SkinManagerGlobal | ✅ | Lee Animator, 45 skins, idle + death correctos |
| CharacterDataGenerator | ✅ | Crea 45 assets con sprites automáticamente |
| SkinManagerAssigner | ✅ | Asigna characters ordenados por número |
| ShopManagerAssigner | ✅ | Asigna characters y auras en una operación |

---

## Pendientes conocidos

- [ ] Asignar `AuraLoader` al campo en Inspector de `GameController`
- [ ] Configurar `Simulation Space → World` en ParticleSystem desde el Inspector (no por código)
- [ ] Configurar `Sorting Layer` y `Order in Layer` del ParticleSystem sobre el sprite del pájaro
- [ ] Verificar que todos los assets `AuraData` tienen `Alpha = 255` en sus colores
- [ ] Ajustar compensación de velocidad X si la velocidad del mundo cambia (actualmente hardcoded `-2f`)
- [ ] Poblar campos de `AuraData` assets (particleSprite, colorMode, intensity, etc.) para las 10 auras
- [ ] Asignar `AuraData` assets al `AuraLoader` en Inspector (lista `allAuras`)
- [ ] Precio de los 45 `CharacterData` — generados con `100` por defecto, revisar individualmente
- [ ] Build de producción — verificar que no queda ningún `using UnityEditor` en scripts de runtime
- [ ] Multiplayer (V2 — no implementar en V1)
