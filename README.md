# Lost In Transit
Lost in Transit is a mod for Risk of Rain 2 that adapts content from Risk of Rain 1, intending to balance and redesign varying features in ways that preserve that best original identites while also respecting the balance and gameplay changes made in Risk of Rain 2.

In its current state, the mod is intended to be an adaptation over a pure port - some features may have changed function or identities for varying reasons. This is done to help players less familiar with Risk of Rain 1's content ease into it and keep things simple. 

If you have any issues with the mod, any feedback you'd like to give, any ideas for new content, or would like to follow progress, you can either join the Lost in Transit Discord Server (https://discord.gg/jzxXsQEZ5y) or you can contact me on Discord (swuff★#2224) - shoot me a DM or ping me in a mutual server. The mod is in active development, so expect new features soon. And as a personal note: I'm still fairly new to modding, so expect issues. 

## Items / Equipment

| Icon | Item | Description | Rarity |
|:-|-|------|-|
|![](https://i.imgur.com/Vqj1kyK.png) | **Life Savings** | Keep 4% (+4% per stack) of gold between stages. | Common |
|![](https://i.imgur.com/0JpFYMD.png) | **Mysterious Vial** | Regenerate an extra 0.8 (+0.8 per stack) hp per second. | Common |
|![](https://i.imgur.com/zTCttJR.png) | **Beckoning Cat** | Elite monsters have a 4.5% (+1.5% per stack) chance to drop items on death. | Uncommon |
|![](https://i.imgur.com/L4TZX13.png) | **Golden Gun** | Deal extra damage based on held gold, up to +40% damage (+20% per stack) at 600 gold (+300 per stack, scaling with time). | Uncommon |
|![](https://i.imgur.com/4qpaGr0.png) | **Smart Shopper** | Monsters drop 25% (+25% per stack) more gold. | Uncommon |
|![](https://i.imgur.com/16yqiHX.png) | **Thallium** | 10% chance to inflict thallium poisoning for 500% (+250% per stack) of enemy's base damage and slow by 75%. | Rare |
|![](https://i.imgur.com/xmQADqk.png) | **Telescopic Sight** | Gain 10% critical chance. 10% (+10% per stack) chance for critical strikes to deal 5x damage. | Rare |
|![](https://i.imgur.com/27uyOZz.png) | **Gigantic Amethyst** | Reset skill cooldowns on use. | Equipment |

## Elites

| Elite Type | Name | Description | Tier |
|:-|-|------|-|
|![](https://i.imgur.com/O7976kP.png) | **Frenzying** | Increased attack and move speed. Periodically dash towards enemies at high speeds. | 1 |
|![](https://i.imgur.com/CubhqEH.png) | **Leeching** | Damage dealt is returned as healing. Periodically heal nearby allies for a small amount of health. | 1 |

## Credits
* Code - Nebby, swuff★
* Art/Modelling/Animation - bruh, GEMO, LucidInceptor, SOM
* Writing - BlimBlam, Lyrical Endymion, QandQuestion, swuff★, T_Dimensional
* Additional support/special thanks - Draymarc, KevinFromHPCustomerService, KomradeSpectre, Moffein, rob, Shared, TheTimesweeper, xpcybic, /vm/

## Changelog

### 0.3.0
*I strongly recommend deleting old config and starting fresh. I think things should update either way, but am not sure. If values seem off in game - try this before reporting it to me.*
* Items
	* Added proper item tags for items (*Developer note: Apparently none of these items have been tagged properly, allowing Life Savings to show up in Healing chests and other similar occurances. They're all sorted nicely, now.*)
	* AI Blacklisted certain items that the AI should never get for balance reasons (e.g. Telescopic Sight) or technical reasons (e.g. Golden Gun)
	* Items now properly reflect config-based changes in their descriptions. This is mostly complete and doesn't support some more complicated config options e.g. Beckoning Cat Luck, Telescopic Sight config
	* Bitter Root: Added (*Developer note: I'm not happy with this, honestly. I kind of want to make it a consumeable like the DLC's "Elixir", but I don't feel like it'll be well received. Thoughts?*)
	* Life Savings: Rewritten entirely and should now be networked, we hope
	* Life Savings: Stat change: 5% of money kept from previous stage, +2.5% per stack
	* Prison Shackles: Added (*Developer note: Shoutout to my lovely friend who had the idea for this. I think it's a simple change that makes it just as valuable as another beloved Uncommon item ;) Enjoy!*)
	* Repulsion Armor: Added (*Developer note: I'd like to eventually make it so projectiles bounce off entirely, but it also sounds like a bitch to do.*)
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

### 0.2.0
* General
	* Added MoonstormSharedUtils as a dependency
* Items
	* Beckoning Cat: Item drops off of slain enemy position rather than player position. Will reintroduce player position drop behavior in future config
	* Golden Gun: Added (*Developer note: This item's scaling has been changed semi-significantly. In Risk of Rain 1, it capped at 700 gold, with stacks decreasing this capacity. In Lost in Transit, stacks increase the max damage output by +20% per, but also increase the cap by +300 gold per. Cap is also 600 gold for even math, no clue why Hopoo chose 700 - doesn't divide easily by 40. Anyway, yeah, lemme know how this feels.*)
	* Gigantic Amethyst: Fixed networking
	* Guttural Whimpers: Updated icon
	* Life Savings: (*Developer note: This item is currently not networked, and I want to get this update out sooner rather than later - I recommend disabling this item for multiplayer games*)
	* Life Savings: Increased gold kept between stages from 2% to 4%
	* Mysterious Vial: Added level-based scaling to the regeneration e.g. Cautious Slug
	* Mysterious Vial: Increased from 0.4 regen to 0.8 regen (*Developer note: This change, as well as Life Savings' balance change, won't take effect unless you either delete your old config or edit it yourself to reflect these changes. This is just the nature of how the config file currently works*)
* Elites
	* Frenzying: Added (*Developer note: As a player, you have to press a key to activate the dash. By default, this is 'F', as to not conflict with Aspect Abilities*)
	* Leeching: Leeching Elites now use a custom buff for their AoE that will always heal 10% and never more than 10%, and also doesn't stack. Hopefully they should feel a bit more tame, now
	* Leeching: Removed behaviors from 0.1.2 where boss monsters would take twice as long to regen, as well as longer cooldowns based on enemies affected

### 0.1.3
* General
	* The mod works this time I swear

### 0.1.2
* General
	* Fixed issue preventing the game from launching depending on the user's system localization
	* Disabling Leeching Elites no longer prevents the mod from loading
* Items
	* Life Savings: Updated render
	* Smart Shopper: Updated materials & render
* Elites
	* Leeching: Regen buff no longer stacks with itself
	* Leeching: The more monsters affected by a Leeching Elite's AoE, the longer the duration until the next AoE burst will be
	* Leeching: Boss monsters now take twice as long to fully regen off of a Leeching Elite's healing AoE

### 0.1.1
* General
	* Updated manifest
	* Mod should actually work now... Oops

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
  (*Developer note: Another changed item. The original Life Savings worked fine in Risk of Rain 2, but felt extremely boring to play with. Instead of passive gold, the item now allows you to keep a small amount of your gold between stages. Hoping this makes the item much more desirable.*)
* Elites
	* Leeching: Reduced healing AoE from 256m to 20m. Oops

### 0.0.3
* Elites
	* Turns out Leeching Elites weren't actually doing anything before - given an itemBehavior so they start doing stuff, now
	* Buffed the on-hit healing of Leeching Elites significantly (removed x0.25 multiplier)
	* Leeching Elites Now create a tracer to nearby monsters when healing them

### 0.0.2
* General
	* Added FixPluginTypesSerialzation as a dependency
	* Fixed issue where mod wouldn't load without Aspect Abilities installed

### 0.0.1
*Should probably start keeping track of version #'s - this is the first ThunderKit release, so let's start here.*
* Items
	* Beckoning Cat: Added
	 (*Developer note: This item was originally the "56 Leaf Clover" in Risk of Rain 1, but was changed due to the inclusion of "57 Leaf Clover" in Risk of Rain 2. To help prevent confusion betwen the two, the item was given a complete overhaul - may provide a config option to restore this into the 56 Leaf Clover if there's demand for it.*)
	* Mysterious Vial: Added
	* Telescopic Sight: Added
 	(*Developer note: The original Telescopic Sight is widely considered an overpowered and unfun item. The current implementation instead offers players a 10% chance to quintuple their damage on crit. That said, I'm not happy with this implementation and am looking into alternatives that both better preserve the item's identity, and are more interesting to play with.*)
	* Thallium: Added
* Elites
	* Added Leeching Elites
