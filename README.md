# Community.PowerToys.Run.Plugins<!-- omit in toc -->

> 🗂️🔎 Community PowerToys Run Plugins 🔌

- [Bang](#bang)
- [DenCode](#dencode)
- [Dice](#dice)
- [Need](#need)
- [Twitch](#twitch)

The plugins has been developed and tested with `PowerToys` `v0.78.0`.

## Bang

> Search websites with DuckDuckGo !Bangs

<https://duckduckgo.com/bangs>

<!-- TODO: gif -->

### Installation<!-- omit in toc -->

1. Download the `.zip` file from the latest [release](https://github.com/hlaueriksson/Community.PowerToys.Run.Plugins/releases/latest) and extract it to:
   - `%LocalAppData%\Microsoft\PowerToys\PowerToys Run\Plugins`
2. Restart PowerToys

### Usage<!-- omit in toc -->

1. Open PowerToys Run with `alt + space`
2. Type `bang`
   - A list of common website bangs will be displayed
3. Continue to type to find website bangs
4. Use ⬆️ and ⬇️ keys to select a website bang
5. Continue to type to query the selected website bang
   - A list of search suggestions will be displayed
6. Press `Enter` to open the website and perform the search

## DenCode

> Encoding & Decoding

<https://dencode.com>

<!-- TODO: gif -->

### Installation<!-- omit in toc -->

1. Download the `.zip` file from the latest [release](https://github.com/hlaueriksson/Community.PowerToys.Run.Plugins/releases/latest) and extract it to:
   - `%LocalAppData%\Microsoft\PowerToys\PowerToys Run\Plugins`
2. Restart PowerToys

### Usage<!-- omit in toc -->

1. Open PowerToys Run with `alt + space`
2. Type `dencode`
   - A list of all conversions will be displayed
3. Continue to type to find conversions
4. Use ⬆️ and ⬇️ keys to select a conversion
5. Continue to type to enter a value to encode / decode
   - A list of encodings / decodings will be displayed
6. Press `Enter` to copy the selected encoding / decoding to clipboard
7. Press `Ctrl + Enter` to open the selected encoding / decoding on the DenCode website

## Dice

> Roleplaying Dice Roller

<https://rolz.org>

<!-- TODO: gif -->

Dice Codes:

- <https://rolz.org/help/general>
- <https://rolz.org/wiki/inframe?w=help&n=basiccodes>
- <https://rolz.org/wiki/inframe?w=help&n=successcodes>

### Installation<!-- omit in toc -->

1. Download the `.zip` file from the latest [release](https://github.com/hlaueriksson/Community.PowerToys.Run.Plugins/releases/latest) and extract it to:
   - `%LocalAppData%\Microsoft\PowerToys\PowerToys Run\Plugins`
2. Restart PowerToys

### Usage<!-- omit in toc -->

1. Open PowerToys Run with `alt + space`
2. Type `roll`
   - A list of preconfigured roll expression will be displayed
3. Use ⬆️ and ⬇️ keys to select a roll expression
4. Press `Enter` to roll the expression
   - The result will be displayed
5. Continue to type to change the roll expression
   - The result will be displayed
6. Press `Enter` to copy the selected result to clipboard
7. Press `Ctrl + C` to copy the selected roll details to clipboard

### Configuration<!-- omit in toc -->

Add, update or delete preconfigured roll expressions via the plugin settings.

<!-- TODO: png -->

The settings are stored in the file:

- `DiceSettings.json`

Located in the folder:

- `%LocalAppData%\Microsoft\PowerToys\PowerToys Run\Settings\Plugins\Community.PowerToys.Run.Plugin.Dice\`

## Need

> Store things you need, but can't remember

<!-- TODO: gif -->

### Installation<!-- omit in toc -->

1. Download the `.zip` file from the latest [release](https://github.com/hlaueriksson/Community.PowerToys.Run.Plugins/releases/latest) and extract it to:
   - `%LocalAppData%\Microsoft\PowerToys\PowerToys Run\Plugins`
2. Restart PowerToys

### Usage<!-- omit in toc -->

1. Open PowerToys Run with `alt + space`
2. Type `need`
   - A list of saved records will be displayed
3. Continue to type to find saved records via key or value
4. Use ⬆️ and ⬇️ keys to select a record
5. Press `Enter` to copy the value of the selected record to clipboard
6. Press `Ctrl + C` to copy the selected record details to clipboard
7. Press `Ctrl + Del` to delete the selected record

Add record:

1. Open PowerToys Run with `alt + space`
2. Type `need`
3. Continue to type `<key> <value>`
   - Key without spaces
   - Separate with space ` `
   - Value with or without spaces
4. Press `Enter` to save the record

Example:

- `need mykey My value`

Update record:

1. Open PowerToys Run with `alt + space`
2. Type `need`
3. Continue to type `<key> <new value>`
   - Existing key without spaces
   - Separate with space ` `
   - New value with or without spaces
4. Press `Enter` to update the record

Example:

- `need mykey My new value`

### Configuration<!-- omit in toc -->

Change the file where records are stored via the plugin settings.

<!-- TODO: png -->

The records are, by default, stored in the file:

`NeedStorage.json`

Located in the folder:

- `%LocalAppData%\Microsoft\PowerToys\PowerToys Run\Settings\Plugins\Community.PowerToys.Run.Plugin.Need\`

## Twitch

> Browse, search and view streams on Twitch

<https://www.twitch.tv>

<!-- TODO: gif -->

### Requirements<!-- omit in toc -->

1. A [Twitch](https://www.twitch.tv) account
2. An application with `Client ID` and `Client Secret` from the [Twitch Developer Console](https://dev.twitch.tv/console)

### Installation<!-- omit in toc -->

1. Download the `.zip` file from the latest [release](https://github.com/hlaueriksson/Community.PowerToys.Run.Plugins/releases/latest) and extract it to:
   - `%LocalAppData%\Microsoft\PowerToys\PowerToys Run\Plugins`
2. Restart PowerToys
3. Update the plugin settings:
   - Copy the `Client ID` and `Client Secret` from the application in the [Twitch Developer Console](https://dev.twitch.tv/console)
   - Paste the values to the `Twitch API Client ID` and `Twitch API Client Secret` settings

### Usage<!-- omit in toc -->

1. Open PowerToys Run with `alt + space`
2. Type `twitch`
   - A list of commands are displayed:
     - Top games
     - Search channels
     - Search categories
3. Continue to type to prepare for:
   - Search channels
   - Search categories
4. Use ⬆️ and ⬇️ keys to select a command
5. Press `Enter` to run the command

Find streams:

1. Open PowerToys Run with `alt + space`
2. Type `twitch` and select one of:
   - Top games
   - Search categories
3. Top games
   - Use ⬆️ and ⬇️ keys to select a game
4. Search categories
   - Continue to type to find categories or games
5. Press `Enter` to expand the selected category or game
   - A list of streams will be displayed
6. Use ⬆️ and ⬇️ keys to select a stream
7. Press `Enter` to open the stream on the Twitch website

Find channels:

1. Open PowerToys Run with `alt + space`
2. Type `twitch` and select:
   - Search channels
3. Search channels
   - Continue to type to find channels
4. Use ⬆️ and ⬇️ keys to select a channel
5. Press `Enter` to open the channel on the Twitch website

### Configuration<!-- omit in toc -->

Set credentials and parameters for the Twitch API via the plugin settings.

<!-- TODO: png -->

The settings are stored in the file:

- `TwitchSettings.json`

Located in the folder:

- `%LocalAppData%\Microsoft\PowerToys\PowerToys Run\Settings\Plugins\Community.PowerToys.Run.Plugin.Twitch\`
