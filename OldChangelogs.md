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
