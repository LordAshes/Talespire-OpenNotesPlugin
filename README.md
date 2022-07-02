## Open Notes Plugin
The Open Notes Plugin opens mini specific notes for a character in the application of your choice.

## Changelog

```
1.1.0: Added error condition messages (application missing, content missing)
1.0.0: Initial Release
```

## Installing With R2ModMan

This package is designed specifically for R2ModMan and Talespire. 
You can install them via clicking on "Install with Mod Manager" or using the r2modman directly.
Use the Edit Config option in R2ModMan for the plugin to configure the desired application (default Notepad),
application path (default plugin CustomData sub-folder), and file type (default txt).

## Player Usage

When a mini is selected press the hotkey (default RCTRL+N).

If a mini is not selected, a display message will appear indicating that a mini needs to be selected.
If the configured application cannot be found, a display message indicates this (as well as a log message).
If the mini specific content cannot be found, a display message indicates this (as well as a log message).

## Advanced Usage

While intended to be used to launch a editor, reader or viewer this plugin just launched the specified
application with the specified path, mini name and file type as parameters. As a result this plugin can be used
to start any kind of application with potentially mini specific parameters.
