# OrderACharmMod_cloverpit

A BepInEx mod for **CloverPit** that allows you to order charms without spending tickets.

Based on the original [Order a Charm](https://www.nexusmods.com/cloverpit/mods/4) mod.

## 📦 Installation

### Requirements

* [BepInEx](https://github.com/BepInEx/BepInEx)
* CloverPit

### Installation Steps

1. Download [`OrderCharmMod.dll`](https://github.com/toronjilSupremo/OrderACharmMod_cloverpit/releases/tag/1.0.0).
2. Navigate to your CloverPit installation folder:

`C:\Program Files (x86)\Steam\steamapps\common\CloverPit\BepInEx\plugins`

3. Copy `OrderCharmMod.dll` into the `plugins` folder.
4. Launch **CloverPit** through Steam.
5. BepInEx will automatically load the mod.

## 🎮 How It Works

Once the mod is installed, you can order charms through the game's charm ordering system.

You can have up to **4 pending orders** at the same time.

**Example:**

`Orders: 0/4` → You can place an order.

`Orders: 1/4` → You can place another order.

`Orders: 2/4` → You can place another order.

`Orders: 3/4` → You can place one final order.

`Orders: 4/4` → The order button is disabled until the next restock.

After the restock:

`Orders: 4/4` → The queued charms are added to the shop.

`Orders: 0/4` → The counter resets and you can place new orders.

## 🔧 Configuration

Currently, the mod does not require any additional configuration.

The maximum number of pending orders is **4**.

## 📝 Changelog

### Current Version

* Added support for ordering any charm.
* Added support for special charms such as **God's Eye**.
* Removed ticket consumption.
* Removed the requirement to have enough tickets.
* Added a maximum queue of 4 orders.
* Orders are processed automatically after restock.
* Order counter resets after restock.

## ⚠️ Notes

This mod is designed for **BepInEx**.

Make sure BepInEx is correctly installed before installing the mod.

If the mod does not appear to work, check the BepInEx log files for errors.

## 📜 Credits

Based on the original **Order a Charm** mod:

https://www.nexusmods.com/cloverpit/mods/4

Coded for **BepInEx**.

## 📄 License

See the repository for licensing information.
