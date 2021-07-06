# Sample-Filepicker-Multi-Workaround

## Description

This sample is a work around for the WindowsAppSDK / WinUI 3.0 bug documented here: https://github.com/microsoft/WindowsAppSDK/issues/467. This is where FileOpenPicker.PickMultipleFilesAsync crashes when selecting multiple files.

This uses using ```System.Runtime.InteropServices``` to call Win32 APIs to accomplish mutliple file selection. See ```LibWrap.cs```
