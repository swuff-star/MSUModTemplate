# Lost In Transit
Lost in Transit is a mod for Risk of Rain 2 that adapts content from Risk of Rain 1, intending to balance and redesign varying features in ways that preserve that best original identites while also respecting the balance and gameplay changes made in Risk of Rain 2.

In its current state, the mod is intended to be an adaptation over a pure port - some features may have changed function or identities for varying reasons. This is done to help players less familiar with Risk of Rain 1's content ease into it and keep things simple. 

If you have any issues with the mod, any feedback you'd like to give, any ideas for new content, or would like to follow progress, you can either join the Lost in Transit Discord Server (https://discord.gg/jzxXsQEZ5y) or you can contact me on Discord (swuff★#2224) - shoot me a DM or ping me in a mutual server. The mod is in active development, so expect new features soon. And as a personal note: I'm still fairly new to modding, so expect issues. 

## Items / Equipment

| Icon | Item | Description | Rarity |
|:-|-|------|-|
|![](https://i.imgur.com/Vqj1kyK.png) | **Life Savings** | Keep 5% (+2.5% per stack) of gold between stages. | Common |
|![](https://i.imgur.com/0JpFYMD.png) | **Mysterious Vial** | Regenerate an extra 0.8 (+0.8 per stack) hp per second. | Common |
|![](https://i.imgur.com/zTCttJR.png) | **Beckoning Cat** | Elite monsters have a 4.5% (+1.5% per stack) chance to drop items on death. | Uncommon |
|![](https://i.imgur.com/L4TZX13.png) | **Golden Gun** | Deal extra damage based on held gold, up to +40% damage (+20% per stack) at 600 gold (+300 per stack, scaling with time). | Uncommon |
|![](https://i.imgur.com/oI6SJMs.png) | **Prison Shackles** | Slow monsters for -30% attack speed for 2 (+2 per stack) seconds. | Uncommon |
|![](https://i.imgur.com/4qpaGr0.png) | **Smart Shopper** | Monsters drop 25% (+25% per stack) more gold. | Uncommon |
|![](https://i.imgur.com/16yqiHX.png) | **Thallium** | 10% chance to inflict thallium poisoning for 500% (+250% per stack) of enemy's base damage and slow by 75%. | Rare |
|![](https://i.imgur.com/xmQADqk.png) | **Telescopic Sight** | 1% (+0.5% per stack) chance to instakill monsters. Boss monsters instead take 20% of their maximum health in damage. Recharges every 20 (-2 per stack) seconds. | Rare |
|![](https://i.imgur.com/27uyOZz.png) | **Gigantic Amethyst** | Reset skill cooldowns on use. | Equipment |

## Elites

| Elite Type | Name | Description | Tier |
|:-|-|------|-|
|![](https://i.imgur.com/O7976kP.png) | **Frenzying** | Increased attack and move speed. Periodically dash towards enemies at high speeds. | 1 |
|![](https://i.imgur.com/CubhqEH.png) | **Leeching** | Damage dealt is returned as healing. Periodically heal nearby allies for a small amount of health. | 1 |
|![](https://i.imgur.com/Ze8oyIg.png) | **Volatile** | Attacks explode on hit. Occasionally drop Spite bombs when damaged. | 1 |
|![](https://i.imgur.com/Xrn4MvE.png) | **Blighted** | An unknown force of the Planet, these phantasms are as mysterious as they are deadly. | ? |

## Credits
* Code - Nebby, swuff★
* Art/Modelling/Animation - bruh, GEMO, LucidInceptor, Nebby, SOM
* Sound - UnknownGlaze, neik, SOM
* Writing - BlimBlam, Lyrical Endymion, QandQuestion, swuff★, T_Dimensional
* Additional support/special thanks - Draymarc, KevinFromHPCustomerService, KomradeSpectre, Moffein, rob, Shared, TheTimesweeper, xpcybic, /vm/

## Changelog

### 0.3.6
* Items
	* Prison Shackles: Reduced attack speed reduction (60% -> 30%)
* Elites
	* Leeching: Increased time between healing bursts from 10 to 15 seconds
	* Leeching: Now have a visible window before healing, accompanied by an AoE radius
	* Leeching: Damaging a Leeching elite during the aforementioned window now reduces total healing done, up to 50%
	* Leeching: Reduced particle count on VFX
	* Volatile: Added cooldown between bombs appearing
	* Volatile: Maximum bomb count halved
	* Volatile: Bomb damage halved
	* Blighted: Fixed bug where spawn rate didn't reset in-between runs
	* Blighted: Replaced configurable blacklist with a configurable weight system
	* Blighted: Teleporter bosses can now be Blighted, for double the cost (also configurable)
	* Blighted: Removed SFX from smoke visual
	* Blighted: Lowered CDR (50% -> 25%)
	* Blighted: Slightly lowered damage boost (-25%), but raised attack speed (+110%)
	* Blighted: Smoke effect no longer spawns when the aspect is held by a player

### 0.3.5
* Elites
	* Blighted: Added config to allow bosses to spawn as Blighted enemies
	* Blighted: Fixed invisibility

### 0.3.4
*Developer Note: PLEASE wipe your config.*
* Items
	* Beckoning Cat: Added On-Proc sound effect
	* Bitter Root: Temporariliy disabled. Go join Mortar Tube!
	* Gigantic Amethyst: Added On-Use sound effect
	* Golden Gun: Hopefully tackled some issues with how the number of buffs was calculated
	* Telescopic Sight: Added On-Proc sound effect
	* Thallium: Added On-Proc sound effect
* Equipments
	* Gigantic Amethyst: Added on use sound effect
* Elites
	* Blighted: Lowered extra damage (300% -> 200%)
	* Blighted: Changed how visibility / invisiblity is handled to make them visible for longer periods of time
	* Blighted: Should be less common overall.
	* Leeching: Added a sound effect to their AoE heal
	* Frenzied: Added sound effect when Dash is ready
	* Volatile: Added missing IDRS
* BugFixes:
	* Volatile elites should no longer cause a myriad of issues, included but not limited to:
		* Volatile Bombs exploding constantly rarely. [link](https://github.com/swuff-star/LostInTransit/issues/5)
		* Glacial Elites's freezing explosion constantly triggering. [link](https://github.com/swuff-star/LostInTransit/issues/4)
		* Volatile Elites should no longer cause infinite explosions when interacting with Enforcer's shield. [link](https://github.com/swuff-star/LostInTransit/issues/8)
		* Causing the camera to get stuck while having aetherium installed.
	* Disabling blighted elites should no longer cause extreme stat scaling bugs. [link](https://github.com/swuff-star/LostInTransit/issues/2)
	* Prison Shackles should no longer trigger on attacks with proc coefficient of 0. [link](https://github.com/swuff-star/LostInTransit/issues/7)

### 0.3.3
* Items
	* Golden Gun: Fixed the Item Display causing a ton of null reference exceptions, alongside causing invisible enemies.
	
### 0.3.2
* General
	* Fixed the readme file not having the proper description for the new Telescopic Sight
* Elites
	* Blighted: Fixed smoke particles lingering indefinetly.
	* Blighted: Fixed spawn sound effect not playing.
	* Frenzying: New ramp is now properly applied.
	* Leeching: New ramp is now properly applied.
	* Volatile: New ramp is now properly applied.
	* Volatile: Fixed crown not having proper materials.
* Items
	* Life Savings: I swear it should work now

### 0.3.1
* General
	* No, this is not a readme update disguised as fix
* Items
	* Prison Shackles: Fixed icon

### 0.3.0
*I strongly recommend deleting old config and starting fresh. I think things should update either way, but am not sure. If values seem off in game - try this before reporting it to me.*
* Items
	* Added proper item tags for items (*Developer note: Apparently none of these items have been tagged properly, allowing Life Savings to show up in Healing chests and other similar occurances. They're all sorted nicely, now.*)
	* AI Blacklisted certain items that the AI should never get for balance reasons (e.g. Telescopic Sight) or technical reasons (e.g. Golden Gun)
	* Added item displays for Mysterious Vial, Beckoning Cat, Telescopic Sight, and Golden Gun for Vanilla characters. Expect the rest next update
	* Items now properly reflect config-based changes in their descriptions. This is mostly complete and doesn't support some more complicated config options e.g. Beckoning Cat Luck, Telescopic Sight config
	* Bitter Root: Added (*Developer note: I'm not happy with this, honestly. I kind of want to make it a consumeable like the DLC's "Elixir", but I don't feel like it'll be well received. Thoughts?*)
	* Life Savings: Rewritten entirely and should now be networked, we hope
	* Life Savings: Stat change: 5% of money kept from previous stage, +2.5% per stack
	* Prison Shackles: Added (*Developer note: Shoutout to my lovely friend who had the idea for this. I think it's a simple change that makes it just as valuable as another beloved Uncommon item ;) Enjoy!*)
	* Telescopic Sight: Reworked -> Attacks have a 1% (+0.5% per stack) chance to instakill enemies. Bosses recieve 20% of their maximum health as damage rather than instantaneous death. Has a 20 second (-2 seconds per stack) cooldown. (*Developer Note: I think I'm happy with this rework? Lemme know how it feels. I don't want it to be a crit-based item anymore, and I've heard some interesting ideas that drop the old idea entirely that'd make it a unique utility - if people aren't happy with this, I could try that next?*)
	* Thallium: Slightly reworked how scaling works; very technical change to numbers, but stacking is now done by increasing duration rather than damage. Insanely minor numbers nerf that's negligible - can change back to old behavior via config if desired.*)
* Elites
	* Frenzying: Updated buff icon
	* Frenzying: Updated elite ramp
	* Frenzying: Added a visual indicator when dash is ready
	* Leeching: Updated buff icon
	* Leeching: Updated elite ramp
	* Volatile: Added
	* Blighted: Added (*Developer note: These guys use a unique spawning method. Do they feel too common? Too strong? Too visible?)
* Other
	* Updated "ELITE_MODIFIER_GOLD" from "Gold" to "Auric". I like how it sounds since it comes from the same root Aurelionite does. No other mod fixes this and it's too small a change to warrant its own mod. Golden Coast Plus, maybe?

(Old changelogs can be found [Here](https://github.com/swuff-star/LostInTransit/blob/master/OldChangelogs.md))
