using System;

using ModLogger = UnityModManagerNet.UnityModManager.ModEntry.ModLogger;

namespace MySummerTrain;

public static class Logging
{
	public static ModLogger Logger => Main.mod.Logger;

	public static void Log(string str) => Logger.Log(str);

	public static void Error(string str) => Logger.Log(str);

	public static void Critical(string str) => Logger.Log(str);

	public static void Warning(string str) => Logger.Log(str);

	public static void NativeLog(string str) => Logger.NativeLog(str);

	public static void LogException(string key, Exception e) => Logger.LogException(key, e);

	public static void LogException(Exception e) => Logger.LogException(e);
}
