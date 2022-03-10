# Moonstorm Shared Utils

- Moonstorm Shared Utils (Abreviated as MSU) is a library mod with the intention to help ease the creation of mods, mainly ones that are developed trough Thunderkit. It is a library mod used by the members of Team Moonstorm.

- Contains both classes for mod making and an assembly for Editor utilities.

- Classes for mod making include a modular system using abstract classes, alongside abstract content classes. Taking heavy use off the ItemBehavior class found inside the CharacterBody.

## Features

- Module Bases
    - A Module base is a type of class inside MSU that handles a specific type of content found inside RoR2. Theyre abstract by nature so you need to inherit from them to properly work.
    - Key aspects of module bases include:
        - Easily add items to your contentpacks.
        - Handles the automatic use of ItemBehaviors.
    - Module bases included are:
        - Artifact Module
        - Buff Module
        - Character Module
        - Damage Type Module (R2API Dependant)
        - Elite Module
        - Item Display Module
        - Items and Equipments Module
        - Projectile Module

- Content Bases
    - A Content base is what MSU uses to identify the type of content youre trying to add to the game. Theyre abstract by nature so you need to inherit from them to properly work.
    - Key Aspects of Content bases include:
        - Initialization methods to finish anything on runtime, such as replacing materials with ingame ones, or getting any kind of asset that needs to be finished in runtime.
        - Abstract fields for ContentDefinitions (ItemDef, ArtifactDef, EquipmentDef, ETC).
    - Content Bases Included are:
        - Artifact
        - Buff
        - Character
        - Damage Type
        - Equipment (& Elite Equipment)
        - Item
        - Monster
        - Projectile
        - Survivor

- Scriptable Object
    - MSU comes packaged with scriptable objects to either help the aid of content creation, or for the help of working alongside RoR2's Systems.
    - Scriptable Object Included are:
        - Vanilla Skin Def - Create a Skin for a vanilla character.
        - Serializable Difficulty Def - Creates a Difficulty Def
        - Monster Director Card & Monster Director Card Holder - Used for creation of monsters.
        - EffectDef Holder - Used for creating EffectDefs in the editor and adding them to the content pack.
        - String ItemDisplayRuleSet - Used for creating item display rules in the editor. all based off a system of strings and dictionaries. Creating a new display is as simple as writing the name of the key asset, and the display prefab. and you can even use the values from KEB's Item Display Placement Helper. as simple as copying them and pasting them in it's field.
        - Single Item Display Rule - Similar to R2API's ability to create item displays for each character in an item. Create item display rules for any vanilla IDRS using strings.
        - Key Asset Display Prefab Holder - Holds the Key Asset and their respective Display Prefab. used in the IDRS module.
        - MSEliteDef - An improved version of the vanilla Elite Def. allows you to easily create either a tier 1 or tier 2 elite in the editor. including the ability to pre-set the Elite's Ramp, Overlay, Particles or Effects.
        - MSAspectAbilityDataHolder - Allows you to store data related to AspectAbilities. specifically the equipment def, the max distance and the anim curve needed.

- Components:
    - Item Manager - Takes care of managing the items made with MSU. automatically handling their ItemBehaviors.
    - Moonstorm Item Display Helper - Used for the ItemDisplays module.
    - Comes pre-packaged with KomradeSpectre's HGController finder. modify in real time a material that uses hopoo's shaders with an inspector.

- Utilities, Interfaces and Attributes:
    - Utilities:
        - MSUtil - Contains for now a single method for checking wether or not a mod is installed.
        - MSDebug - A MonoBehavior that gets attached to the base unity plugin. enabling this debug component causes changes that should facilitate the creation of mods.
            - Changes include:
                - Connect to yourself with a second instance of RoR2.
                - Muting commando by removing his skills (In case you need to do a lot of searching in a runtime inspector.)
                - Automatic deployment of the no_enemies command from debug toolkit.
                - Automatic addition of the Moonstorm Item Display Helper component to all the characterbodies found in the catalog.
                - Spawning the Material Tester, which comes pre-packaged with the HGControllerFinder.
    - Interfaces:
        - IStatItemBehavior - An interface that works alongside the ItemBehaviors. this Interface allows you to interact with RecalculateStats. Albeit only for the Begining portion and the end portion. if you want to interact in a more deeply level you should use recalcstatsAPI.
        - IOnIncomingDamageOtherServerReceiver - An interface used to modify the incoming damage of soon to be Victims in the damage report, it is similar to the IOnIncomingDamage interface.
    - Attributes:
        - DisabledContent - Put this attribute on top of a content base inheriting class, and MSU will not load it nor add it to the content pack.

## Documentation & Sourcecode

* The Documentation and Sourcecode can be found in MoonstormSharedUtil's Github Repository, which can be found [here](https://github.com/TeamMoonstorm/MoonstormSharedUtils)

Some things to note...

> The Repository might have some disparaties between the current release of MSU and the repo itself, this is because it's not the real repository where the development happens

> we'll try to keep this repository up to date at any cost.

- MoonstormSharedUtils now has a [Wiki](https://github.com/TeamMoonstorm/MoonstormSharedUtils/wiki), filled with introductions and explanations on how the systems work.

- For now, you can find an example on how to use the mod in Lost In Transit's Github repository, which can be found [here](https://github.com/swuff-star/LostInTransit/tree/master/LIT/Assets/LostInTransit)

## Changelog

### '0.3.0'

* Rewrote a lot of the Elite related code to allow for more pleasant and balanced behaviors.
    * Fixed Elite Ramps not Reseting properly when grabbing an MSEliteEquipment and then grabbing a vanilla one.
    * Added Tooltips for the MSEliteDef
* Added an Opaque Cloud Remap runtime editor. (OCR are deprecated, ghor recommends using regular cloud remaps instead).
* Projectiles with CharacterBody components now get added to the Serializable Content Pack's CharacterBody prefabs.
* Added IOnIncomingDamageOtherServerReceiver
    * Used to modify the incoming damage of soon to be Victims in the damage report, it is similar to the IOnIncomingDamage interface.
* Almost all classes are now documented, However, MSU does not come with XML documentation yet.
* Made MoonstormEditorUtils Dependant on MoonstormSharedUtils.
* Changes to the IDRS System.
    * The ItemDisplayPrefab scriptable object has been revamped to a key asset display pair holder.
    * The KeyAssetDisplayPairHolder can be given to the IDRS module and it'll populate both the key asset dictionary and the display prefab dictionary.

### '0.2.1'

* Re-Added the MSAspectAbility, now named "MSAspectAbilityDataHolder"
    * Only holds data, doesnt use aspect abilities at all.
    * Used for elite equipments.
* Fixed an issue where the EliteModuleBase wouldnt register new elites.
* Fixed an issue where the ItemManager would not properly obtain new  StatItemBehavior interfaces on buff removal.
* Removed EditorUtils until further notice.
* General internal changes

### '0.2.0'

* Deprecated MSAspectAbility due to issues with initalization of the mod.
    * It has been replaced by an Animation curve asset and a simple float for AIMaxUseDistance
* Fixed an issue where MSIDRS/SIDRS would not be able to retrieve their display prefab correctly.
* Changed any references to definitions adding to content pack to use the content pack's name.
* Added more material controllers, courtesy of Vale-X
* Removed abstract field from module abses
* Module bases now use SystemInitializer attribute

### '0.0.1'

* Initial Release