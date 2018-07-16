using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace ListenMoeClient
{
	/// <summary>Enumeration containing flags for a raw input device.</summary>
	[Flags()]
	public enum RawInputDeviceFlags
	{
		/// <summary>No flags.</summary>
		None = 0,
		/// <summary>If set, this removes the top level collection from the inclusion list. This tells the operating system to stop reading from a device which matches the top level collection.</summary>
		Remove = 0x00000001,
		/// <summary>If set, this specifies the top level collections to exclude when reading a complete usage page. This flag only affects a TLC whose usage page is already specified with PageOnly.</summary>
		Exclude = 0x00000010,
		/// <summary>If set, this specifies all devices whose top level collection is from the specified usUsagePage. Note that Usage must be zero. To exclude a particular top level collection, use Exclude.</summary>
		PageOnly = 0x00000020,
		/// <summary>If set, this prevents any devices specified by UsagePage or Usage from generating legacy messages. This is only for the mouse and keyboard.</summary>
		NoLegacy = 0x00000030,
		/// <summary>If set, this enables the caller to receive the input even when the caller is not in the foreground. Note that WindowHandle must be specified.</summary>
		InputSink = 0x00000100,
		/// <summary>If set, the mouse button click does not activate the other window.</summary>
		CaptureMouse = 0x00000200,
		/// <summary>If set, the application-defined keyboard device hotkeys are not handled. However, the system hotkeys; for example, ALT+TAB and CTRL+ALT+DEL, are still handled. By default, all keyboard hotkeys are handled. NoHotKeys can be specified even if NoLegacy is not specified and WindowHandle is NULL.</summary>
		NoHotKeys = 0x00000200,
		/// <summary>If set, application keys are handled.  NoLegacy must be specified.  Keyboard only.</summary>
		AppKeys = 0x00000400
	}

	/// <summary>Enumeration containing the HID usage values.</summary>
	public enum HIDUsage : ushort
	{
		/// <summary></summary>
		Pointer = 0x01,
		/// <summary></summary>
		Mouse = 0x02,
		/// <summary></summary>
		Joystick = 0x04,
		/// <summary></summary>
		Gamepad = 0x05,
		/// <summary></summary>
		Keyboard = 0x06,
		/// <summary></summary>
		Keypad = 0x07,
		/// <summary></summary>
		SystemControl = 0x80,
		/// <summary></summary>
		X = 0x30,
		/// <summary></summary>
		Y = 0x31,
		/// <summary></summary>
		Z = 0x32,
		/// <summary></summary>
		RelativeX = 0x33,
		/// <summary></summary>
		RelativeY = 0x34,
		/// <summary></summary>
		RelativeZ = 0x35,
		/// <summary></summary>
		Slider = 0x36,
		/// <summary></summary>
		Dial = 0x37,
		/// <summary></summary>
		Wheel = 0x38,
		/// <summary></summary>
		HatSwitch = 0x39,
		/// <summary></summary>
		CountedBuffer = 0x3A,
		/// <summary></summary>
		ByteCount = 0x3B,
		/// <summary></summary>
		MotionWakeup = 0x3C,
		/// <summary></summary>
		VX = 0x40,
		/// <summary></summary>
		VY = 0x41,
		/// <summary></summary>
		VZ = 0x42,
		/// <summary></summary>
		VBRX = 0x43,
		/// <summary></summary>
		VBRY = 0x44,
		/// <summary></summary>
		VBRZ = 0x45,
		/// <summary></summary>
		VNO = 0x46,
		/// <summary></summary>
		SystemControlPower = 0x81,
		/// <summary></summary>
		SystemControlSleep = 0x82,
		/// <summary></summary>
		SystemControlWake = 0x83,
		/// <summary></summary>
		SystemControlContextMenu = 0x84,
		/// <summary></summary>
		SystemControlMainMenu = 0x85,
		/// <summary></summary>
		SystemControlApplicationMenu = 0x86,
		/// <summary></summary>
		SystemControlHelpMenu = 0x87,
		/// <summary></summary>
		SystemControlMenuExit = 0x88,
		/// <summary></summary>
		SystemControlMenuSelect = 0x89,
		/// <summary></summary>
		SystemControlMenuRight = 0x8A,
		/// <summary></summary>
		SystemControlMenuLeft = 0x8B,
		/// <summary></summary>
		SystemControlMenuUp = 0x8C,
		/// <summary></summary>
		SystemControlMenuDown = 0x8D,
		/// <summary></summary>
		KeyboardNoEvent = 0x00,
		/// <summary></summary>
		KeyboardRollover = 0x01,
		/// <summary></summary>
		KeyboardPostFail = 0x02,
		/// <summary></summary>
		KeyboardUndefined = 0x03,
		/// <summary></summary>
		KeyboardaA = 0x04,
		/// <summary></summary>
		KeyboardzZ = 0x1D,
		/// <summary></summary>
		Keyboard1 = 0x1E,
		/// <summary></summary>
		Keyboard0 = 0x27,
		/// <summary></summary>
		KeyboardLeftControl = 0xE0,
		/// <summary></summary>
		KeyboardLeftShift = 0xE1,
		/// <summary></summary>
		KeyboardLeftALT = 0xE2,
		/// <summary></summary>
		KeyboardLeftGUI = 0xE3,
		/// <summary></summary>
		KeyboardRightControl = 0xE4,
		/// <summary></summary>
		KeyboardRightShift = 0xE5,
		/// <summary></summary>
		KeyboardRightALT = 0xE6,
		/// <summary></summary>
		KeyboardRightGUI = 0xE7,
		/// <summary></summary>
		KeyboardScrollLock = 0x47,
		/// <summary></summary>
		KeyboardNumLock = 0x53,
		/// <summary></summary>
		KeyboardCapsLock = 0x39,
		/// <summary></summary>
		KeyboardF1 = 0x3A,
		/// <summary></summary>
		KeyboardF12 = 0x45,
		/// <summary></summary>
		KeyboardReturn = 0x28,
		/// <summary></summary>
		KeyboardEscape = 0x29,
		/// <summary></summary>
		KeyboardDelete = 0x2A,
		/// <summary></summary>
		KeyboardPrintScreen = 0x46,
		/// <summary></summary>
		LEDNumLock = 0x01,
		/// <summary></summary>
		LEDCapsLock = 0x02,
		/// <summary></summary>
		LEDScrollLock = 0x03,
		/// <summary></summary>
		LEDCompose = 0x04,
		/// <summary></summary>
		LEDKana = 0x05,
		/// <summary></summary>
		LEDPower = 0x06,
		/// <summary></summary>
		LEDShift = 0x07,
		/// <summary></summary>
		LEDDoNotDisturb = 0x08,
		/// <summary></summary>
		LEDMute = 0x09,
		/// <summary></summary>
		LEDToneEnable = 0x0A,
		/// <summary></summary>
		LEDHighCutFilter = 0x0B,
		/// <summary></summary>
		LEDLowCutFilter = 0x0C,
		/// <summary></summary>
		LEDEqualizerEnable = 0x0D,
		/// <summary></summary>
		LEDSoundFieldOn = 0x0E,
		/// <summary></summary>
		LEDSurroundFieldOn = 0x0F,
		/// <summary></summary>
		LEDRepeat = 0x10,
		/// <summary></summary>
		LEDStereo = 0x11,
		/// <summary></summary>
		LEDSamplingRateDirect = 0x12,
		/// <summary></summary>
		LEDSpinning = 0x13,
		/// <summary></summary>
		LEDCAV = 0x14,
		/// <summary></summary>
		LEDCLV = 0x15,
		/// <summary></summary>
		LEDRecordingFormatDet = 0x16,
		/// <summary></summary>
		LEDOffHook = 0x17,
		/// <summary></summary>
		LEDRing = 0x18,
		/// <summary></summary>
		LEDMessageWaiting = 0x19,
		/// <summary></summary>
		LEDDataMode = 0x1A,
		/// <summary></summary>
		LEDBatteryOperation = 0x1B,
		/// <summary></summary>
		LEDBatteryOK = 0x1C,
		/// <summary></summary>
		LEDBatteryLow = 0x1D,
		/// <summary></summary>
		LEDSpeaker = 0x1E,
		/// <summary></summary>
		LEDHeadset = 0x1F,
		/// <summary></summary>
		LEDHold = 0x20,
		/// <summary></summary>
		LEDMicrophone = 0x21,
		/// <summary></summary>
		LEDCoverage = 0x22,
		/// <summary></summary>
		LEDNightMode = 0x23,
		/// <summary></summary>
		LEDSendCalls = 0x24,
		/// <summary></summary>
		LEDCallPickup = 0x25,
		/// <summary></summary>
		LEDConference = 0x26,
		/// <summary></summary>
		LEDStandBy = 0x27,
		/// <summary></summary>
		LEDCameraOn = 0x28,
		/// <summary></summary>
		LEDCameraOff = 0x29,
		/// <summary></summary>
		LEDOnLine = 0x2A,
		/// <summary></summary>
		LEDOffLine = 0x2B,
		/// <summary></summary>
		LEDBusy = 0x2C,
		/// <summary></summary>
		LEDReady = 0x2D,
		/// <summary></summary>
		LEDPaperOut = 0x2E,
		/// <summary></summary>
		LEDPaperJam = 0x2F,
		/// <summary></summary>
		LEDRemote = 0x30,
		/// <summary></summary>
		LEDForward = 0x31,
		/// <summary></summary>
		LEDReverse = 0x32,
		/// <summary></summary>
		LEDStop = 0x33,
		/// <summary></summary>
		LEDRewind = 0x34,
		/// <summary></summary>
		LEDFastForward = 0x35,
		/// <summary></summary>
		LEDPlay = 0x36,
		/// <summary></summary>
		LEDPause = 0x37,
		/// <summary></summary>
		LEDRecord = 0x38,
		/// <summary></summary>
		LEDError = 0x39,
		/// <summary></summary>
		LEDSelectedIndicator = 0x3A,
		/// <summary></summary>
		LEDInUseIndicator = 0x3B,
		/// <summary></summary>
		LEDMultiModeIndicator = 0x3C,
		/// <summary></summary>
		LEDIndicatorOn = 0x3D,
		/// <summary></summary>
		LEDIndicatorFlash = 0x3E,
		/// <summary></summary>
		LEDIndicatorSlowBlink = 0x3F,
		/// <summary></summary>
		LEDIndicatorFastBlink = 0x40,
		/// <summary></summary>
		LEDIndicatorOff = 0x41,
		/// <summary></summary>
		LEDFlashOnTime = 0x42,
		/// <summary></summary>
		LEDSlowBlinkOnTime = 0x43,
		/// <summary></summary>
		LEDSlowBlinkOffTime = 0x44,
		/// <summary></summary>
		LEDFastBlinkOnTime = 0x45,
		/// <summary></summary>
		LEDFastBlinkOffTime = 0x46,
		/// <summary></summary>
		LEDIndicatorColor = 0x47,
		/// <summary></summary>
		LEDRed = 0x48,
		/// <summary></summary>
		LEDGreen = 0x49,
		/// <summary></summary>
		LEDAmber = 0x4A,
		/// <summary></summary>
		LEDGenericIndicator = 0x3B,
		/// <summary></summary>
		TelephonyPhone = 0x01,
		/// <summary></summary>
		TelephonyAnsweringMachine = 0x02,
		/// <summary></summary>
		TelephonyMessageControls = 0x03,
		/// <summary></summary>
		TelephonyHandset = 0x04,
		/// <summary></summary>
		TelephonyHeadset = 0x05,
		/// <summary></summary>
		TelephonyKeypad = 0x06,
		/// <summary></summary>
		TelephonyProgrammableButton = 0x07,
		/// <summary></summary>
		SimulationRudder = 0xBA,
		/// <summary></summary>
		SimulationThrottle = 0xBB
	}

	/// <summary>
	/// Enumeration containing HID usage page flags.
	/// </summary>
	public enum HIDUsagePage : ushort
	{
		/// <summary>Unknown usage page.</summary>
		Undefined = 0x00,
		/// <summary>Generic desktop controls.</summary>
		Generic = 0x01,
		/// <summary>Simulation controls.</summary>
		Simulation = 0x02,
		/// <summary>Virtual reality controls.</summary>
		VR = 0x03,
		/// <summary>Sports controls.</summary>
		Sport = 0x04,
		/// <summary>Games controls.</summary>
		Game = 0x05,
		/// <summary>Keyboard controls.</summary>
		Keyboard = 0x07,
		/// <summary>LED controls.</summary>
		LED = 0x08,
		/// <summary>Button.</summary>
		Button = 0x09,
		/// <summary>Ordinal.</summary>
		Ordinal = 0x0A,
		/// <summary>Telephony.</summary>
		Telephony = 0x0B,
		/// <summary>Consumer.</summary>
		Consumer = 0x0C,
		/// <summary>Digitizer.</summary>
		Digitizer = 0x0D,
		/// <summary>Physical interface device.</summary>
		PID = 0x0F,
		/// <summary>Unicode.</summary>
		Unicode = 0x10,
		/// <summary>Alphanumeric display.</summary>
		AlphaNumeric = 0x14,
		/// <summary>Medical instruments.</summary>
		Medical = 0x40,
		/// <summary>Monitor page 0.</summary>
		MonitorPage0 = 0x80,
		/// <summary>Monitor page 1.</summary>
		MonitorPage1 = 0x81,
		/// <summary>Monitor page 2.</summary>
		MonitorPage2 = 0x82,
		/// <summary>Monitor page 3.</summary>
		MonitorPage3 = 0x83,
		/// <summary>Power page 0.</summary>
		PowerPage0 = 0x84,
		/// <summary>Power page 1.</summary>
		PowerPage1 = 0x85,
		/// <summary>Power page 2.</summary>
		PowerPage2 = 0x86,
		/// <summary>Power page 3.</summary>
		PowerPage3 = 0x87,
		/// <summary>Bar code scanner.</summary>
		BarCode = 0x8C,
		/// <summary>Scale page.</summary>
		Scale = 0x8D,
		/// <summary>Magnetic strip reading devices.</summary>
		MSR = 0x8E
	}

	/// <summary>Value type for raw input devices.</summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct RAWINPUTDEVICE
	{
		/// <summary>Top level collection Usage page for the raw input device.</summary>
		public HIDUsagePage UsagePage;
		/// <summary>Top level collection Usage for the raw input device. </summary>
		public HIDUsage Usage;
		/// <summary>Mode flag that specifies how to interpret the information provided by UsagePage and Usage.</summary>
		public RawInputDeviceFlags Flags;
		/// <summary>Handle to the target device. If NULL, it follows the keyboard focus.</summary>
		public IntPtr WindowHandle;
	}

	/// <summary>
	/// Enumeration contanining the command types to issue.
	/// </summary>
	public enum RawInputCommand
	{
		/// <summary>
		/// Get input data.
		/// </summary>
		Input = 0x10000003,
		/// <summary>
		/// Get header data.
		/// </summary>
		Header = 0x10000005
	}

	/// <summary>
	/// Enumeration containing the type device the raw input is coming from.
	/// </summary>
	public enum RawInputType
	{
		/// <summary>
		/// Mouse input.
		/// </summary>
		Mouse = 0,

		/// <summary>
		/// Keyboard input.
		/// </summary>
		Keyboard = 1,

		/// <summary>
		///  Human interface device input.
		/// </summary>
		HID = 2,

		/// <summary>
		/// Another device that is not the keyboard or the mouse.
		/// </summary>
		Other = 3
	}

	/// <summary>
	/// Enumeration containing the flags for raw mouse data.
	/// </summary>
	[Flags()]
	public enum RawMouseFlags
		: ushort
	{
		/// <summary>Relative to the last position.</summary>
		MoveRelative = 0,
		/// <summary>Absolute positioning.</summary>
		MoveAbsolute = 1,
		/// <summary>Coordinate data is mapped to a virtual desktop.</summary>
		VirtualDesktop = 2,
		/// <summary>Attributes for the mouse have changed.</summary>
		AttributesChanged = 4
	}

	/// <summary>
	/// Enumeration containing the button data for raw mouse input.
	/// </summary>
	[Flags()]
	public enum RawMouseButtons
		: ushort
	{
		/// <summary>No button.</summary>
		None = 0,
		/// <summary>Left (button 1) down.</summary>
		LeftDown = 0x0001,
		/// <summary>Left (button 1) up.</summary>
		LeftUp = 0x0002,
		/// <summary>Right (button 2) down.</summary>
		RightDown = 0x0004,
		/// <summary>Right (button 2) up.</summary>
		RightUp = 0x0008,
		/// <summary>Middle (button 3) down.</summary>
		MiddleDown = 0x0010,
		/// <summary>Middle (button 3) up.</summary>
		MiddleUp = 0x0020,
		/// <summary>Button 4 down.</summary>
		Button4Down = 0x0040,
		/// <summary>Button 4 up.</summary>
		Button4Up = 0x0080,
		/// <summary>Button 5 down.</summary>
		Button5Down = 0x0100,
		/// <summary>Button 5 up.</summary>
		Button5Up = 0x0200,
		/// <summary>Mouse wheel moved.</summary>
		MouseWheel = 0x0400
	}

	/// <summary>
	/// Contains information about the state of the mouse.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct RawMouse
	{
		/// <summary>
		/// The mouse state.
		/// </summary>
		public RawMouseFlags Flags;

		[StructLayout(LayoutKind.Explicit)]
		public struct DATA
		{
			[FieldOffset(0)]
			public uint Buttons;
			/// <summary>
			/// If the mouse wheel is moved, this will contain the delta amount.
			/// </summary>
			[FieldOffset(2)]
			public ushort ButtonData;
			/// <summary>
			/// Flags for the event.
			/// </summary>
			[FieldOffset(0)]
			public RawMouseButtons ButtonFlags;
		}

		public DATA Data;

		/// <summary>
		/// Raw button data.
		/// </summary>
		public uint RawButtons;
		/// <summary>
		/// The motion in the X direction. This is signed relative motion or
		/// absolute motion, depending on the value of usFlags.
		/// </summary>
		public int LastX;
		/// <summary>
		/// The motion in the Y direction. This is signed relative motion or absolute motion,
		/// depending on the value of usFlags.
		/// </summary>
		public int LastY;
		/// <summary>
		/// The device-specific additional information for the event.
		/// </summary>
		public uint ExtraInformation;
	}

	/// <summary>
	/// Value type for a raw input header.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct RAWINPUTHEADER
	{
		/// <summary>Type of device the input is coming from.</summary>
		public RawInputType Type;
		/// <summary>Size of the packet of data.</summary>
		public int Size;
		/// <summary>Handle to the device sending the data.</summary>
		public IntPtr Device;
		/// <summary>wParam from the window message.</summary>
		public IntPtr wParam;
	}

	/// <summary>
	/// Enumeration containing flags for raw keyboard input.
	/// </summary>
	[Flags]
	public enum RawKeyboardFlags : ushort
	{
		/// <summary></summary>
		KeyMake = 0,
		/// <summary></summary>
		KeyBreak = 1,
		/// <summary></summary>
		KeyE0 = 2,
		/// <summary></summary>
		KeyE1 = 4,
		/// <summary></summary>
		TerminalServerSetLED = 8,
		/// <summary></summary>
		TerminalServerShadow = 0x10,
		/// <summary></summary>
		TerminalServerVKPACKET = 0x20
	}

	/// <summary>
	/// Enumeration for virtual keys.
	/// </summary>
	public enum VirtualKeys
		: ushort
	{
		/// <summary></summary>
		LeftButton = 0x01,
		/// <summary></summary>
		RightButton = 0x02,
		/// <summary></summary>
		Cancel = 0x03,
		/// <summary></summary>
		MiddleButton = 0x04,
		/// <summary></summary>
		ExtraButton1 = 0x05,
		/// <summary></summary>
		ExtraButton2 = 0x06,
		/// <summary></summary>
		Back = 0x08,
		/// <summary></summary>
		Tab = 0x09,
		/// <summary></summary>
		Clear = 0x0C,
		/// <summary></summary>
		Return = 0x0D,
		/// <summary></summary>
		Shift = 0x10,
		/// <summary></summary>
		Control = 0x11,
		/// <summary></summary>
		Menu = 0x12,
		/// <summary></summary>
		Pause = 0x13,
		/// <summary></summary>
		CapsLock = 0x14,
		/// <summary></summary>
		Kana = 0x15,
		/// <summary></summary>
		Hangeul = 0x15,
		/// <summary></summary>
		Hangul = 0x15,
		/// <summary></summary>
		Junja = 0x17,
		/// <summary></summary>
		Final = 0x18,
		/// <summary></summary>
		Hanja = 0x19,
		/// <summary></summary>
		Kanji = 0x19,
		/// <summary></summary>
		Escape = 0x1B,
		/// <summary></summary>
		Convert = 0x1C,
		/// <summary></summary>
		NonConvert = 0x1D,
		/// <summary></summary>
		Accept = 0x1E,
		/// <summary></summary>
		ModeChange = 0x1F,
		/// <summary></summary>
		Space = 0x20,
		/// <summary></summary>
		Prior = 0x21,
		/// <summary></summary>
		Next = 0x22,
		/// <summary></summary>
		End = 0x23,
		/// <summary></summary>
		Home = 0x24,
		/// <summary></summary>
		Left = 0x25,
		/// <summary></summary>
		Up = 0x26,
		/// <summary></summary>
		Right = 0x27,
		/// <summary></summary>
		Down = 0x28,
		/// <summary></summary>
		Select = 0x29,
		/// <summary></summary>
		Print = 0x2A,
		/// <summary></summary>
		Execute = 0x2B,
		/// <summary></summary>
		Snapshot = 0x2C,
		/// <summary></summary>
		Insert = 0x2D,
		/// <summary></summary>
		Delete = 0x2E,
		/// <summary></summary>
		Help = 0x2F,
		/// <summary></summary>
		N0 = 0x30,
		/// <summary></summary>
		N1 = 0x31,
		/// <summary></summary>
		N2 = 0x32,
		/// <summary></summary>
		N3 = 0x33,
		/// <summary></summary>
		N4 = 0x34,
		/// <summary></summary>
		N5 = 0x35,
		/// <summary></summary>
		N6 = 0x36,
		/// <summary></summary>
		N7 = 0x37,
		/// <summary></summary>
		N8 = 0x38,
		/// <summary></summary>
		N9 = 0x39,
		/// <summary></summary>
		A = 0x41,
		/// <summary></summary>
		B = 0x42,
		/// <summary></summary>
		C = 0x43,
		/// <summary></summary>
		D = 0x44,
		/// <summary></summary>
		E = 0x45,
		/// <summary></summary>
		F = 0x46,
		/// <summary></summary>
		G = 0x47,
		/// <summary></summary>
		H = 0x48,
		/// <summary></summary>
		I = 0x49,
		/// <summary></summary>
		J = 0x4A,
		/// <summary></summary>
		K = 0x4B,
		/// <summary></summary>
		L = 0x4C,
		/// <summary></summary>
		M = 0x4D,
		/// <summary></summary>
		N = 0x4E,
		/// <summary></summary>
		O = 0x4F,
		/// <summary></summary>
		P = 0x50,
		/// <summary></summary>
		Q = 0x51,
		/// <summary></summary>
		R = 0x52,
		/// <summary></summary>
		S = 0x53,
		/// <summary></summary>
		T = 0x54,
		/// <summary></summary>
		U = 0x55,
		/// <summary></summary>
		V = 0x56,
		/// <summary></summary>
		W = 0x57,
		/// <summary></summary>
		X = 0x58,
		/// <summary></summary>
		Y = 0x59,
		/// <summary></summary>
		Z = 0x5A,
		/// <summary></summary>
		LeftWindows = 0x5B,
		/// <summary></summary>
		RightWindows = 0x5C,
		/// <summary></summary>
		Application = 0x5D,
		/// <summary></summary>
		Sleep = 0x5F,
		/// <summary></summary>
		Numpad0 = 0x60,
		/// <summary></summary>
		Numpad1 = 0x61,
		/// <summary></summary>
		Numpad2 = 0x62,
		/// <summary></summary>
		Numpad3 = 0x63,
		/// <summary></summary>
		Numpad4 = 0x64,
		/// <summary></summary>
		Numpad5 = 0x65,
		/// <summary></summary>
		Numpad6 = 0x66,
		/// <summary></summary>
		Numpad7 = 0x67,
		/// <summary></summary>
		Numpad8 = 0x68,
		/// <summary></summary>
		Numpad9 = 0x69,
		/// <summary></summary>
		Multiply = 0x6A,
		/// <summary></summary>
		Add = 0x6B,
		/// <summary></summary>
		Separator = 0x6C,
		/// <summary></summary>
		Subtract = 0x6D,
		/// <summary></summary>
		Decimal = 0x6E,
		/// <summary></summary>
		Divide = 0x6F,
		/// <summary></summary>
		F1 = 0x70,
		/// <summary></summary>
		F2 = 0x71,
		/// <summary></summary>
		F3 = 0x72,
		/// <summary></summary>
		F4 = 0x73,
		/// <summary></summary>
		F5 = 0x74,
		/// <summary></summary>
		F6 = 0x75,
		/// <summary></summary>
		F7 = 0x76,
		/// <summary></summary>
		F8 = 0x77,
		/// <summary></summary>
		F9 = 0x78,
		/// <summary></summary>
		F10 = 0x79,
		/// <summary></summary>
		F11 = 0x7A,
		/// <summary></summary>
		F12 = 0x7B,
		/// <summary></summary>
		F13 = 0x7C,
		/// <summary></summary>
		F14 = 0x7D,
		/// <summary></summary>
		F15 = 0x7E,
		/// <summary></summary>
		F16 = 0x7F,
		/// <summary></summary>
		F17 = 0x80,
		/// <summary></summary>
		F18 = 0x81,
		/// <summary></summary>
		F19 = 0x82,
		/// <summary></summary>
		F20 = 0x83,
		/// <summary></summary>
		F21 = 0x84,
		/// <summary></summary>
		F22 = 0x85,
		/// <summary></summary>
		F23 = 0x86,
		/// <summary></summary>
		F24 = 0x87,
		/// <summary></summary>
		NumLock = 0x90,
		/// <summary></summary>
		ScrollLock = 0x91,
		/// <summary></summary>
		NEC_Equal = 0x92,
		/// <summary></summary>
		Fujitsu_Jisho = 0x92,
		/// <summary></summary>
		Fujitsu_Masshou = 0x93,
		/// <summary></summary>
		Fujitsu_Touroku = 0x94,
		/// <summary></summary>
		Fujitsu_Loya = 0x95,
		/// <summary></summary>
		Fujitsu_Roya = 0x96,
		/// <summary></summary>
		LeftShift = 0xA0,
		/// <summary></summary>
		RightShift = 0xA1,
		/// <summary></summary>
		LeftControl = 0xA2,
		/// <summary></summary>
		RightControl = 0xA3,
		/// <summary></summary>
		LeftMenu = 0xA4,
		/// <summary></summary>
		RightMenu = 0xA5,
		/// <summary></summary>
		BrowserBack = 0xA6,
		/// <summary></summary>
		BrowserForward = 0xA7,
		/// <summary></summary>
		BrowserRefresh = 0xA8,
		/// <summary></summary>
		BrowserStop = 0xA9,
		/// <summary></summary>
		BrowserSearch = 0xAA,
		/// <summary></summary>
		BrowserFavorites = 0xAB,
		/// <summary></summary>
		BrowserHome = 0xAC,
		/// <summary></summary>
		VolumeMute = 0xAD,
		/// <summary></summary>
		VolumeDown = 0xAE,
		/// <summary></summary>
		VolumeUp = 0xAF,
		/// <summary></summary>
		MediaNextTrack = 0xB0,
		/// <summary></summary>
		MediaPrevTrack = 0xB1,
		/// <summary></summary>
		MediaStop = 0xB2,
		/// <summary></summary>
		MediaPlayPause = 0xB3,
		/// <summary></summary>
		LaunchMail = 0xB4,
		/// <summary></summary>
		LaunchMediaSelect = 0xB5,
		/// <summary></summary>
		LaunchApplication1 = 0xB6,
		/// <summary></summary>
		LaunchApplication2 = 0xB7,
		/// <summary></summary>
		OEM1 = 0xBA,
		/// <summary></summary>
		OEMPlus = 0xBB,
		/// <summary></summary>
		OEMComma = 0xBC,
		/// <summary></summary>
		OEMMinus = 0xBD,
		/// <summary></summary>
		OEMPeriod = 0xBE,
		/// <summary></summary>
		OEM2 = 0xBF,
		/// <summary></summary>
		OEM3 = 0xC0,
		/// <summary></summary>
		OEM4 = 0xDB,
		/// <summary></summary>
		OEM5 = 0xDC,
		/// <summary></summary>
		OEM6 = 0xDD,
		/// <summary></summary>
		OEM7 = 0xDE,
		/// <summary></summary>
		OEM8 = 0xDF,
		/// <summary></summary>
		OEMAX = 0xE1,
		/// <summary></summary>
		OEM102 = 0xE2,
		/// <summary></summary>
		ICOHelp = 0xE3,
		/// <summary></summary>
		ICO00 = 0xE4,
		/// <summary></summary>
		ProcessKey = 0xE5,
		/// <summary></summary>
		ICOClear = 0xE6,
		/// <summary></summary>
		Packet = 0xE7,
		/// <summary></summary>
		OEMReset = 0xE9,
		/// <summary></summary>
		OEMJump = 0xEA,
		/// <summary></summary>
		OEMPA1 = 0xEB,
		/// <summary></summary>
		OEMPA2 = 0xEC,
		/// <summary></summary>
		OEMPA3 = 0xED,
		/// <summary></summary>
		OEMWSCtrl = 0xEE,
		/// <summary></summary>
		OEMCUSel = 0xEF,
		/// <summary></summary>
		OEMATTN = 0xF0,
		/// <summary></summary>
		OEMFinish = 0xF1,
		/// <summary></summary>
		OEMCopy = 0xF2,
		/// <summary></summary>
		OEMAuto = 0xF3,
		/// <summary></summary>
		OEMENLW = 0xF4,
		/// <summary></summary>
		OEMBackTab = 0xF5,
		/// <summary></summary>
		ATTN = 0xF6,
		/// <summary></summary>
		CRSel = 0xF7,
		/// <summary></summary>
		EXSel = 0xF8,
		/// <summary></summary>
		EREOF = 0xF9,
		/// <summary></summary>
		Play = 0xFA,
		/// <summary></summary>
		Zoom = 0xFB,
		/// <summary></summary>
		Noname = 0xFC,
		/// <summary></summary>
		PA1 = 0xFD,
		/// <summary></summary>
		OEMClear = 0xFE
	}

	/// <summary>
	/// Windows Messages
	/// Defined in winuser.h from Windows SDK v6.1
	/// Documentation pulled from MSDN.
	/// </summary>
	public enum WM : uint
	{
		/// <summary>
		/// The WM_NULL message performs no operation. An application sends the WM_NULL message if it wants to post a message that the recipient window will ignore.
		/// </summary>
		NULL = 0x0000,
		/// <summary>
		/// The WM_CREATE message is sent when an application requests that a window be created by calling the CreateWindowEx or CreateWindow function. (The message is sent before the function returns.) The window procedure of the new window receives this message after the window is created, but before the window becomes visible.
		/// </summary>
		CREATE = 0x0001,
		/// <summary>
		/// The WM_DESTROY message is sent when a window is being destroyed. It is sent to the window procedure of the window being destroyed after the window is removed from the screen.
		/// This message is sent first to the window being destroyed and then to the child windows (if any) as they are destroyed. During the processing of the message, it can be assumed that all child windows still exist.
		/// /// </summary>
		DESTROY = 0x0002,
		/// <summary>
		/// The WM_MOVE message is sent after a window has been moved.
		/// </summary>
		MOVE = 0x0003,
		/// <summary>
		/// The WM_SIZE message is sent to a window after its size has changed.
		/// </summary>
		SIZE = 0x0005,
		/// <summary>
		/// The WM_ACTIVATE message is sent to both the window being activated and the window being deactivated. If the windows use the same input queue, the message is sent synchronously, first to the window procedure of the top-level window being deactivated, then to the window procedure of the top-level window being activated. If the windows use different input queues, the message is sent asynchronously, so the window is activated immediately.
		/// </summary>
		ACTIVATE = 0x0006,
		/// <summary>
		/// The WM_SETFOCUS message is sent to a window after it has gained the keyboard focus.
		/// </summary>
		SETFOCUS = 0x0007,
		/// <summary>
		/// The WM_KILLFOCUS message is sent to a window immediately before it loses the keyboard focus.
		/// </summary>
		KILLFOCUS = 0x0008,
		/// <summary>
		/// The WM_ENABLE message is sent when an application changes the enabled state of a window. It is sent to the window whose enabled state is changing. This message is sent before the EnableWindow function returns, but after the enabled state (WS_DISABLED style bit) of the window has changed.
		/// </summary>
		ENABLE = 0x000A,
		/// <summary>
		/// An application sends the WM_SETREDRAW message to a window to allow changes in that window to be redrawn or to prevent changes in that window from being redrawn.
		/// </summary>
		SETREDRAW = 0x000B,
		/// <summary>
		/// An application sends a WM_SETTEXT message to set the text of a window.
		/// </summary>
		SETTEXT = 0x000C,
		/// <summary>
		/// An application sends a WM_GETTEXT message to copy the text that corresponds to a window into a buffer provided by the caller.
		/// </summary>
		GETTEXT = 0x000D,
		/// <summary>
		/// An application sends a WM_GETTEXTLENGTH message to determine the length, in characters, of the text associated with a window.
		/// </summary>
		GETTEXTLENGTH = 0x000E,
		/// <summary>
		/// The WM_PAINT message is sent when the system or another application makes a request to paint a portion of an application's window. The message is sent when the UpdateWindow or RedrawWindow function is called, or by the DispatchMessage function when the application obtains a WM_PAINT message by using the GetMessage or PeekMessage function.
		/// </summary>
		PAINT = 0x000F,
		/// <summary>
		/// The WM_CLOSE message is sent as a signal that a window or an application should terminate.
		/// </summary>
		CLOSE = 0x0010,
		/// <summary>
		/// The WM_QUERYENDSESSION message is sent when the user chooses to end the session or when an application calls one of the system shutdown functions. If any application returns zero, the session is not ended. The system stops sending WM_QUERYENDSESSION messages as soon as one application returns zero.
		/// After processing this message, the system sends the WM_ENDSESSION message with the wParam parameter set to the results of the WM_QUERYENDSESSION message.
		/// </summary>
		QUERYENDSESSION = 0x0011,
		/// <summary>
		/// The WM_QUERYOPEN message is sent to an icon when the user requests that the window be restored to its previous size and position.
		/// </summary>
		QUERYOPEN = 0x0013,
		/// <summary>
		/// The WM_ENDSESSION message is sent to an application after the system processes the results of the WM_QUERYENDSESSION message. The WM_ENDSESSION message informs the application whether the session is ending.
		/// </summary>
		ENDSESSION = 0x0016,
		/// <summary>
		/// The WM_QUIT message indicates a request to terminate an application and is generated when the application calls the PostQuitMessage function. It causes the GetMessage function to return zero.
		/// </summary>
		QUIT = 0x0012,
		/// <summary>
		/// The WM_ERASEBKGND message is sent when the window background must be erased (for example, when a window is resized). The message is sent to prepare an invalidated portion of a window for painting.
		/// </summary>
		ERASEBKGND = 0x0014,
		/// <summary>
		/// This message is sent to all top-level windows when a change is made to a system color setting.
		/// </summary>
		SYSCOLORCHANGE = 0x0015,
		/// <summary>
		/// The WM_SHOWWINDOW message is sent to a window when the window is about to be hidden or shown.
		/// </summary>
		SHOWWINDOW = 0x0018,
		/// <summary>
		/// An application sends the WM_WININICHANGE message to all top-level windows after making a change to the WIN.INI file. The SystemParametersInfo function sends this message after an application uses the function to change a setting in WIN.INI.
		/// Note  The WM_WININICHANGE message is provided only for compatibility with earlier versions of the system. Applications should use the WM_SETTINGCHANGE message.
		/// </summary>
		WININICHANGE = 0x001A,
		/// <summary>
		/// An application sends the WM_WININICHANGE message to all top-level windows after making a change to the WIN.INI file. The SystemParametersInfo function sends this message after an application uses the function to change a setting in WIN.INI.
		/// Note  The WM_WININICHANGE message is provided only for compatibility with earlier versions of the system. Applications should use the WM_SETTINGCHANGE message.
		/// </summary>
		SETTINGCHANGE = WININICHANGE,
		/// <summary>
		/// The WM_DEVMODECHANGE message is sent to all top-level windows whenever the user changes device-mode settings.
		/// </summary>
		DEVMODECHANGE = 0x001B,
		/// <summary>
		/// The WM_ACTIVATEAPP message is sent when a window belonging to a different application than the active window is about to be activated. The message is sent to the application whose window is being activated and to the application whose window is being deactivated.
		/// </summary>
		ACTIVATEAPP = 0x001C,
		/// <summary>
		/// An application sends the WM_FONTCHANGE message to all top-level windows in the system after changing the pool of font resources.
		/// </summary>
		FONTCHANGE = 0x001D,
		/// <summary>
		/// A message that is sent whenever there is a change in the system time.
		/// </summary>
		TIMECHANGE = 0x001E,
		/// <summary>
		/// The WM_CANCELMODE message is sent to cancel certain modes, such as mouse capture. For example, the system sends this message to the active window when a dialog box or message box is displayed. Certain functions also send this message explicitly to the specified window regardless of whether it is the active window. For example, the EnableWindow function sends this message when disabling the specified window.
		/// </summary>
		CANCELMODE = 0x001F,
		/// <summary>
		/// The WM_SETCURSOR message is sent to a window if the mouse causes the cursor to move within a window and mouse input is not captured.
		/// </summary>
		SETCURSOR = 0x0020,
		/// <summary>
		/// The WM_MOUSEACTIVATE message is sent when the cursor is in an inactive window and the user presses a mouse button. The parent window receives this message only if the child window passes it to the DefWindowProc function.
		/// </summary>
		MOUSEACTIVATE = 0x0021,
		/// <summary>
		/// The WM_CHILDACTIVATE message is sent to a child window when the user clicks the window's title bar or when the window is activated, moved, or sized.
		/// </summary>
		CHILDACTIVATE = 0x0022,
		/// <summary>
		/// The WM_QUEUESYNC message is sent by a computer-based training (CBT) application to separate user-input messages from other messages sent through the WH_JOURNALPLAYBACK Hook procedure.
		/// </summary>
		QUEUESYNC = 0x0023,
		/// <summary>
		/// The WM_GETMINMAXINFO message is sent to a window when the size or position of the window is about to change. An application can use this message to override the window's default maximized size and position, or its default minimum or maximum tracking size.
		/// </summary>
		GETMINMAXINFO = 0x0024,
		/// <summary>
		/// Windows NT 3.51 and earlier: The WM_PAINTICON message is sent to a minimized window when the icon is to be painted. This message is not sent by newer versions of Microsoft Windows, except in unusual circumstances explained in the Remarks.
		/// </summary>
		PAINTICON = 0x0026,
		/// <summary>
		/// Windows NT 3.51 and earlier: The WM_ICONERASEBKGND message is sent to a minimized window when the background of the icon must be filled before painting the icon. A window receives this message only if a class icon is defined for the window; otherwise, WM_ERASEBKGND is sent. This message is not sent by newer versions of Windows.
		/// </summary>
		ICONERASEBKGND = 0x0027,
		/// <summary>
		/// The WM_NEXTDLGCTL message is sent to a dialog box procedure to set the keyboard focus to a different control in the dialog box.
		/// </summary>
		NEXTDLGCTL = 0x0028,
		/// <summary>
		/// The WM_SPOOLERSTATUS message is sent from Print Manager whenever a job is added to or removed from the Print Manager queue.
		/// </summary>
		SPOOLERSTATUS = 0x002A,
		/// <summary>
		/// The WM_DRAWITEM message is sent to the parent window of an owner-drawn button, combo box, list box, or menu when a visual aspect of the button, combo box, list box, or menu has changed.
		/// </summary>
		DRAWITEM = 0x002B,
		/// <summary>
		/// The WM_MEASUREITEM message is sent to the owner window of a combo box, list box, list view control, or menu item when the control or menu is created.
		/// </summary>
		MEASUREITEM = 0x002C,
		/// <summary>
		/// Sent to the owner of a list box or combo box when the list box or combo box is destroyed or when items are removed by the LB_DELETESTRING, LB_RESETCONTENT, CB_DELETESTRING, or CB_RESETCONTENT message. The system sends a WM_DELETEITEM message for each deleted item. The system sends the WM_DELETEITEM message for any deleted list box or combo box item with nonzero item data.
		/// </summary>
		DELETEITEM = 0x002D,
		/// <summary>
		/// Sent by a list box with the LBS_WANTKEYBOARDINPUT style to its owner in response to a WM_KEYDOWN message.
		/// </summary>
		VKEYTOITEM = 0x002E,
		/// <summary>
		/// Sent by a list box with the LBS_WANTKEYBOARDINPUT style to its owner in response to a WM_CHAR message.
		/// </summary>
		CHARTOITEM = 0x002F,
		/// <summary>
		/// An application sends a WM_SETFONT message to specify the font that a control is to use when drawing text.
		/// </summary>
		SETFONT = 0x0030,
		/// <summary>
		/// An application sends a WM_GETFONT message to a control to retrieve the font with which the control is currently drawing its text.
		/// </summary>
		GETFONT = 0x0031,
		/// <summary>
		/// An application sends a WM_SETHOTKEY message to a window to associate a hot key with the window. When the user presses the hot key, the system activates the window.
		/// </summary>
		SETHOTKEY = 0x0032,
		/// <summary>
		/// An application sends a WM_GETHOTKEY message to determine the hot key associated with a window.
		/// </summary>
		GETHOTKEY = 0x0033,
		/// <summary>
		/// The WM_QUERYDRAGICON message is sent to a minimized (iconic) window. The window is about to be dragged by the user but does not have an icon defined for its class. An application can return a handle to an icon or cursor. The system displays this cursor or icon while the user drags the icon.
		/// </summary>
		QUERYDRAGICON = 0x0037,
		/// <summary>
		/// The system sends the WM_COMPAREITEM message to determine the relative position of a new item in the sorted list of an owner-drawn combo box or list box. Whenever the application adds a new item, the system sends this message to the owner of a combo box or list box created with the CBS_SORT or LBS_SORT style.
		/// </summary>
		COMPAREITEM = 0x0039,
		/// <summary>
		/// Active Accessibility sends the WM_GETOBJECT message to obtain information about an accessible object contained in a server application.
		/// Applications never send this message directly. It is sent only by Active Accessibility in response to calls to AccessibleObjectFromPoint, AccessibleObjectFromEvent, or AccessibleObjectFromWindow. However, server applications handle this message.
		/// </summary>
		GETOBJECT = 0x003D,
		/// <summary>
		/// The WM_COMPACTING message is sent to all top-level windows when the system detects more than 12.5 percent of system time over a 30- to 60-second interval is being spent compacting memory. This indicates that system memory is low.
		/// </summary>
		COMPACTING = 0x0041,
		/// <summary>
		/// WM_COMMNOTIFY is Obsolete for Win32-Based Applications
		/// </summary>
		[Obsolete]
		COMMNOTIFY = 0x0044,
		/// <summary>
		/// The WM_WINDOWPOSCHANGING message is sent to a window whose size, position, or place in the Z order is about to change as a result of a call to the SetWindowPos function or another window-management function.
		/// </summary>
		WINDOWPOSCHANGING = 0x0046,
		/// <summary>
		/// The WM_WINDOWPOSCHANGED message is sent to a window whose size, position, or place in the Z order has changed as a result of a call to the SetWindowPos function or another window-management function.
		/// </summary>
		WINDOWPOSCHANGED = 0x0047,
		/// <summary>
		/// Notifies applications that the system, typically a battery-powered personal computer, is about to enter a suspended mode.
		/// Use: POWERBROADCAST
		/// </summary>
		[Obsolete]
		POWER = 0x0048,
		/// <summary>
		/// An application sends the WM_COPYDATA message to pass data to another application.
		/// </summary>
		COPYDATA = 0x004A,
		/// <summary>
		/// The WM_CANCELJOURNAL message is posted to an application when a user cancels the application's journaling activities. The message is posted with a NULL window handle.
		/// </summary>
		CANCELJOURNAL = 0x004B,
		/// <summary>
		/// Sent by a common control to its parent window when an event has occurred or the control requires some information.
		/// </summary>
		NOTIFY = 0x004E,
		/// <summary>
		/// The WM_INPUTLANGCHANGEREQUEST message is posted to the window with the focus when the user chooses a new input language, either with the hotkey (specified in the Keyboard control panel application) or from the indicator on the system taskbar. An application can accept the change by passing the message to the DefWindowProc function or reject the change (and prevent it from taking place) by returning immediately.
		/// </summary>
		INPUTLANGCHANGEREQUEST = 0x0050,
		/// <summary>
		/// The WM_INPUTLANGCHANGE message is sent to the topmost affected window after an application's input language has been changed. You should make any application-specific settings and pass the message to the DefWindowProc function, which passes the message to all first-level child windows. These child windows can pass the message to DefWindowProc to have it pass the message to their child windows, and so on.
		/// </summary>
		INPUTLANGCHANGE = 0x0051,
		/// <summary>
		/// Sent to an application that has initiated a training card with Microsoft Windows Help. The message informs the application when the user clicks an authorable button. An application initiates a training card by specifying the HELP_TCARD command in a call to the WinHelp function.
		/// </summary>
		TCARD = 0x0052,
		/// <summary>
		/// Indicates that the user pressed the F1 key. If a menu is active when F1 is pressed, WM_HELP is sent to the window associated with the menu; otherwise, WM_HELP is sent to the window that has the keyboard focus. If no window has the keyboard focus, WM_HELP is sent to the currently active window.
		/// </summary>
		HELP = 0x0053,
		/// <summary>
		/// The WM_USERCHANGED message is sent to all windows after the user has logged on or off. When the user logs on or off, the system updates the user-specific settings. The system sends this message immediately after updating the settings.
		/// </summary>
		USERCHANGED = 0x0054,
		/// <summary>
		/// Determines if a window accepts ANSI or Unicode structures in the WM_NOTIFY notification message. WM_NOTIFYFORMAT messages are sent from a common control to its parent window and from the parent window to the common control.
		/// </summary>
		NOTIFYFORMAT = 0x0055,
		/// <summary>
		/// The WM_CONTEXTMENU message notifies a window that the user clicked the right mouse button (right-clicked) in the window.
		/// </summary>
		CONTEXTMENU = 0x007B,
		/// <summary>
		/// The WM_STYLECHANGING message is sent to a window when the SetWindowLong function is about to change one or more of the window's styles.
		/// </summary>
		STYLECHANGING = 0x007C,
		/// <summary>
		/// The WM_STYLECHANGED message is sent to a window after the SetWindowLong function has changed one or more of the window's styles
		/// </summary>
		STYLECHANGED = 0x007D,
		/// <summary>
		/// The WM_DISPLAYCHANGE message is sent to all windows when the display resolution has changed.
		/// </summary>
		DISPLAYCHANGE = 0x007E,
		/// <summary>
		/// The WM_GETICON message is sent to a window to retrieve a handle to the large or small icon associated with a window. The system displays the large icon in the ALT+TAB dialog, and the small icon in the window caption.
		/// </summary>
		GETICON = 0x007F,
		/// <summary>
		/// An application sends the WM_SETICON message to associate a new large or small icon with a window. The system displays the large icon in the ALT+TAB dialog box, and the small icon in the window caption.
		/// </summary>
		SETICON = 0x0080,
		/// <summary>
		/// The WM_NCCREATE message is sent prior to the WM_CREATE message when a window is first created.
		/// </summary>
		NCCREATE = 0x0081,
		/// <summary>
		/// The WM_NCDESTROY message informs a window that its nonclient area is being destroyed. The DestroyWindow function sends the WM_NCDESTROY message to the window following the WM_DESTROY message. WM_DESTROY is used to free the allocated memory object associated with the window.
		/// The WM_NCDESTROY message is sent after the child windows have been destroyed. In contrast, WM_DESTROY is sent before the child windows are destroyed.
		/// </summary>
		NCDESTROY = 0x0082,
		/// <summary>
		/// The WM_NCCALCSIZE message is sent when the size and position of a window's client area must be calculated. By processing this message, an application can control the content of the window's client area when the size or position of the window changes.
		/// </summary>
		NCCALCSIZE = 0x0083,
		/// <summary>
		/// The WM_NCHITTEST message is sent to a window when the cursor moves, or when a mouse button is pressed or released. If the mouse is not captured, the message is sent to the window beneath the cursor. Otherwise, the message is sent to the window that has captured the mouse.
		/// </summary>
		NCHITTEST = 0x0084,
		/// <summary>
		/// The WM_NCPAINT message is sent to a window when its frame must be painted.
		/// </summary>
		NCPAINT = 0x0085,
		/// <summary>
		/// The WM_NCACTIVATE message is sent to a window when its nonclient area needs to be changed to indicate an active or inactive state.
		/// </summary>
		NCACTIVATE = 0x0086,
		/// <summary>
		/// The WM_GETDLGCODE message is sent to the window procedure associated with a control. By default, the system handles all keyboard input to the control; the system interprets certain types of keyboard input as dialog box navigation keys. To override this default behavior, the control can respond to the WM_GETDLGCODE message to indicate the types of input it wants to process itself.
		/// </summary>
		GETDLGCODE = 0x0087,
		/// <summary>
		/// The WM_SYNCPAINT message is used to synchronize painting while avoiding linking independent GUI threads.
		/// </summary>
		SYNCPAINT = 0x0088,
		/// <summary>
		/// The WM_NCMOUSEMOVE message is posted to a window when the cursor is moved within the nonclient area of the window. This message is posted to the window that contains the cursor. If a window has captured the mouse, this message is not posted.
		/// </summary>
		NCMOUSEMOVE = 0x00A0,
		/// <summary>
		/// The WM_NCLBUTTONDOWN message is posted when the user presses the left mouse button while the cursor is within the nonclient area of a window. This message is posted to the window that contains the cursor. If a window has captured the mouse, this message is not posted.
		/// </summary>
		NCLBUTTONDOWN = 0x00A1,
		/// <summary>
		/// The WM_NCLBUTTONUP message is posted when the user releases the left mouse button while the cursor is within the nonclient area of a window. This message is posted to the window that contains the cursor. If a window has captured the mouse, this message is not posted.
		/// </summary>
		NCLBUTTONUP = 0x00A2,
		/// <summary>
		/// The WM_NCLBUTTONDBLCLK message is posted when the user double-clicks the left mouse button while the cursor is within the nonclient area of a window. This message is posted to the window that contains the cursor. If a window has captured the mouse, this message is not posted.
		/// </summary>
		NCLBUTTONDBLCLK = 0x00A3,
		/// <summary>
		/// The WM_NCRBUTTONDOWN message is posted when the user presses the right mouse button while the cursor is within the nonclient area of a window. This message is posted to the window that contains the cursor. If a window has captured the mouse, this message is not posted.
		/// </summary>
		NCRBUTTONDOWN = 0x00A4,
		/// <summary>
		/// The WM_NCRBUTTONUP message is posted when the user releases the right mouse button while the cursor is within the nonclient area of a window. This message is posted to the window that contains the cursor. If a window has captured the mouse, this message is not posted.
		/// </summary>
		NCRBUTTONUP = 0x00A5,
		/// <summary>
		/// The WM_NCRBUTTONDBLCLK message is posted when the user double-clicks the right mouse button while the cursor is within the nonclient area of a window. This message is posted to the window that contains the cursor. If a window has captured the mouse, this message is not posted.
		/// </summary>
		NCRBUTTONDBLCLK = 0x00A6,
		/// <summary>
		/// The WM_NCMBUTTONDOWN message is posted when the user presses the middle mouse button while the cursor is within the nonclient area of a window. This message is posted to the window that contains the cursor. If a window has captured the mouse, this message is not posted.
		/// </summary>
		NCMBUTTONDOWN = 0x00A7,
		/// <summary>
		/// The WM_NCMBUTTONUP message is posted when the user releases the middle mouse button while the cursor is within the nonclient area of a window. This message is posted to the window that contains the cursor. If a window has captured the mouse, this message is not posted.
		/// </summary>
		NCMBUTTONUP = 0x00A8,
		/// <summary>
		/// The WM_NCMBUTTONDBLCLK message is posted when the user double-clicks the middle mouse button while the cursor is within the nonclient area of a window. This message is posted to the window that contains the cursor. If a window has captured the mouse, this message is not posted.
		/// </summary>
		NCMBUTTONDBLCLK = 0x00A9,
		/// <summary>
		/// The WM_NCXBUTTONDOWN message is posted when the user presses the first or second X button while the cursor is in the nonclient area of a window. This message is posted to the window that contains the cursor. If a window has captured the mouse, this message is not posted.
		/// </summary>
		NCXBUTTONDOWN = 0x00AB,
		/// <summary>
		/// The WM_NCXBUTTONUP message is posted when the user releases the first or second X button while the cursor is in the nonclient area of a window. This message is posted to the window that contains the cursor. If a window has captured the mouse, this message is not posted.
		/// </summary>
		NCXBUTTONUP = 0x00AC,
		/// <summary>
		/// The WM_NCXBUTTONDBLCLK message is posted when the user double-clicks the first or second X button while the cursor is in the nonclient area of a window. This message is posted to the window that contains the cursor. If a window has captured the mouse, this message is not posted.
		/// </summary>
		NCXBUTTONDBLCLK = 0x00AD,
		/// <summary>
		/// The WM_INPUT_DEVICE_CHANGE message is sent to the window that registered to receive raw input. A window receives this message through its WindowProc function.
		/// </summary>
		INPUT_DEVICE_CHANGE = 0x00FE,
		/// <summary>
		/// The WM_INPUT message is sent to the window that is getting raw input.
		/// </summary>
		INPUT = 0x00FF,
		/// <summary>
		/// This message filters for keyboard messages.
		/// </summary>
		KEYFIRST = 0x0100,
		/// <summary>
		/// The WM_KEYDOWN message is posted to the window with the keyboard focus when a nonsystem key is pressed. A nonsystem key is a key that is pressed when the ALT key is not pressed.
		/// </summary>
		KEYDOWN = 0x0100,
		/// <summary>
		/// The WM_KEYUP message is posted to the window with the keyboard focus when a nonsystem key is released. A nonsystem key is a key that is pressed when the ALT key is not pressed, or a keyboard key that is pressed when a window has the keyboard focus.
		/// </summary>
		KEYUP = 0x0101,
		/// <summary>
		/// The WM_CHAR message is posted to the window with the keyboard focus when a WM_KEYDOWN message is translated by the TranslateMessage function. The WM_CHAR message contains the character code of the key that was pressed.
		/// </summary>
		CHAR = 0x0102,
		/// <summary>
		/// The WM_DEADCHAR message is posted to the window with the keyboard focus when a WM_KEYUP message is translated by the TranslateMessage function. WM_DEADCHAR specifies a character code generated by a dead key. A dead key is a key that generates a character, such as the umlaut (double-dot), that is combined with another character to form a composite character. For example, the umlaut-O character (Ö) is generated by typing the dead key for the umlaut character, and then typing the O key.
		/// </summary>
		DEADCHAR = 0x0103,
		/// <summary>
		/// The WM_SYSKEYDOWN message is posted to the window with the keyboard focus when the user presses the F10 key (which activates the menu bar) or holds down the ALT key and then presses another key. It also occurs when no window currently has the keyboard focus; in this case, the WM_SYSKEYDOWN message is sent to the active window. The window that receives the message can distinguish between these two contexts by checking the context code in the lParam parameter.
		/// </summary>
		SYSKEYDOWN = 0x0104,
		/// <summary>
		/// The WM_SYSKEYUP message is posted to the window with the keyboard focus when the user releases a key that was pressed while the ALT key was held down. It also occurs when no window currently has the keyboard focus; in this case, the WM_SYSKEYUP message is sent to the active window. The window that receives the message can distinguish between these two contexts by checking the context code in the lParam parameter.
		/// </summary>
		SYSKEYUP = 0x0105,
		/// <summary>
		/// The WM_SYSCHAR message is posted to the window with the keyboard focus when a WM_SYSKEYDOWN message is translated by the TranslateMessage function. It specifies the character code of a system character key — that is, a character key that is pressed while the ALT key is down.
		/// </summary>
		SYSCHAR = 0x0106,
		/// <summary>
		/// The WM_SYSDEADCHAR message is sent to the window with the keyboard focus when a WM_SYSKEYDOWN message is translated by the TranslateMessage function. WM_SYSDEADCHAR specifies the character code of a system dead key — that is, a dead key that is pressed while holding down the ALT key.
		/// </summary>
		SYSDEADCHAR = 0x0107,
		/// <summary>
		/// The WM_UNICHAR message is posted to the window with the keyboard focus when a WM_KEYDOWN message is translated by the TranslateMessage function. The WM_UNICHAR message contains the character code of the key that was pressed.
		/// The WM_UNICHAR message is equivalent to WM_CHAR, but it uses Unicode Transformation Format (UTF)-32, whereas WM_CHAR uses UTF-16. It is designed to send or post Unicode characters to ANSI windows and it can can handle Unicode Supplementary Plane characters.
		/// </summary>
		UNICHAR = 0x0109,
		/// <summary>
		/// This message filters for keyboard messages.
		/// </summary>
		KEYLAST = 0x0108,
		/// <summary>
		/// Sent immediately before the IME generates the composition string as a result of a keystroke. A window receives this message through its WindowProc function.
		/// </summary>
		IME_STARTCOMPOSITION = 0x010D,
		/// <summary>
		/// Sent to an application when the IME ends composition. A window receives this message through its WindowProc function.
		/// </summary>
		IME_ENDCOMPOSITION = 0x010E,
		/// <summary>
		/// Sent to an application when the IME changes composition status as a result of a keystroke. A window receives this message through its WindowProc function.
		/// </summary>
		IME_COMPOSITION = 0x010F,
		IME_KEYLAST = 0x010F,
		/// <summary>
		/// The WM_INITDIALOG message is sent to the dialog box procedure immediately before a dialog box is displayed. Dialog box procedures typically use this message to initialize controls and carry out any other initialization tasks that affect the appearance of the dialog box.
		/// </summary>
		INITDIALOG = 0x0110,
		/// <summary>
		/// The WM_COMMAND message is sent when the user selects a command item from a menu, when a control sends a notification message to its parent window, or when an accelerator keystroke is translated.
		/// </summary>
		COMMAND = 0x0111,
		/// <summary>
		/// A window receives this message when the user chooses a command from the Window menu, clicks the maximize button, minimize button, restore button, close button, or moves the form. You can stop the form from moving by filtering this out.
		/// </summary>
		SYSCOMMAND = 0x0112,
		/// <summary>
		/// The WM_TIMER message is posted to the installing thread's message queue when a timer expires. The message is posted by the GetMessage or PeekMessage function.
		/// </summary>
		TIMER = 0x0113,
		/// <summary>
		/// The WM_HSCROLL message is sent to a window when a scroll event occurs in the window's standard horizontal scroll bar. This message is also sent to the owner of a horizontal scroll bar control when a scroll event occurs in the control.
		/// </summary>
		HSCROLL = 0x0114,
		/// <summary>
		/// The WM_VSCROLL message is sent to a window when a scroll event occurs in the window's standard vertical scroll bar. This message is also sent to the owner of a vertical scroll bar control when a scroll event occurs in the control.
		/// </summary>
		VSCROLL = 0x0115,
		/// <summary>
		/// The WM_INITMENU message is sent when a menu is about to become active. It occurs when the user clicks an item on the menu bar or presses a menu key. This allows the application to modify the menu before it is displayed.
		/// </summary>
		INITMENU = 0x0116,
		/// <summary>
		/// The WM_INITMENUPOPUP message is sent when a drop-down menu or submenu is about to become active. This allows an application to modify the menu before it is displayed, without changing the entire menu.
		/// </summary>
		INITMENUPOPUP = 0x0117,
		/// <summary>
		/// The WM_MENUSELECT message is sent to a menu's owner window when the user selects a menu item.
		/// </summary>
		MENUSELECT = 0x011F,
		/// <summary>
		/// The WM_MENUCHAR message is sent when a menu is active and the user presses a key that does not correspond to any mnemonic or accelerator key. This message is sent to the window that owns the menu.
		/// </summary>
		MENUCHAR = 0x0120,
		/// <summary>
		/// The WM_ENTERIDLE message is sent to the owner window of a modal dialog box or menu that is entering an idle state. A modal dialog box or menu enters an idle state when no messages are waiting in its queue after it has processed one or more previous messages.
		/// </summary>
		ENTERIDLE = 0x0121,
		/// <summary>
		/// The WM_MENURBUTTONUP message is sent when the user releases the right mouse button while the cursor is on a menu item.
		/// </summary>
		MENURBUTTONUP = 0x0122,
		/// <summary>
		/// The WM_MENUDRAG message is sent to the owner of a drag-and-drop menu when the user drags a menu item.
		/// </summary>
		MENUDRAG = 0x0123,
		/// <summary>
		/// The WM_MENUGETOBJECT message is sent to the owner of a drag-and-drop menu when the mouse cursor enters a menu item or moves from the center of the item to the top or bottom of the item.
		/// </summary>
		MENUGETOBJECT = 0x0124,
		/// <summary>
		/// The WM_UNINITMENUPOPUP message is sent when a drop-down menu or submenu has been destroyed.
		/// </summary>
		UNINITMENUPOPUP = 0x0125,
		/// <summary>
		/// The WM_MENUCOMMAND message is sent when the user makes a selection from a menu.
		/// </summary>
		MENUCOMMAND = 0x0126,
		/// <summary>
		/// An application sends the WM_CHANGEUISTATE message to indicate that the user interface (UI) state should be changed.
		/// </summary>
		CHANGEUISTATE = 0x0127,
		/// <summary>
		/// An application sends the WM_UPDATEUISTATE message to change the user interface (UI) state for the specified window and all its child windows.
		/// </summary>
		UPDATEUISTATE = 0x0128,
		/// <summary>
		/// An application sends the WM_QUERYUISTATE message to retrieve the user interface (UI) state for a window.
		/// </summary>
		QUERYUISTATE = 0x0129,
		/// <summary>
		/// The WM_CTLCOLORMSGBOX message is sent to the owner window of a message box before Windows draws the message box. By responding to this message, the owner window can set the text and background colors of the message box by using the given display device context handle.
		/// </summary>
		CTLCOLORMSGBOX = 0x0132,
		/// <summary>
		/// An edit control that is not read-only or disabled sends the WM_CTLCOLOREDIT message to its parent window when the control is about to be drawn. By responding to this message, the parent window can use the specified device context handle to set the text and background colors of the edit control.
		/// </summary>
		CTLCOLOREDIT = 0x0133,
		/// <summary>
		/// Sent to the parent window of a list box before the system draws the list box. By responding to this message, the parent window can set the text and background colors of the list box by using the specified display device context handle.
		/// </summary>
		CTLCOLORLISTBOX = 0x0134,
		/// <summary>
		/// The WM_CTLCOLORBTN message is sent to the parent window of a button before drawing the button. The parent window can change the button's text and background colors. However, only owner-drawn buttons respond to the parent window processing this message.
		/// </summary>
		CTLCOLORBTN = 0x0135,
		/// <summary>
		/// The WM_CTLCOLORDLG message is sent to a dialog box before the system draws the dialog box. By responding to this message, the dialog box can set its text and background colors using the specified display device context handle.
		/// </summary>
		CTLCOLORDLG = 0x0136,
		/// <summary>
		/// The WM_CTLCOLORSCROLLBAR message is sent to the parent window of a scroll bar control when the control is about to be drawn. By responding to this message, the parent window can use the display context handle to set the background color of the scroll bar control.
		/// </summary>
		CTLCOLORSCROLLBAR = 0x0137,
		/// <summary>
		/// A static control, or an edit control that is read-only or disabled, sends the WM_CTLCOLORSTATIC message to its parent window when the control is about to be drawn. By responding to this message, the parent window can use the specified device context handle to set the text and background colors of the static control.
		/// </summary>
		CTLCOLORSTATIC = 0x0138,
		/// <summary>
		/// Use WM_MOUSEFIRST to specify the first mouse message. Use the PeekMessage() Function.
		/// </summary>
		MOUSEFIRST = 0x0200,
		/// <summary>
		/// The WM_MOUSEMOVE message is posted to a window when the cursor moves. If the mouse is not captured, the message is posted to the window that contains the cursor. Otherwise, the message is posted to the window that has captured the mouse.
		/// </summary>
		MOUSEMOVE = 0x0200,
		/// <summary>
		/// The WM_LBUTTONDOWN message is posted when the user presses the left mouse button while the cursor is in the client area of a window. If the mouse is not captured, the message is posted to the window beneath the cursor. Otherwise, the message is posted to the window that has captured the mouse.
		/// </summary>
		LBUTTONDOWN = 0x0201,
		/// <summary>
		/// The WM_LBUTTONUP message is posted when the user releases the left mouse button while the cursor is in the client area of a window. If the mouse is not captured, the message is posted to the window beneath the cursor. Otherwise, the message is posted to the window that has captured the mouse.
		/// </summary>
		LBUTTONUP = 0x0202,
		/// <summary>
		/// The WM_LBUTTONDBLCLK message is posted when the user double-clicks the left mouse button while the cursor is in the client area of a window. If the mouse is not captured, the message is posted to the window beneath the cursor. Otherwise, the message is posted to the window that has captured the mouse.
		/// </summary>
		LBUTTONDBLCLK = 0x0203,
		/// <summary>
		/// The WM_RBUTTONDOWN message is posted when the user presses the right mouse button while the cursor is in the client area of a window. If the mouse is not captured, the message is posted to the window beneath the cursor. Otherwise, the message is posted to the window that has captured the mouse.
		/// </summary>
		RBUTTONDOWN = 0x0204,
		/// <summary>
		/// The WM_RBUTTONUP message is posted when the user releases the right mouse button while the cursor is in the client area of a window. If the mouse is not captured, the message is posted to the window beneath the cursor. Otherwise, the message is posted to the window that has captured the mouse.
		/// </summary>
		RBUTTONUP = 0x0205,
		/// <summary>
		/// The WM_RBUTTONDBLCLK message is posted when the user double-clicks the right mouse button while the cursor is in the client area of a window. If the mouse is not captured, the message is posted to the window beneath the cursor. Otherwise, the message is posted to the window that has captured the mouse.
		/// </summary>
		RBUTTONDBLCLK = 0x0206,
		/// <summary>
		/// The WM_MBUTTONDOWN message is posted when the user presses the middle mouse button while the cursor is in the client area of a window. If the mouse is not captured, the message is posted to the window beneath the cursor. Otherwise, the message is posted to the window that has captured the mouse.
		/// </summary>
		MBUTTONDOWN = 0x0207,
		/// <summary>
		/// The WM_MBUTTONUP message is posted when the user releases the middle mouse button while the cursor is in the client area of a window. If the mouse is not captured, the message is posted to the window beneath the cursor. Otherwise, the message is posted to the window that has captured the mouse.
		/// </summary>
		MBUTTONUP = 0x0208,
		/// <summary>
		/// The WM_MBUTTONDBLCLK message is posted when the user double-clicks the middle mouse button while the cursor is in the client area of a window. If the mouse is not captured, the message is posted to the window beneath the cursor. Otherwise, the message is posted to the window that has captured the mouse.
		/// </summary>
		MBUTTONDBLCLK = 0x0209,
		/// <summary>
		/// The WM_MOUSEWHEEL message is sent to the focus window when the mouse wheel is rotated. The DefWindowProc function propagates the message to the window's parent. There should be no internal forwarding of the message, since DefWindowProc propagates it up the parent chain until it finds a window that processes it.
		/// </summary>
		MOUSEWHEEL = 0x020A,
		/// <summary>
		/// The WM_XBUTTONDOWN message is posted when the user presses the first or second X button while the cursor is in the client area of a window. If the mouse is not captured, the message is posted to the window beneath the cursor. Otherwise, the message is posted to the window that has captured the mouse.
		/// </summary>
		XBUTTONDOWN = 0x020B,
		/// <summary>
		/// The WM_XBUTTONUP message is posted when the user releases the first or second X button while the cursor is in the client area of a window. If the mouse is not captured, the message is posted to the window beneath the cursor. Otherwise, the message is posted to the window that has captured the mouse.
		/// </summary>
		XBUTTONUP = 0x020C,
		/// <summary>
		/// The WM_XBUTTONDBLCLK message is posted when the user double-clicks the first or second X button while the cursor is in the client area of a window. If the mouse is not captured, the message is posted to the window beneath the cursor. Otherwise, the message is posted to the window that has captured the mouse.
		/// </summary>
		XBUTTONDBLCLK = 0x020D,
		/// <summary>
		/// The WM_MOUSEHWHEEL message is sent to the focus window when the mouse's horizontal scroll wheel is tilted or rotated. The DefWindowProc function propagates the message to the window's parent. There should be no internal forwarding of the message, since DefWindowProc propagates it up the parent chain until it finds a window that processes it.
		/// </summary>
		MOUSEHWHEEL = 0x020E,
		/// <summary>
		/// Use WM_MOUSELAST to specify the last mouse message. Used with PeekMessage() Function.
		/// </summary>
		MOUSELAST = 0x020E,
		/// <summary>
		/// The WM_PARENTNOTIFY message is sent to the parent of a child window when the child window is created or destroyed, or when the user clicks a mouse button while the cursor is over the child window. When the child window is being created, the system sends WM_PARENTNOTIFY just before the CreateWindow or CreateWindowEx function that creates the window returns. When the child window is being destroyed, the system sends the message before any processing to destroy the window takes place.
		/// </summary>
		PARENTNOTIFY = 0x0210,
		/// <summary>
		/// The WM_ENTERMENULOOP message informs an application's main window procedure that a menu modal loop has been entered.
		/// </summary>
		ENTERMENULOOP = 0x0211,
		/// <summary>
		/// The WM_EXITMENULOOP message informs an application's main window procedure that a menu modal loop has been exited.
		/// </summary>
		EXITMENULOOP = 0x0212,
		/// <summary>
		/// The WM_NEXTMENU message is sent to an application when the right or left arrow key is used to switch between the menu bar and the system menu.
		/// </summary>
		NEXTMENU = 0x0213,
		/// <summary>
		/// The WM_SIZING message is sent to a window that the user is resizing. By processing this message, an application can monitor the size and position of the drag rectangle and, if needed, change its size or position.
		/// </summary>
		SIZING = 0x0214,
		/// <summary>
		/// The WM_CAPTURECHANGED message is sent to the window that is losing the mouse capture.
		/// </summary>
		CAPTURECHANGED = 0x0215,
		/// <summary>
		/// The WM_MOVING message is sent to a window that the user is moving. By processing this message, an application can monitor the position of the drag rectangle and, if needed, change its position.
		/// </summary>
		MOVING = 0x0216,
		/// <summary>
		/// Notifies applications that a power-management event has occurred.
		/// </summary>
		POWERBROADCAST = 0x0218,
		/// <summary>
		/// Notifies an application of a change to the hardware configuration of a device or the computer.
		/// </summary>
		DEVICECHANGE = 0x0219,
		/// <summary>
		/// An application sends the WM_MDICREATE message to a multiple-document interface (MDI) client window to create an MDI child window.
		/// </summary>
		MDICREATE = 0x0220,
		/// <summary>
		/// An application sends the WM_MDIDESTROY message to a multiple-document interface (MDI) client window to close an MDI child window.
		/// </summary>
		MDIDESTROY = 0x0221,
		/// <summary>
		/// An application sends the WM_MDIACTIVATE message to a multiple-document interface (MDI) client window to instruct the client window to activate a different MDI child window.
		/// </summary>
		MDIACTIVATE = 0x0222,
		/// <summary>
		/// An application sends the WM_MDIRESTORE message to a multiple-document interface (MDI) client window to restore an MDI child window from maximized or minimized size.
		/// </summary>
		MDIRESTORE = 0x0223,
		/// <summary>
		/// An application sends the WM_MDINEXT message to a multiple-document interface (MDI) client window to activate the next or previous child window.
		/// </summary>
		MDINEXT = 0x0224,
		/// <summary>
		/// An application sends the WM_MDIMAXIMIZE message to a multiple-document interface (MDI) client window to maximize an MDI child window. The system resizes the child window to make its client area fill the client window. The system places the child window's window menu icon in the rightmost position of the frame window's menu bar, and places the child window's restore icon in the leftmost position. The system also appends the title bar text of the child window to that of the frame window.
		/// </summary>
		MDIMAXIMIZE = 0x0225,
		/// <summary>
		/// An application sends the WM_MDITILE message to a multiple-document interface (MDI) client window to arrange all of its MDI child windows in a tile format.
		/// </summary>
		MDITILE = 0x0226,
		/// <summary>
		/// An application sends the WM_MDICASCADE message to a multiple-document interface (MDI) client window to arrange all its child windows in a cascade format.
		/// </summary>
		MDICASCADE = 0x0227,
		/// <summary>
		/// An application sends the WM_MDIICONARRANGE message to a multiple-document interface (MDI) client window to arrange all minimized MDI child windows. It does not affect child windows that are not minimized.
		/// </summary>
		MDIICONARRANGE = 0x0228,
		/// <summary>
		/// An application sends the WM_MDIGETACTIVE message to a multiple-document interface (MDI) client window to retrieve the handle to the active MDI child window.
		/// </summary>
		MDIGETACTIVE = 0x0229,
		/// <summary>
		/// An application sends the WM_MDISETMENU message to a multiple-document interface (MDI) client window to replace the entire menu of an MDI frame window, to replace the window menu of the frame window, or both.
		/// </summary>
		MDISETMENU = 0x0230,
		/// <summary>
		/// The WM_ENTERSIZEMOVE message is sent one time to a window after it enters the moving or sizing modal loop. The window enters the moving or sizing modal loop when the user clicks the window's title bar or sizing border, or when the window passes the WM_SYSCOMMAND message to the DefWindowProc function and the wParam parameter of the message specifies the SC_MOVE or SC_SIZE value. The operation is complete when DefWindowProc returns.
		/// The system sends the WM_ENTERSIZEMOVE message regardless of whether the dragging of full windows is enabled.
		/// </summary>
		ENTERSIZEMOVE = 0x0231,
		/// <summary>
		/// The WM_EXITSIZEMOVE message is sent one time to a window, after it has exited the moving or sizing modal loop. The window enters the moving or sizing modal loop when the user clicks the window's title bar or sizing border, or when the window passes the WM_SYSCOMMAND message to the DefWindowProc function and the wParam parameter of the message specifies the SC_MOVE or SC_SIZE value. The operation is complete when DefWindowProc returns.
		/// </summary>
		EXITSIZEMOVE = 0x0232,
		/// <summary>
		/// Sent when the user drops a file on the window of an application that has registered itself as a recipient of dropped files.
		/// </summary>
		DROPFILES = 0x0233,
		/// <summary>
		/// An application sends the WM_MDIREFRESHMENU message to a multiple-document interface (MDI) client window to refresh the window menu of the MDI frame window.
		/// </summary>
		MDIREFRESHMENU = 0x0234,
		/// <summary>
		/// Sent to an application when a window is activated. A window receives this message through its WindowProc function.
		/// </summary>
		IME_SETCONTEXT = 0x0281,
		/// <summary>
		/// Sent to an application to notify it of changes to the IME window. A window receives this message through its WindowProc function.
		/// </summary>
		IME_NOTIFY = 0x0282,
		/// <summary>
		/// Sent by an application to direct the IME window to carry out the requested command. The application uses this message to control the IME window that it has created. To send this message, the application calls the SendMessage function with the following parameters.
		/// </summary>
		IME_CONTROL = 0x0283,
		/// <summary>
		/// Sent to an application when the IME window finds no space to extend the area for the composition window. A window receives this message through its WindowProc function.
		/// </summary>
		IME_COMPOSITIONFULL = 0x0284,
		/// <summary>
		/// Sent to an application when the operating system is about to change the current IME. A window receives this message through its WindowProc function.
		/// </summary>
		IME_SELECT = 0x0285,
		/// <summary>
		/// Sent to an application when the IME gets a character of the conversion result. A window receives this message through its WindowProc function.
		/// </summary>
		IME_CHAR = 0x0286,
		/// <summary>
		/// Sent to an application to provide commands and request information. A window receives this message through its WindowProc function.
		/// </summary>
		IME_REQUEST = 0x0288,
		/// <summary>
		/// Sent to an application by the IME to notify the application of a key press and to keep message order. A window receives this message through its WindowProc function.
		/// </summary>
		IME_KEYDOWN = 0x0290,
		/// <summary>
		/// Sent to an application by the IME to notify the application of a key release and to keep message order. A window receives this message through its WindowProc function.
		/// </summary>
		IME_KEYUP = 0x0291,
		/// <summary>
		/// The WM_MOUSEHOVER message is posted to a window when the cursor hovers over the client area of the window for the period of time specified in a prior call to TrackMouseEvent.
		/// </summary>
		MOUSEHOVER = 0x02A1,
		/// <summary>
		/// The WM_MOUSELEAVE message is posted to a window when the cursor leaves the client area of the window specified in a prior call to TrackMouseEvent.
		/// </summary>
		MOUSELEAVE = 0x02A3,
		/// <summary>
		/// The WM_NCMOUSEHOVER message is posted to a window when the cursor hovers over the nonclient area of the window for the period of time specified in a prior call to TrackMouseEvent.
		/// </summary>
		NCMOUSEHOVER = 0x02A0,
		/// <summary>
		/// The WM_NCMOUSELEAVE message is posted to a window when the cursor leaves the nonclient area of the window specified in a prior call to TrackMouseEvent.
		/// </summary>
		NCMOUSELEAVE = 0x02A2,
		/// <summary>
		/// The WM_WTSSESSION_CHANGE message notifies applications of changes in session state.
		/// </summary>
		WTSSESSION_CHANGE = 0x02B1,
		TABLET_FIRST = 0x02c0,
		TABLET_LAST = 0x02df,
		/// <summary>
		/// An application sends a WM_CUT message to an edit control or combo box to delete (cut) the current selection, if any, in the edit control and copy the deleted text to the clipboard in CF_TEXT format.
		/// </summary>
		CUT = 0x0300,
		/// <summary>
		/// An application sends the WM_COPY message to an edit control or combo box to copy the current selection to the clipboard in CF_TEXT format.
		/// </summary>
		COPY = 0x0301,
		/// <summary>
		/// An application sends a WM_PASTE message to an edit control or combo box to copy the current content of the clipboard to the edit control at the current caret position. Data is inserted only if the clipboard contains data in CF_TEXT format.
		/// </summary>
		PASTE = 0x0302,
		/// <summary>
		/// An application sends a WM_CLEAR message to an edit control or combo box to delete (clear) the current selection, if any, from the edit control.
		/// </summary>
		CLEAR = 0x0303,
		/// <summary>
		/// An application sends a WM_UNDO message to an edit control to undo the last operation. When this message is sent to an edit control, the previously deleted text is restored or the previously added text is deleted.
		/// </summary>
		UNDO = 0x0304,
		/// <summary>
		/// The WM_RENDERFORMAT message is sent to the clipboard owner if it has delayed rendering a specific clipboard format and if an application has requested data in that format. The clipboard owner must render data in the specified format and place it on the clipboard by calling the SetClipboardData function.
		/// </summary>
		RENDERFORMAT = 0x0305,
		/// <summary>
		/// The WM_RENDERALLFORMATS message is sent to the clipboard owner before it is destroyed, if the clipboard owner has delayed rendering one or more clipboard formats. For the content of the clipboard to remain available to other applications, the clipboard owner must render data in all the formats it is capable of generating, and place the data on the clipboard by calling the SetClipboardData function.
		/// </summary>
		RENDERALLFORMATS = 0x0306,
		/// <summary>
		/// The WM_DESTROYCLIPBOARD message is sent to the clipboard owner when a call to the EmptyClipboard function empties the clipboard.
		/// </summary>
		DESTROYCLIPBOARD = 0x0307,
		/// <summary>
		/// The WM_DRAWCLIPBOARD message is sent to the first window in the clipboard viewer chain when the content of the clipboard changes. This enables a clipboard viewer window to display the new content of the clipboard.
		/// </summary>
		DRAWCLIPBOARD = 0x0308,
		/// <summary>
		/// The WM_PAINTCLIPBOARD message is sent to the clipboard owner by a clipboard viewer window when the clipboard contains data in the CF_OWNERDISPLAY format and the clipboard viewer's client area needs repainting.
		/// </summary>
		PAINTCLIPBOARD = 0x0309,
		/// <summary>
		/// The WM_VSCROLLCLIPBOARD message is sent to the clipboard owner by a clipboard viewer window when the clipboard contains data in the CF_OWNERDISPLAY format and an event occurs in the clipboard viewer's vertical scroll bar. The owner should scroll the clipboard image and update the scroll bar values.
		/// </summary>
		VSCROLLCLIPBOARD = 0x030A,
		/// <summary>
		/// The WM_SIZECLIPBOARD message is sent to the clipboard owner by a clipboard viewer window when the clipboard contains data in the CF_OWNERDISPLAY format and the clipboard viewer's client area has changed size.
		/// </summary>
		SIZECLIPBOARD = 0x030B,
		/// <summary>
		/// The WM_ASKCBFORMATNAME message is sent to the clipboard owner by a clipboard viewer window to request the name of a CF_OWNERDISPLAY clipboard format.
		/// </summary>
		ASKCBFORMATNAME = 0x030C,
		/// <summary>
		/// The WM_CHANGECBCHAIN message is sent to the first window in the clipboard viewer chain when a window is being removed from the chain.
		/// </summary>
		CHANGECBCHAIN = 0x030D,
		/// <summary>
		/// The WM_HSCROLLCLIPBOARD message is sent to the clipboard owner by a clipboard viewer window. This occurs when the clipboard contains data in the CF_OWNERDISPLAY format and an event occurs in the clipboard viewer's horizontal scroll bar. The owner should scroll the clipboard image and update the scroll bar values.
		/// </summary>
		HSCROLLCLIPBOARD = 0x030E,
		/// <summary>
		/// This message informs a window that it is about to receive the keyboard focus, giving the window the opportunity to realize its logical palette when it receives the focus.
		/// </summary>
		QUERYNEWPALETTE = 0x030F,
		/// <summary>
		/// The WM_PALETTEISCHANGING message informs applications that an application is going to realize its logical palette.
		/// </summary>
		PALETTEISCHANGING = 0x0310,
		/// <summary>
		/// This message is sent by the OS to all top-level and overlapped windows after the window with the keyboard focus realizes its logical palette.
		/// This message enables windows that do not have the keyboard focus to realize their logical palettes and update their client areas.
		/// </summary>
		PALETTECHANGED = 0x0311,
		/// <summary>
		/// The WM_HOTKEY message is posted when the user presses a hot key registered by the RegisterHotKey function. The message is placed at the top of the message queue associated with the thread that registered the hot key.
		/// </summary>
		HOTKEY = 0x0312,
		/// <summary>
		/// The WM_PRINT message is sent to a window to request that it draw itself in the specified device context, most commonly in a printer device context.
		/// </summary>
		PRINT = 0x0317,
		/// <summary>
		/// The WM_PRINTCLIENT message is sent to a window to request that it draw its client area in the specified device context, most commonly in a printer device context.
		/// </summary>
		PRINTCLIENT = 0x0318,
		/// <summary>
		/// The WM_APPCOMMAND message notifies a window that the user generated an application command event, for example, by clicking an application command button using the mouse or typing an application command key on the keyboard.
		/// </summary>
		APPCOMMAND = 0x0319,
		/// <summary>
		/// The WM_THEMECHANGED message is broadcast to every window following a theme change event. Examples of theme change events are the activation of a theme, the deactivation of a theme, or a transition from one theme to another.
		/// </summary>
		THEMECHANGED = 0x031A,
		/// <summary>
		/// Sent when the contents of the clipboard have changed.
		/// </summary>
		CLIPBOARDUPDATE = 0x031D,
		/// <summary>
		/// The system will send a window the WM_DWMCOMPOSITIONCHANGED message to indicate that the availability of desktop composition has changed.
		/// </summary>
		DWMCOMPOSITIONCHANGED = 0x031E,
		/// <summary>
		/// WM_DWMNCRENDERINGCHANGED is called when the non-client area rendering status of a window has changed. Only windows that have set the flag DWM_BLURBEHIND.fTransitionOnMaximized to true will get this message.
		/// </summary>
		DWMNCRENDERINGCHANGED = 0x031F,
		/// <summary>
		/// Sent to all top-level windows when the colorization color has changed.
		/// </summary>
		DWMCOLORIZATIONCOLORCHANGED = 0x0320,
		/// <summary>
		/// WM_DWMWINDOWMAXIMIZEDCHANGE will let you know when a DWM composed window is maximized. You also have to register for this message as well. You'd have other windowd go opaque when this message is sent.
		/// </summary>
		DWMWINDOWMAXIMIZEDCHANGE = 0x0321,
		/// <summary>
		/// Sent to request extended title bar information. A window receives this message through its WindowProc function.
		/// </summary>
		GETTITLEBARINFOEX = 0x033F,
		HANDHELDFIRST = 0x0358,
		HANDHELDLAST = 0x035F,
		AFXFIRST = 0x0360,
		AFXLAST = 0x037F,
		PENWINFIRST = 0x0380,
		PENWINLAST = 0x038F,
		/// <summary>
		/// The WM_APP constant is used by applications to help define private messages, usually of the form WM_APP+X, where X is an integer value.
		/// </summary>
		APP = 0x8000,
		/// <summary>
		/// The WM_USER constant is used by applications to help define private messages for use by private window classes, usually of the form WM_USER+X, where X is an integer value.
		/// </summary>
		USER = 0x0400,

		/// <summary>
		/// An application sends the WM_CPL_LAUNCH message to Windows Control Panel to request that a Control Panel application be started.
		/// </summary>
		CPL_LAUNCH = USER + 0x1000,
		/// <summary>
		/// The WM_CPL_LAUNCHED message is sent when a Control Panel application, started by the WM_CPL_LAUNCH message, has closed. The WM_CPL_LAUNCHED message is sent to the window identified by the wParam parameter of the WM_CPL_LAUNCH message that started the application.
		/// </summary>
		CPL_LAUNCHED = USER + 0x1001,
		/// <summary>
		/// WM_SYSTIMER is a well-known yet still undocumented message. Windows uses WM_SYSTIMER for internal actions like scrolling.
		/// </summary>
		SYSTIMER = 0x118,

		/// <summary>
		/// The accessibility state has changed.
		/// </summary>
		HSHELL_ACCESSIBILITYSTATE = 11,
		/// <summary>
		/// The shell should activate its main window.
		/// </summary>
		HSHELL_ACTIVATESHELLWINDOW = 3,
		/// <summary>
		/// The user completed an input event (for example, pressed an application command button on the mouse or an application command key on the keyboard), and the application did not handle the WM_APPCOMMAND message generated by that input.
		/// If the Shell procedure handles the WM_COMMAND message, it should not call CallNextHookEx. See the Return Value section for more information.
		/// </summary>
		HSHELL_APPCOMMAND = 12,
		/// <summary>
		/// A window is being minimized or maximized. The system needs the coordinates of the minimized rectangle for the window.
		/// </summary>
		HSHELL_GETMINRECT = 5,
		/// <summary>
		/// Keyboard language was changed or a new keyboard layout was loaded.
		/// </summary>
		HSHELL_LANGUAGE = 8,
		/// <summary>
		/// The title of a window in the task bar has been redrawn.
		/// </summary>
		HSHELL_REDRAW = 6,
		/// <summary>
		/// The user has selected the task list. A shell application that provides a task list should return TRUE to prevent Windows from starting its task list.
		/// </summary>
		HSHELL_TASKMAN = 7,
		/// <summary>
		/// A top-level, unowned window has been created. The window exists when the system calls this hook.
		/// </summary>
		HSHELL_WINDOWCREATED = 1,
		/// <summary>
		/// A top-level, unowned window is about to be destroyed. The window still exists when the system calls this hook.
		/// </summary>
		HSHELL_WINDOWDESTROYED = 2,
		/// <summary>
		/// The activation has changed to a different top-level, unowned window.
		/// </summary>
		HSHELL_WINDOWACTIVATED = 4,
		/// <summary>
		/// A top-level window is being replaced. The window exists when the system calls this hook.
		/// </summary>
		HSHELL_WINDOWREPLACED = 13
	}

	/// <summary>
	/// Value type for raw input from a keyboard.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct RAWKEYBOARD
	{
		/// <summary>Scan code for key depression.</summary>
		public short MakeCode;
		/// <summary>Scan code information.</summary>
		public RawKeyboardFlags Flags;
		/// <summary>Reserved.</summary>
		public short Reserved;
		/// <summary>Virtual key code.</summary>
		public VirtualKeys VirtualKey;
		/// <summary>Corresponding window message.</summary>
		public WM Message;
		/// <summary>Extra information.</summary>
		public int ExtraInformation;
	}

	/// <summary>
	/// Value type for raw input from a HID.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct RAWINPUTHID
	{
		/// <summary>Size of the HID data in bytes.</summary>
		public int Size;
		/// <summary>Number of HID in Data.</summary>
		public int Count;
		/// <summary>Data for the HID.</summary>
		public IntPtr Data;
	}

	/// <summary>
	/// Contains the raw input from a device.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct RAWINPUT
	{
		/// <summary>
		/// Header for the data.
		/// </summary>
		public RAWINPUTHEADER Header;
		public Union Data;
		[StructLayout(LayoutKind.Explicit)]
		public struct Union
		{
			/// <summary>
			/// Mouse raw input data.
			/// </summary>
			[FieldOffset(0)]
			public RawMouse Mouse;
			/// <summary>
			/// Keyboard raw input data.
			/// </summary>
			[FieldOffset(0)]
			public RAWKEYBOARD Keyboard;
			/// <summary>
			/// HID raw input data.
			/// </summary>
			[FieldOffset(0)]
			public RAWINPUTHID HID;
		}
	}

	class RawInput
	{
		/// <summary>Function to register a raw input device.</summary>
		/// <param name="pRawInputDevices">Array of raw input devices.</param>
		/// <param name="uiNumDevices">Number of devices.</param>
		/// <param name="cbSize">Size of the RAWINPUTDEVICE structure.</param>
		/// <returns>TRUE if successful, FALSE if not.</returns>
		[DllImport("user32.dll")]
		private static extern bool RegisterRawInputDevices([MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] RAWINPUTDEVICE[] pRawInputDevices, int uiNumDevices, int cbSize);

		/// <summary>
		/// Function to retrieve raw input data.
		/// </summary>
		/// <param name="hRawInput">Handle to the raw input.</param>
		/// <param name="uiCommand">Command to issue when retrieving data.</param>
		/// <param name="pData">Raw input data.</param>
		/// <param name="pcbSize">Number of bytes in the array.</param>
		/// <param name="cbSizeHeader">Size of the header.</param>
		/// <returns>0 if successful if pData is null, otherwise number of bytes if pData is not null.</returns>

		[DllImport("User32.dll", SetLastError = true)]
		private static extern int GetRawInputData(IntPtr hRawInput, RawInputCommand command, [Out] out RAWINPUT buffer, [In, Out] ref int size, int cbSizeHeader);

		[DllImport("User32.dll", SetLastError = true)]
		private static extern int GetRawInputData(IntPtr hRawInput, RawInputCommand command, [Out] IntPtr pData, [In, Out] ref int size, int sizeHeader);

		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		private static extern IntPtr GetForegroundWindow();

		private static Dictionary<VirtualKeys, bool> lastStates = new Dictionary<VirtualKeys, bool>();
		private static Dictionary<VirtualKeys, HashSet<Action>> callbacks = new Dictionary<VirtualKeys, HashSet<Action>>();
		private static Dictionary<VirtualKeys, HashSet<Action>> focusCallbacks = new Dictionary<VirtualKeys, HashSet<Action>>();
		private static VirtualKeys[] password;
		private static Action passwordCallback;
		private static int passwordProgress = 0;

		static IntPtr handle;

		public static void RegisterDevice(HIDUsagePage usagePage, HIDUsage usage, RawInputDeviceFlags deviceFlags, IntPtr handle)
		{
			RawInput.handle = handle;
			RAWINPUTDEVICE device = new RAWINPUTDEVICE() { UsagePage = usagePage, Usage = usage, Flags = deviceFlags, WindowHandle = handle };
			RegisterRawInputDevices(new RAWINPUTDEVICE[1] { device }, 1, Marshal.SizeOf(device));
		}

		public static void ProcessMessage(IntPtr lParam)
		{
			int dwSize = 0;

			GetRawInputData(lParam, RawInputCommand.Input, IntPtr.Zero, ref dwSize, Marshal.SizeOf(typeof(RAWINPUTHEADER)));

			if (dwSize != GetRawInputData(lParam, RawInputCommand.Input, out RAWINPUT buffer, ref dwSize, Marshal.SizeOf(typeof(RAWINPUTHEADER))))
				return;

			VirtualKeys key = buffer.Data.Keyboard.VirtualKey;
			bool pressed = !buffer.Data.Keyboard.Flags.HasFlag(RawKeyboardFlags.KeyBreak);
			lastStates[key] = pressed;
			if (pressed)
			{
				if (callbacks.ContainsKey(key))
					foreach (var callback in callbacks[key])
						callback();

				if (focusCallbacks.ContainsKey(key) && GetForegroundWindow() == handle)
					foreach (var callback in focusCallbacks[key])
						callback();

				if (password != null)
				{
					if (key == password[passwordProgress])
					{
						passwordProgress++;
						if (passwordProgress == password.Length)
						{
							passwordProgress = 0;
							passwordCallback();
						}
					}
					else
					{
						passwordProgress = 0;
					}
				}
			}
		}

		//Very basic event handling. Only supports single key callbacks currently.
		public static void RegisterCallback(VirtualKeys key, Action callback)
		{
			if (callbacks.ContainsKey(key))
				callbacks[key].Add(callback);
			else
				callbacks.Add(key, new HashSet<Action>() { callback });
		}

		public static void RegisterFocusCallback(VirtualKeys key, Action callback)
		{
			if (focusCallbacks.ContainsKey(key))
				focusCallbacks[key].Add(callback);
			else
				focusCallbacks.Add(key, new HashSet<Action>() { callback });
		}

		public static bool IsPressed(VirtualKeys key)
		{
			if (lastStates.ContainsKey(key))
				return lastStates[key];
			return false;
		}

		public static void RegisterPassword(VirtualKeys[] password, Action callback)
		{
			RawInput.password = password;
			passwordCallback = callback;
		}
	}
}
