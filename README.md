# Lost In Transit
Lost in Transit is a mod for Risk of Rain 2 that adapts content from Risk of Rain 1, intending to balance and redesign varying features in ways that preserve that best original identites while also respecting the balance and gameplay changes made in Risk of Rain 2.

In its current state, the mod is intended to be an adaptation over a pure port - some features may have changed function or identities for varying reasons. This is done to help players less familiar with Risk of Rain 1's content ease into it and keep things simple. 

If you have any issues with the mod, any feedback you'd like to give, any ideas for new content, or would like to follow progress, you can either join the Lost in Transit Discord Server (https://discord.gg/jzxXsQEZ5y) or you can contact me on Discord (swuff★#2224) - shoot me a DM or ping me in a mutual server. The mod is in active development, so expect new features soon. And as a personal note: I'm still fairly new to modding, so expect issues. 

## Items / Equipment

| Icon | Item | Description | Rarity |
|:-|-|------|-|
|![](https://i.imgur.com/Vqj1kyK.png) | **Life Savings** | Keep 2% (+2% per stack) of gold between stages. | Common |
|![](https://i.imgur.com/0JpFYMD.png) | **Mysterious Vial** | Regenerate an extra 0.4 (+0.4 per stack) hp per second. | Common |
|![](https://i.imgur.com/zTCttJR.png) | **Beckoning Cat** | Elite monsters have a 4.5% (+1.5% per stack) chance to drop items on death. | Uncommon |
|![](https://i.imgur.com/4qpaGr0.png) | **Smart Shopper** | Monsters drop 25% (+25% per stack) more gold. | Uncommon |
|![](https://i.imgur.com/16yqiHX.png) | **Thallium** | 10% chance to inflict thallium poisoning for 500% (+250% per stack) of enemy's base damage and slow by 75%. | Rare |
|![](https://i.imgur.com/xmQADqk.png) | **Telescopic Sight** | Gain 10% critical chance. 10% (+10% per stack) chance for critical strikes to deal 5x damage. | Rare |
|![](https://i.imgur.com/27uyOZz.png) | **Gigantic Amethyst** | Reset skill cooldowns on use. | Equipment |

## Elites

| Elite Type | Name | Description | Tier |
|:-|-|------|-|
|![](https://i.imgur.com/CubhqEH.png) | **Leeching** | Damage dealt is returned as healing. Periodically heal nearby allies for a small amount of health. | 1 |

## Credits
* Code - Nebby, swuff★
* Art/Modelling/Animation -  bruh, GEMO, LucidInceptor
* Writing - BlimBlam, Lyrical Endymion, QandQuestion, swuff★, T_Dimensional
* Additional support/special thanks - Anreol, Kevin, KomradeSpectre, Nebby, rob, xpcybic

## Changelog

### 0.1.0
*First ThunderStore release!*
* Items
 * Life Savings: No longer preserves any gold when travelling between Hidden Realms
 * Smart Shopper: Added

### 0.0.4
* General
 * Added a proper .lang file to un-"Tower of Babel" the mod
* Items
 * Life Savings: Added
  *Developer note: Another changed item. The original Life Savings worked fine in Risk of Rain 2, but felt extremely boring to play with. Instead of passive gold, the item now allows you to keep a small amount of your gold between stages. Hoping this makes the item much more desirable.*
* Elites
 * Reduced Leeching Elite healing AoE from 256m to 20m. Oops

### 0.0.3
* Elites
 * Turns out Leeching Elites weren't actually doing anything before - given an itemBehavior so they start doing stuff, now
 * Buffed the on-hit healing of Leeching Elites significantly (removed x0.25 multiplier)
 * Leeching Elites now create a tracer to nearby monsters when healing them

### 0.0.2
* General
 * Added FixPluginTypesSerialzation as a dependency
 * Fixed issue where mod wouldn't load without Aspect Abilities installed

### 0.0.1
*Should probably start keeping track of version #'s - this is the first ThunderKit release, so let's start here.*
* Items
 * Beckoning Cat: Added
	 *Developer note: This item was originally the "56 Leaf Clover" in Risk of Rain 1, but was changed due to the inclusion of "57 Leaf Clover" in Risk of Rain 2. To help prevent confusion betwen the two, the item was given a complete overhaul - may provide a config option to restore this into the 56 Leaf Clover if there's demand for it.*
	* Mysterious Vial: Added
	* Telescopic Sight: Added
 	*Developer note: The original Telescopic Sight is widely considered an overpowered and unfun item. The current implementation instead offers players a 10% chance to quintuple their damage on crit. That said, I'm not happy with this implementation and am looking into alternatives that both better preserve the item's identity, and are more interesting to play with.*
	* Thallium: Added
* Elites
	* Added Leeching Elites
