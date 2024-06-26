﻿namespace AssetStudio {
	public enum LoggerEvent {
		Verbose,
		Debug,
		Info,
		Warning,
		Error,
	}

	public interface ILogger {
		void Log(LoggerEvent loggerEvent, string message);
	}

	public sealed class DummyLogger : ILogger {
		public void Log(LoggerEvent loggerEvent, string message) { }
	}
}
