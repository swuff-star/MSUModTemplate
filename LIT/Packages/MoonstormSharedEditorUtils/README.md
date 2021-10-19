# Moonstorm Shared Editor Utils

Moonstorm shared editor utils (Abreviated as MSEU) is a package of classes specifically designed for the unity editor to aid in the creation of Risk of Rain 2 Mods.

**YOU NEED MOONSTORM SHARED UTILS IN YOUR PROJECT OR ELSE THE EDITOR WONT WORK PROPERLY** [Link](https://thunderstore.io/package/TeamMoonstorm/MoonstormSharedUtils/)

## Features:

* Comes prepackaged with stubbed shaders.
* Comes with a custom tree drawer for serializable entity state types.

![](https://media.discordapp.net/attachments/757459787117101096/890766627832987658/unknown.png)

* Custom Editor utilities for managing material's shaders. Including KingEnderBrine's Shader Asset Picker.

![](https://i.gyazo.com/fbcc764992e87e2f9cf08f68ecc86f69.png)

* Pick Shader Asset: Allows you to properly select the real hopoo shader.
* Upgrade to real shader: In case the custom pipeline fails to swap the stubbed shaders to real shaders, you can click this button and it'll swap automatically to the real shaders.

* A custom pipeline based off "StageAssetBundles". which will:
    * Swap all the materials that are using the real hopoo shaders to their stubbed versions.
    * Build the Assetbundle.
    * Swap all the materials that are using the stubbed shaders to their real hopoo shaders versions.
* The end result is that materials will always use the real shaders in the editor, but the packaged assetbundle contains no references to the real shaders, avoiding possible copyright issues that entail redistributing Risk of Rain 2 assets.

![](https://i.gyazo.com/27fd721d3a2a0595a8f32c284a550015.png)

* Creation of UnlockableDefs via the CreateAsset Menu.
* A "Pseudo-Extension" of the assetdatabase class, allowing you to search for all assets of a specific type.

* Editor Scripts for custom Editor Windows for the following RoR2 Types:
    * EntityStateConfigurations
    * SerializableContentPack

* Editor Scripts for custom Editor Windows for the following MSU Types:
    * Key Asset Display Pair Holder
    * MSIDRS
    * SingleItemDisplayRuleSet
    * Vanilla Skin Def

## Troubleshooting

* Any kind of bug or issue when using these editor utilities should be informed in the Starstorm2 Discord server, specifically towards "Nebby", the developer that takes care of this project.

# Changelog

'0.1.0'
* Added MSU as a Needed Dependency for the mod to work properly.
* Added Editor Scripts for custom editor windows for the following types:
    * EntityStateConfigurations
    * Key Asset Display Pair Holder
    * MSIDRS
    * SerializableContentPack
    * SingleItemDisplayRuleSet
    * Vanilla Skin Def
* SwapShadersAndStageAssetBundles now fix any stubbed shaders that are found
* Added CalmWater stubbed shaders.

'0.0.1'
* Real initial release

'0.0.0'
* Test release

